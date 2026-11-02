#pragma warning disable CS0414, CS0649
using System.Collections;
using UnityEngine;

/// <summary>
/// Quản lý sự kiện Sập nguồn mất điện toàn căn hộ mở màn Hồi 2 (Tuần 14 - 15)
/// và Khôi phục ánh sáng khi người chơi giải xong câu đố Hộp cầu dao (FuseBoxPuzzle).
/// Tuân thủ SRP & DIP: Giao tiếp hoàn toàn qua StoryEventBus.
/// </summary>
public class PowerOutageManager : MonoBehaviour
{
    [Header("Trigger Delay After Act 1")]
    [SerializeField] private float delayAfterBalconyCutscene = 4.0f;

    [Header("Apartment Lighting")]
    [SerializeField] private Light[] apartmentLights;
    [SerializeField] private Renderer[] emissiveBulbRenderers;

    [Header("Outage Scare Effects")]
    [SerializeField] private ThunderEffect thunderEffect;
    [SerializeField] private AudioSource breakerTripAudio;
    [SerializeField] private AudioSource powerRestoreAudio;
    [SerializeField] private Rigidbody glassCupToKnockOver;
    [SerializeField] private Vector3 cupKnockForce = new Vector3(1.5f, 0.2f, -1.0f);
    [SerializeField] private AudioSource glassSpillAudio;

    [Header("Story Feedback")]
    [SerializeField] private DialogueData outageReactionDialogue;
    [SerializeField] private DialogueData powerRestoredDialogue;

    private bool isPowerOut;

    public bool IsPowerOut => isPowerOut;

    private void Start()
    {
        // TODO:
        // Đăng ký lắng nghe StoryEventBus.Instance.OnStoryFlagChanged += HandleStoryFlagChanged trong Start()
        // (Đảm bảo StoryEventBus.Instance đã được khởi tạo xong trong Awake() trước khi đăng ký!)
    }

    private void OnDestroy()
    {
        // TODO:
        // Kiểm tra nếu StoryEventBus.Instance != null thì hủy đăng ký:
        // StoryEventBus.Instance.OnStoryFlagChanged -= HandleStoryFlagChanged
    }

    private void HandleStoryFlagChanged(StoryFlag flag)
    {
        // TODO:
        // 1. Khi flag == StoryFlag.SeenStrangerInRain (Vừa đóng cửa ban công cuối Hồi 1):
        //    - Gọi StartCoroutine(TriggerPowerOutageRoutine())
        // 2. Khi flag == StoryFlag.PowerRestored (Vừa gạt cầu dao có điện lại ở Hồi 2):
        //    - Gọi RestoreApartmentPower()
    }

    private IEnumerator TriggerPowerOutageRoutine()
    {
        // TODO:
        // 1. Chờ một nhịp ngắn sau Cutscene ban công: yield return new WaitForSeconds(delayAfterBalconyCutscene);
        // 2. Đặt isPowerOut = true
        // 3. Kích hoạt sấm nổ lớn + rung màn hình qua thunderEffect.TriggerThunder()
        // 4. Phát tiếng nổ cầu dao (breakerTripAudio.Play()) và tắt toàn bộ đèn trong mảng apartmentLights
        // 5. Hất đổ cốc nước trên bàn (bật isKinematic = false cho glassCupToKnockOver, AddForce(cupKnockForce, ForceMode.Impulse))
        //    kèm âm thanh đổ vỡ/tràn nước (glassSpillAudio)
        // 6. Bật cờ cốt truyện: StoryEventBus.Instance.SetFlag(StoryFlag.PowerOutageOccurred, true)
        // 7. Bắn độc thoại của Nam: StoryEventBus.Instance.RequestDialogue(outageReactionDialogue)
        //    ("Mất điện rồi?! Phải mở ngăn kéo tìm đèn pin rồi ra kho kiểm tra hộp cầu dao thôi...")
        yield return null;
    }

    public void RestoreApartmentPower()
    {
        // TODO:
        // 1. Đặt isPowerOut = false
        // 2. Phát âm thanh có điện lại (powerRestoreAudio.Play())
        // 3. Bật lại toàn bộ đèn trong mảng apartmentLights
        // 4. Bắn độc thoại: StoryEventBus.Instance.RequestDialogue(powerRestoredDialogue)
    }
}
