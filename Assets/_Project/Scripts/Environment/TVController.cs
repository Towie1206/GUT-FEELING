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
    [SerializeField] private int screenMaterialIndex = 2;
    [SerializeField] private Light screenGlowLight;
    [SerializeField] private AudioSource tvStaticAudio;
    [SerializeField] private AudioSource tvNewsAudio;

    [Header("Video Player Setup")]
    [SerializeField] private VideoPlayer videoPlayer;

    [Header("Dynamic Screen Glow")]
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

    private Coroutine glowRoutine;

    private TVChannelState currentChannel = TVChannelState.Off;

    public TVChannelState CurrentChannel => currentChannel;

    private void OnEnable()
    {

    }

    private void Start()
    {
        // TODO:
        // 1. Đảm bảo hiddenWallCamera (nếu có) mặc định tắt (hiddenWallCamera.enabled = false) ở Hồi 1 để tiết kiệm GPU
        // 2. Đăng ký lắng nghe StoryEventBus.Instance.OnStoryFlagChanged += HandleStoryFlagChanged trong Start()
        //    (Đăng ký ở Start() thay vì OnEnable() để tránh lỗi NullReferenceException nếu TVController chạy Awake() trước StoryEventBus!)

        if (hiddenWallCamera != null)
            hiddenWallCamera.enabled = false;
        screenGlowLight.enabled = false;
        StoryEventBus.Instance.OnStoryFlagChanged += HandleStoryFlagChanged;
    }

    private void OnDestroy()
    {
        // TODO:
        // Kiểm tra nếu StoryEventBus.Instance != null thì hủy đăng ký:
        // StoryEventBus.Instance.OnStoryFlagChanged -= HandleStoryFlagChanged

        StoryEventBus.Instance.OnStoryFlagChanged -= HandleStoryFlagChanged;
    }

    private void SetScreenMaterial(Material mat)
    {
        Material[] mats = screenRenderer.sharedMaterials;
        mats[screenMaterialIndex] = mat;
        screenRenderer.sharedMaterials = mats;
    }
    public void TogglePower()
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

        if (StoryEventBus.Instance.HasFlag(StoryFlag.PowerOutageOccurred) && !StoryEventBus.Instance.HasFlag(StoryFlag.PowerRestored))
            return;

        if (currentChannel == TVChannelState.HiddenCameraAct2)
        {
            StoryEventBus.Instance.SetFlag(StoryFlag.CreepyTVBroadcastSeen, true);
            return;
        }

        if (!StoryEventBus.Instance.HasFlag(StoryFlag.TVNewsWatched))
        {
            StoryEventBus.Instance.SetFlag(StoryFlag.TVNewsWatched, true);
            if (watchNewsDialogue != null)
                StoryEventBus.Instance.RequestDialogue(watchNewsDialogue);
        }

        if (currentChannel == TVChannelState.Off)
        {
            currentChannel = TVChannelState.WeatherNewsAct1;
            SetScreenMaterial(weatherNewsMat);
            videoPlayer.Play();
            tvNewsAudio.mute = false;
            screenGlowLight.enabled = true;
            glowRoutine = StartCoroutine(DynamicScreenGlowRoutine());
        }
        else
        {
            currentChannel = TVChannelState.Off;
            SetScreenMaterial(offScreenMat);
            tvNewsAudio.mute = true;
            screenGlowLight.enabled = false;
            StopCoroutine(glowRoutine);
        }

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

        if(flag == StoryFlag.PowerOutageOccurred)
        {
            currentChannel = TVChannelState.Off;
            SetScreenMaterial(offScreenMat);
            screenGlowLight.enabled = false;
            tvNewsAudio.Stop();
            tvStaticAudio.Stop();
        }
        else if (flag == StoryFlag.PowerRestored)
        {
            currentChannel = TVChannelState.HiddenCameraAct2;
            hiddenWallCamera.enabled = true;
            SetScreenMaterial(hiddenCameraMat);
            screenGlowLight.enabled = true;
            tvStaticAudio.Play();
        }
    }

    private IEnumerator DynamicScreenGlowRoutine()
    {
        while (currentChannel != TVChannelState.Off && screenGlowLight != null)
        {
            float noise = Mathf.PerlinNoise(Time.time * glowFlickerSpeed, 0f); // 
            screenGlowLight.intensity = Mathf.Lerp(minGlowIntensity, maxGlowIntensity, noise);
            yield return null;
        }
    }
}
