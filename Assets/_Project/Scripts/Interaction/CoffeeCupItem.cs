#pragma warning disable CS0414, CS0649
using System.Collections;
using UnityEngine;

/// <summary>
/// Chiếc cốc cà phê/trà nóng của Nam trong đêm mưa bão Hồi 1.
/// Lấy cảm hứng từ cơ chế uống đồ nóng của Fears to Fathom (Video 4 - BatPan).
/// Kế thừa HoldableItemBase (LSP/OCP) và triển khai cả IAltUsable lẫn IHoldInteractable
/// để hỗ trợ tương thích hoàn hảo với hệ thống cầm giữ đồ trên tay (StartHoldHand).
/// </summary>
public class CoffeeCupItem : HoldableItemBase, IAltUsable, IHoldInteractable
{
    [Header("Coffee Visual & Animation (BatPan Video 4)")]
    [SerializeField] private Animator mugAnimator;
    [SerializeField] private Transform liquidMeshTransform; // Bề mặt nước để hạ độ cao khi uống
    [SerializeField] private ParticleSystem hotSteamVFX;    // Khói bốc lên từ cốc nước nóng

    [Header("Audio")]
    [SerializeField] private AudioSource coffeeAudioSource;
    [SerializeField] private AudioClip sipAudioClip;
    [SerializeField] private AudioClip swallowAudioClip;

    [Header("Drinking Settings")]
    [SerializeField] private int maxSips = 2; // Uống 2 ngụm: ngụm 1 vơi nửa cốc, ngụm 2 cạn đáy
    [SerializeField] private float sipCooldown = 1.5f;

    [Header("Story Dialogues")]
    [SerializeField] private DialogueData firstSipDialogue; // "Cà phê nóng làm người tỉnh táo hẳn..."
    [SerializeField] private DialogueData emptyCupDialogue;  // "Hết sạch rồi."

    private int sipsTaken = 0;
    private bool isDrinking = false;
    private Vector3 initialLiquidScale;

    public bool IsEmpty => sipsTaken >= maxSips;
    public int SipsTaken => sipsTaken;

    public override string InteractionPrompt
    {
        get
        {
            if (isHeld) return string.Empty;
            return IsEmpty ? "[E] Nhặt Cốc rỗng" : $"[E] Nhặt Cốc {itemName}";
        }
    }

    protected override void Awake()
    {
        base.Awake();
        if (liquidMeshTransform != null)
        {
            initialLiquidScale = liquidMeshTransform.localScale;
        }
    }

    // =========================================================================
    // 1. HỢP ĐỒNG IHoldInteractable (Giữ chuột phải / phím AltUse để nhấp từng ngụm)
    // =========================================================================

    public float HoldDuration => sipCooldown;
    public string ActionPrompt => "Đang uống cà phê...";
    public bool CanHold => isHeld && !IsEmpty && !isDrinking;

    public void OnHoldStart()
    {
        // TODO:
        // 1. Bật cờ isDrinking = true
        // 2. Kích hoạt Animator: mugAnimator.SetInteger("DrinkStep", sipsTaken + 1) hoặc mugAnimator.SetTrigger("Drink")
        // 3. Phát tiếng húp nước nóng: coffeeAudioSource.PlayOneShot(sipAudioClip)
    }

    public void OnHoldProgress(float normalizedProgress)
    {
        // TODO:
        // Hạ dần mực nước liquidMeshTransform theo tiến trình normalizedProgress:
        // Target Y scale = Lerp từ mức hiện tại xuống mức sau ngụm uống kế tiếp.
    }

    public void OnHoldCancel()
    {
        // TODO:
        // Người chơi nhả phím giữa chừng -> Khôi phục Animator về Idle, đặt isDrinking = false
    }

    public void OnHoldComplete(Player player)
    {
        // TODO:
        // 1. Tăng sipsTaken lên 1
        // 2. Phát tiếng nuốt nước: coffeeAudioSource.PlayOneShot(swallowAudioClip)
        // 3. Cập nhật cố định scale của liquidMeshTransform
        // 4. Nếu sipsTaken >= maxSips -> Tắt hotSteamVFX.Stop() (cốc đã cạn và nguội)
        // 5. Bắn độc thoại cốt truyện:
        //    - sipsTaken == 1: StoryEventBus.Instance.RequestDialogue(firstSipDialogue)
        //    - IsEmpty: StoryEventBus.Instance.RequestDialogue(emptyCupDialogue)
        // 6. Đặt isDrinking = false
    }

    // =========================================================================
    // 2. HỢP ĐỒNG IAltUsable & OnUse (Hỗ trợ click chuột tức thì qua Coroutine)
    // =========================================================================

    public void OnAltUse()
    {
        if (isDrinking || IsEmpty) return;
        StartCoroutine(DrinkCoffeeRoutine());
    }

    public override void OnUse()
    {
        OnAltUse();
    }

    private IEnumerator DrinkCoffeeRoutine()
    {
        isDrinking = true;

        // TODO:
        // 1. OnHoldStart() mô phỏng: Kích hoạt mugAnimator và phát sipAudioClip
        // 2. Chờ thời gian ngụm uống: yield return new WaitForSeconds(sipCooldown)
        // 3. Gọi logic hoàn tất uống OnHoldComplete(null)
        yield return null;
    }
}
