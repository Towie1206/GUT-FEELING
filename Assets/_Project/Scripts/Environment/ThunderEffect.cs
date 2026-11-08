using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class ThunderEffect : MonoBehaviour
{
    [Header("Impulse & Camera Shake")]
    [SerializeField] private CinemachineImpulseSource impulseSource;

    [Header("Audio & Ducking")]
    [SerializeField] private AudioSource thunderAudioSource;
    [SerializeField] private AudioSource rainAudioSource; // Giảm âm lượng mưa để sấm nổ uy lực
    [SerializeField] private float rainDuckedVolume = 0.15f;
    [SerializeField] private float duckLeadTime = 0.3f; // Hạ tiếng mưa trước khi sấm đánh 0.3s

    [Header("Lightning Flash Setup")]
    [SerializeField] private Light lightningLight; // Đèn ngoài ban công chiếu xuyên cửa kính
    [SerializeField] private float maxIntensity = 12.0f;
    [SerializeField] private float flashDuration = 0.4f;

    private float originalRainVolume = 1.0f;

    private void OnEnable()
    {
        if (StoryEventBus.Instance != null)
        {
            StoryEventBus.Instance.OnThunderRequested += TriggerThunder;
        }
    }

    private void Start()
    {
        // Đảm bảo đăng ký nếu StoryEventBus được khởi tạo sau OnEnable
        if (StoryEventBus.Instance != null)
        {
            StoryEventBus.Instance.OnThunderRequested -= TriggerThunder;
            StoryEventBus.Instance.OnThunderRequested += TriggerThunder;
        }
    }

    private void OnDisable()
    {
        if (StoryEventBus.Instance != null)
        {
            StoryEventBus.Instance.OnThunderRequested -= TriggerThunder;
        }
    }
    private void Awake()
    {
        if (impulseSource == null) impulseSource = GetComponent<CinemachineImpulseSource>();
        if (lightningLight != null) lightningLight.intensity = 0f;
        if (rainAudioSource != null) originalRainVolume = rainAudioSource.volume;
    }

    [ContextMenu("Test Thunder")]
    public void TriggerThunder()
    {
        if (rainAudioSource != null) StartCoroutine(AudioDuckingRoutine());
        if (thunderAudioSource != null) thunderAudioSource.Play();
        if (impulseSource != null) impulseSource.GenerateImpulse();
        if (lightningLight != null) StartCoroutine(FlashLightRoutine());
    }

    private IEnumerator FlashLightRoutine()
    {
        lightningLight.intensity = maxIntensity * 0.5f;
        yield return new WaitForSeconds(0.04f);
        lightningLight.intensity = 0f;
        yield return new WaitForSeconds(0.05f);

        lightningLight.intensity = maxIntensity;
        float elapsedTime = 0f;
        while (elapsedTime < flashDuration)
        {
            elapsedTime += Time.deltaTime;
            lightningLight.intensity = Mathf.Lerp(maxIntensity, 0f, elapsedTime / flashDuration);
            yield return null;
        }
        lightningLight.intensity = 0f;
    }

    private IEnumerator AudioDuckingRoutine()
    {
        rainAudioSource.volume = rainDuckedVolume;
        yield return new WaitForSeconds(duckLeadTime + 1.5f);
        float elapsedTime = 0f;
        while(elapsedTime < duckLeadTime)
        {
            elapsedTime += Time.deltaTime;
            rainAudioSource.volume = Mathf.Lerp(rainDuckedVolume, originalRainVolume, elapsedTime / duckLeadTime);
            yield return null;
        }
        rainAudioSource.volume = originalRainVolume;
    }
}
