using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class TVController : MonoBehaviour
{
    public enum TVChannelState
    {
        Off,
        WeatherNewsAct1,
        HiddenCameraAct2
    }

    [Header("TV Hardware & Screen")]
    [SerializeField] private MeshRenderer screenRenderer;
    [SerializeField] private int screenMaterialIndex = 0;
    [SerializeField] private Light screenGlowLight;
    [SerializeField] private AudioSource tvStaticAudio;
    [SerializeField] private AudioSource newsBroadcastAudio;

    [Header("Video Player Setup (BatPan Video 5)")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private VideoClip weatherNewsClip;
    [SerializeField] private RenderTexture tvRenderTexture;

    [Header("Dynamic Screen Glow (BatPan Video 5)")]
    [SerializeField] private float minGlowIntensity = 0.6f;
    [SerializeField] private float maxGlowIntensity = 2.2f;
    [SerializeField] private float glowFlickerSpeed = 14f;

    [Header("Channel Materials")]
    [SerializeField] private Material offScreenMat;
    [SerializeField] private Material weatherNewsMat;
    [SerializeField] private Material hiddenCameraMat; // Gắn RenderTexture của Camera giấu sau bức tranh!

    [Header("Hidden Camera Reference (Act 2)")]
    [SerializeField] private Camera hiddenWallCamera;

    [Header("Story Dialogues")]
    [SerializeField] private DialogueData watchNewsDialogue;
    [SerializeField] private DialogueData creepyHiddenCamDialogue;

    private TVChannelState currentChannel = TVChannelState.Off;
    private Coroutine glowRoutine;

    public TVChannelState CurrentChannel => currentChannel;

    public string InteractionPrompt
    {
        get
        {
            // TODO:
            // - Nếu đang ở chế độ HiddenCameraAct2 (Hồi 2): "[E] Xem kỹ màn hình Tivi"
            // - Nếu đang Off: "[E] Bật Tivi"
            // - Nếu đang WeatherNewsAct1: "[E] Tắt Tivi"
            return currentChannel == TVChannelState.Off ? "[E] Bật Tivi" : "[E] Tắt Tivi";
        }
    }

    private void Start()
    {
        // TODO:
        // 1. Đảm bảo hiddenWallCamera (nếu có) mặc định tắt (hiddenWallCamera.enabled = false) ở Hồi 1 để tiết kiệm GPU
        // 2. Đăng ký lắng nghe StoryEventBus.Instance.OnStoryFlagChanged += HandleStoryFlagChanged trong Start()
        //    (Đăng ký ở Start() thay vì OnEnable() để tránh lỗi NullReferenceException nếu TVController chạy Awake() trước StoryEventBus!)
    }

    private void OnDestroy()
    {
        // TODO:
        // Kiểm tra nếu StoryEventBus.Instance != null thì hủy đăng ký:
        // StoryEventBus.Instance.OnStoryFlagChanged -= HandleStoryFlagChanged
    }

    public void Interact(Player player)
    {
        // TODO:
        // 1. Nếu đã mất điện (HasFlag(PowerOutageOccurred) && !HasFlag(PowerRestored)) -> Không bật được Tivi, thoát hàm
        // 2. Nếu đang ở kênh HiddenCameraAct2 (Hồi 2):
        //    - Bật cờ StoryEventBus.Instance.SetFlag(StoryFlag.CreepyTVBroadcastSeen, true)
        //    - Bắn độc thoại kinh hoàng của Nam (creepyHiddenCamDialogue: "Góc quay này... là từ bức tranh sau lưng mình?!")
        //    - return;
        // 3. Ở Hồi 1: Đảo trạng thái giữa Off và WeatherNewsAct1:
        //    - Nếu bật WeatherNewsAct1:
        //      + Đổi Material của screenRenderer thành weatherNewsMat
        //      + Bật screenGlowLight.enabled = true và khởi chạy glowRoutine = StartCoroutine(DynamicScreenGlowRoutine())
        //      + Nếu có videoPlayer: videoPlayer.clip = weatherNewsClip; videoPlayer.targetTexture = tvRenderTexture; videoPlayer.Play()
        //      + Nếu bật lần đầu (!StoryEventBus.Instance.HasFlag(StoryFlag.TVNewsWatched)):
        //        * StoryEventBus.Instance.SetFlag(StoryFlag.TVNewsWatched, true)
        //        * StoryEventBus.Instance.RequestDialogue(watchNewsDialogue)
        //    - Nếu tắt (Off):
        //      + Gán screenRenderer.material = offScreenMat
        //      + Tắt screenGlowLight.enabled = false, dừng glowRoutine
        //      + Nếu có videoPlayer: videoPlayer.Stop()
    }

    private void HandleStoryFlagChanged(StoryFlag flag)
    {
        // TODO:
        // 1. Khi flag == StoryFlag.PowerOutageOccurred (Sập nguồn đầu Hồi 2):
        //    - Cưỡng bức chuyển currentChannel = TVChannelState.Off
        //    - Gán offScreenMat, tắt screenGlowLight, dừng videoPlayer và mọi AudioSource
        // 2. Khi flag == StoryFlag.PowerRestored (Cắm đủ cầu chì bật lại điện):
        //    - Tự động chuyển currentChannel = TVChannelState.HiddenCameraAct2
        //    - Bật hiddenWallCamera.enabled = true (chỉ render Camera phụ khi cần để tối ưu GPU!)
        //    - Gán hiddenCameraMat lên màn hình Tivi, bật screenGlowLight và phát tiếng rè tvStaticAudio
    }

    private IEnumerator DynamicScreenGlowRoutine()
    {
        // TODO:
        // Giả lập ánh sáng hắt từ màn hình CRT nhấp nháy theo nhịp video:
        // Trong khi currentChannel != TVChannelState.Off && screenGlowLight != null:
        // screenGlowLight.intensity = Mathf.Lerp(minGlowIntensity, maxGlowIntensity, Mathf.PerlinNoise(Time.time * glowFlickerSpeed, 0f));
        // yield return null;
        yield return null;
    }
}
