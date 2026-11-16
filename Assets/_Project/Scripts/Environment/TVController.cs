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
        if (hiddenWallCamera != null)
            hiddenWallCamera.enabled = false;
        screenGlowLight.enabled = false;
        StoryEventBus.Instance.OnStoryFlagChanged += HandleStoryFlagChanged;
    }

    private void OnDestroy()
    {
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

        if(flag == StoryFlag.PowerOutageOccurred)
        {
            currentChannel = TVChannelState.Off;
            SetScreenMaterial(offScreenMat);
            screenGlowLight.enabled = false;
            if (glowRoutine != null) StopCoroutine(glowRoutine);
            if (videoPlayer != null) videoPlayer.Stop();
            if (tvNewsAudio != null) tvNewsAudio.Stop();
            if (tvStaticAudio != null) tvStaticAudio.Stop();
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
