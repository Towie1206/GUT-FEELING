using System.Collections;
using UnityEngine;

public class NoodleBowlController : HoldableItemBase
{
    public enum NoodleState
    {
        Dry,
        PouringWater,
        PouredHotWater,
        EatingInProgress,
        HalfEaten,
        EmptyByStalker
    }

    [Header("Visual Meshes")]
    [SerializeField] private GameObject lidClosedMesh;
    [SerializeField] private GameObject lidPeeledMesh;
    [SerializeField] private GameObject dryNoodleMesh;
    [SerializeField] private GameObject cookedNoodleMesh;
    [SerializeField] private GameObject[] biteProgressMeshes; // Mảng mesh các mức vơi của bát mì (BatPan pizzapieces[])
    [SerializeField] private GameObject halfEatenMesh;
    [SerializeField] private GameObject emptyStalkerMesh;
    [SerializeField] private GameObject mysteriousHalfDrunkCup; // Cốc nước lạ xuất hiện cạnh bát mì ở Hồi 2!
    [SerializeField] private ParticleSystem hotSteamVFX;
    [SerializeField] private ParticleSystem pourWaterVFX;

    [Header("Audio")]
    [SerializeField] private AudioSource pourWaterAudio;
    [SerializeField] private AudioSource slurpEatAudio;

    [Header("Eating Settings")]
    [SerializeField] private float eatHoldDuration = 3.5f;
    [SerializeField] private float chewCooldown = 2f; // Khóa tương tác giữa các miếng để tránh spam (CanInteract delay)
    [SerializeField] private int maxAct1Bites = 2;       // Nam chỉ gắp 2 miếng rồi chừa lại nửa bát (HalfEaten)

    [Header("Story & SMS")]
    [SerializeField] private DialogueData needBoiledWaterDialogue;
    [SerializeField] private DialogueData firstBiteDialogue;
    [SerializeField] private DialogueData afterEatDialogue;
    [SerializeField] private SMSData nemWarningSMS;
    [SerializeField] private DialogueData stalkerAteNoodleDialogue;

    [Header("Pouring Cutscene Points")]
    [SerializeField] private Transform pourPoint;
    [SerializeField] private Transform potRestPoint;
    [SerializeField] private float moveToPourDuration = 1.5f;
    [SerializeField] private float pourHoldDuration = 3f;
    [SerializeField] private float moveToRestDuration = 1.5f;

    private NoodleState currentState = NoodleState.Dry;
    private int currentBite = 0;
    private bool isChewingLockout = false;
    private int stateChangedFrame = -1;

    public NoodleState CurrentState => currentState;
    public int CurrentBite => currentBite;

    [SerializeField] private Vector3 eatLocalPos = new Vector3(0.05f, -0.04f, -0.12f);
    [SerializeField] private Vector3 eatLocalEuler = new Vector3(0f, 0f, 45f);
    [SerializeField] private float raiseDuration = 0.35f;
    private bool isEating = false;

    protected override void Awake()
    {
        base.Awake();
        itemName = "Mì Ly Handy Hảo Hảo";
        placeableID = "NoodleBowl";

        heldPositionOffset = new Vector3(0.08f, -0.07f, -0.02f);
        heldRotationOffset = new Vector3(-8f, 0f, 25f);
    }

    public override string InteractionPrompt
    {
        get
        {
            if (isHeld) return string.Empty;
            if (currentState == NoodleState.Dry) return "[E] Pour hot water";
            if (currentState == NoodleState.PouredHotWater || currentState == NoodleState.EatingInProgress)
                return "[E] Pick up noodle cup";
            if (currentState == NoodleState.EmptyByStalker) return "[E] Inspect noodle cup";
            return string.Empty;
        }
    }

    private void OnEnable()
    {
        StoryEventBus.Instance.OnStoryFlagChanged += HandleStoryFlagChanged;
    }

    private void OnDisable()
    {
        StoryEventBus.Instance.OnStoryFlagChanged -= HandleStoryFlagChanged;
    }

