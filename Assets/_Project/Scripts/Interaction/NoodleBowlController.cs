using System.Collections;
using UnityEngine;

public class NoodleBowlController : HoldableItemBase, IHoldInteractable
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
    public bool CanHold => isHeld && (currentState == NoodleState.PouredHotWater || currentState == NoodleState.EatingInProgress) && !isChewingLockout && Time.frameCount > stateChangedFrame;

    protected override void Awake()
    {
        base.Awake();
        itemName = "Mì Ly Handy Hảo Hảo";
        placeableID = "NoodleBowl";
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
    public string ActionPrompt => $"Đang ăn miếng {currentBite + 1}...";


    public void OnHoldStart()
    {
        if (slurpEatAudio != null)
            slurpEatAudio.Play();
    }

    public void OnHoldProgress(float normalizedProgress)
    {
        // Chỉ kích hoạt ĐÚNG 1 LẦN khi đang ăn dở miếng 2 và chạm mốc 50%:
        if (currentState != NoodleState.HalfEaten && currentBite == 1 && normalizedProgress >= 0.5f)
        {
            currentState = NoodleState.HalfEaten;
            if (biteProgressMeshes != null && biteProgressMeshes.Length > 0 && biteProgressMeshes[0] != null)
                biteProgressMeshes[0].SetActive(false);
            if (halfEatenMesh != null)
                halfEatenMesh.SetActive(true);

            StoryEventBus.Instance.RequestThunder();

            if (nemWarningSMS != null)
                StoryEventBus.Instance.RequestSMS(nemWarningSMS);
            if (afterEatDialogue != null)
                StoryEventBus.Instance.RequestDialogue(afterEatDialogue);
        }
    }

    public void OnHoldCancel()
    {
        if (slurpEatAudio != null)
            slurpEatAudio.Stop();
    }

    public void OnHoldComplete(Player player)
    {
        // TODO:
        // 1. Tăng currentBite lên 1
        // 2. Cập nhật mesh: ẩn mức cũ, bật biteProgressMeshes[currentBite - 1] (kế thừa từ BatPan pizzapieces[])
        // 3. Khởi chạy Coroutine khóa thao tác tạm thời: StartCoroutine(ChewLockoutRoutine())
        // 4. Nếu currentBite == 1:
        //    - currentState = NoodleState.EatingInProgress
        //    - Bắn độc thoại firstBiteDialogue ("Nóng hổi vừa thổi vừa ăn...")
        // 5. Nếu currentBite >= maxAct1Bites:
        //    - currentState = NoodleState.HalfEaten (Dừng lại chừa nửa bát!)
        //    - Bật halfEatenMesh
        //    - Bật cờ: StoryEventBus.Instance.SetFlag(StoryFlag.NoodleCooked, true)
        //    - Bắn độc thoại afterEatDialogue ("Mưa to quá, nghe tiếng gió đập ngoài ban công...")
        //    - Bắn tin nhắn cảnh báo của Nem: StoryEventBus.Instance.RequestSMS(nemWarningSMS)

        currentBite++;
        currentState = NoodleState.EatingInProgress;
        if (cookedNoodleMesh != null)
            cookedNoodleMesh.SetActive(false);
        if (biteProgressMeshes != null && biteProgressMeshes.Length > 0 && biteProgressMeshes[0] != null)
            biteProgressMeshes[0].SetActive(true);

        StartCoroutine(ChewLockoutRoutine());
        StoryEventBus.Instance.RequestDialogue(firstBiteDialogue);

    }

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
}