    public override void Interact(Player player)
    {
        // TODO:
        // TRƯỜNG HỢP 1: currentState == NoodleState.Dry
        // - Kiểm tra xem trên tay người chơi (player.hand.CurrentItemHold) có đang cầm KettleItem đã đun sôi (kettle.IsBoiled) không
        // - Nếu ĐÚNG:
        //   + Chuyển currentState = NoodleState.PouredHotWater
        //   + Lưu lại stateChangedFrame = Time.frameCount (Chặn xung đột phím [E] trong cùng 1 frame với StartHoldWorld!)
        //   + Ẩn lidClosedMesh, hiện lidPeeledMesh (bóc nắp mì ra)
        //   + Ẩn dryNoodleMesh, hiện cookedNoodleMesh, bật khói hotSteamVFX, phát tiếng pourWaterAudio
        // - Nếu SAI:
        //   + Bắn độc thoại needBoiledWaterDialogue ("Phải đun nước sôi mới pha mì được...")
        //
        // TRƯỜNG HỢP 2: currentState == NoodleState.EmptyByStalker (Ở Hồi 2 sau khi bật cầu dao)
        // - Bật cờ StoryEventBus.Instance.SetFlag(StoryFlag.HalfEatenNoodleSeen, true)
        // - Bắn độc thoại kinh hãi của Nam (stalkerAteNoodleDialogue: "Cái gì thế này?! Ai đã ăn sạch nửa bát mì của mình trong lúc mất điện?!")
        switch (currentState)
        {
            case NoodleState.Dry:
                if (player.hand.CurrentItemHold is CookingPotItem pot && pot.IsBoiled)
                {
                    player.hand.ReleaseCurrentItem();
                    pot.DetachForPouring();
                    StartCoroutine(PourWaterRoutine(pot));
                }
                else
                {
                    StoryEventBus.Instance.RequestDialogue(needBoiledWaterDialogue);
                }
                break;
            case NoodleState.EmptyByStalker:
                break;
            case NoodleState.PouredHotWater:
                base.Interact(player); // bên holdableitembase sẽ gọi player.hand.equip(this)
                break;
            case NoodleState.EatingInProgress:
                base.Interact(player); // bên holdableitembase sẽ gọi player.hand.equip(this)
                break;
        }

    }

    private IEnumerator MovePotRoutine(Transform potTransform, Transform targetTransform, float duration)
    {
        Vector3 startPos = potTransform.position;
        Quaternion startRot = potTransform.rotation;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            potTransform.position = Vector3.Lerp(startPos, targetTransform.position, t);
            potTransform.rotation = Quaternion.Slerp(startRot, targetTransform.rotation, t);
            yield return null;
        }
        potTransform.position = targetTransform.position;
        potTransform.rotation = targetTransform.rotation;
    }

    private IEnumerator PourWaterRoutine(CookingPotItem pot)
    {
        currentState = NoodleState.PouringWater;

        if (lidClosedMesh != null) lidClosedMesh.SetActive(false);
        if (lidPeeledMesh != null) lidPeeledMesh.SetActive(true);

        yield return MovePotRoutine(pot.transform, pourPoint, moveToPourDuration);

        if (pourWaterVFX != null) pourWaterVFX.Play();
        if (pourWaterAudio != null) pourWaterAudio.Play();

        yield return new WaitForSeconds(pourHoldDuration);

        pot.EmptyWater();
        if (pourWaterVFX != null) pourWaterVFX.Stop();
        if (pourWaterAudio != null) pourWaterAudio.Stop();

        if (dryNoodleMesh != null) dryNoodleMesh.SetActive(false);
        if (cookedNoodleMesh != null) cookedNoodleMesh.SetActive(true);
        if (hotSteamVFX != null) hotSteamVFX.Play();

        yield return MovePotRoutine(pot.transform, potRestPoint, moveToRestDuration);

        stateChangedFrame = Time.frameCount;
        currentState = NoodleState.PouredHotWater;
    }


    public float HoldDuration => eatHoldDuration;
    public string ActionPrompt => $"Eating {currentBite + 1}...";



    private IEnumerator ChewLockoutRoutine()
    {
        isChewingLockout = true;
        yield return new WaitForSeconds(chewCooldown);
        isChewingLockout = false;
    }

    private void HandleStoryFlagChanged(StoryFlag flag)
    {
        // TODO (Chuẩn bị cho Hồi 2):
        // Khi flag == StoryFlag.PowerRestored:
        // - Chuyển currentState = NoodleState.EmptyByStalker
        // - Tắt halfEatenMesh, bật emptyStalkerMesh
        // - Bật cốc nước lạ uống dở bên cạnh (mysteriousHalfDrunkCup.SetActive(true))
    }

    public override void OnPlace(Transform socketPoint)
    {
        base.OnPlace(socketPoint);
        if (currentState == NoodleState.HalfEaten)
        {
            StoryEventBus.Instance.SetFlag(StoryFlag.NoodleCooked, true);
        }
    }

    public override void OnSway(float lookDelta)
    {
        if (isEating)
            return;
        base.OnSway(lookDelta);
    }

    public override void OnUse()
    {
        base.OnUse();
        if (!isHeld || isEating || isChewingLockout || (currentState != NoodleState.PouredHotWater && currentState != NoodleState.EatingInProgress))
            return;

        StartCoroutine(EatBiteRoutine());
    }

    private IEnumerator EatBiteRoutine()
    {
        isEating = true;
        float elapsed = 0f;

        while (elapsed < raiseDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / raiseDuration);
            transform.localPosition = Vector3.Lerp(heldPositionOffset, eatLocalPos, t);
            transform.localRotation = Quaternion.Slerp(Quaternion.Euler(heldRotationOffset), Quaternion.Euler(eatLocalEuler), t);
            yield return null;
        }
        if (slurpEatAudio != null)
            slurpEatAudio.Play();
        elapsed = 0f;
        while (elapsed < eatHoldDuration)
        {
            elapsed += Time.deltaTime;
            float bob = Mathf.Sin(Time.time * 18f) * 2.5f; // Hiệu ứng nhấp nhô khi ăn\
            transform.localRotation = Quaternion.Euler(eatLocalEuler.x, eatLocalEuler.y, eatLocalEuler.z + bob);
            yield return null;
            if (currentBite == 1 && elapsed >= eatHoldDuration * 0.5f)
            {
                break;
            }
        }
        if (slurpEatAudio != null)
            slurpEatAudio.Stop();
        currentBite++;
        elapsed = 0f;
        if (currentBite == 1)
        {
            currentState = NoodleState.EatingInProgress;
            cookedNoodleMesh.SetActive(false);
            biteProgressMeshes[0].SetActive(true);
            StoryEventBus.Instance.RequestDialogue(firstBiteDialogue);
        }
        if (currentBite >= 2)
        {
            currentState = NoodleState.HalfEaten;
            biteProgressMeshes[0].SetActive(false);
            halfEatenMesh.SetActive(true);
            StoryEventBus.Instance.RequestThunder();
        }
        Quaternion startRot = transform.localRotation;
        while (elapsed < raiseDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / raiseDuration);
            transform.localPosition = Vector3.Lerp(eatLocalPos, heldPositionOffset, t);
            transform.localRotation = Quaternion.Slerp(startRot, Quaternion.Euler(heldRotationOffset), t);
            yield return null;
        }
        transform.localPosition = heldPositionOffset;
        transform.localRotation = Quaternion.Euler(heldRotationOffset);
        isEating = false;
        StartCoroutine(ChewLockoutRoutine());
        if (currentBite >= 2) 
        {
            StoryEventBus.Instance.RequestSMS(nemWarningSMS);
            StoryEventBus.Instance.RequestDialogue(afterEatDialogue);
        }
    }
}
