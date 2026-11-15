using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Cinematic thunder and lightning effect during storm scenes.
/// Integrates CinemachineImpulseSource screen shake, dual-light lightning flash
/// (outdoor directional sky light + indoor ambient bounce light), volumetric exposure burst,
/// ambient sky modulation, and dynamic audio ducking.
/// </summary>
public class ThunderEffect : MonoBehaviour
{
    [Header("Impulse & Camera Shake")]
    [SerializeField] private CinemachineImpulseSource impulseSource;

    [Header("Audio & Ducking")]
    [SerializeField] private AudioSource thunderAudioSource;
    [SerializeField] private AudioSource rainAudioSource;
    [SerializeField] private float rainDuckedVolume = 0.12f;
    [SerializeField] private float duckLeadTime = 0.3f;

    [Header("Lightning Flash Setup")]
    [Tooltip("Primary outdoor sky / directional lightning light casting dramatic shadows")]
    [SerializeField] private Light lightningLight;
    [Tooltip("Indoor ambient/fill point light illuminating the room interior")]
    [SerializeField] private Light indoorFillLight;
    [SerializeField] private float maxIntensity = 45.0f;
    [SerializeField] private float indoorFillIntensity = 30.0f;
    [SerializeField] private float flashDuration = 0.48f;

    [Header("Post-Processing & Ambient Flash")]
    [SerializeField] private Volume postProcessVolume;
    [SerializeField] private float exposureBoost = 1.8f;
    [SerializeField] private Color flashAmbientColor = new Color(0.45f, 0.55f, 0.75f, 1.0f);

    private float originalRainVolume = 1.0f;
    private Color originalAmbient;
    private float originalExposure = -0.8f;
    private ColorAdjustments colorAdjustments;
    private bool hasCachedPostProcessing;

    private Coroutine flashRoutine;
    private Coroutine duckingRoutine;

    private void Awake()
    {
        if (impulseSource == null)
        {
            impulseSource = GetComponent<CinemachineImpulseSource>();
        }

        originalAmbient = RenderSettings.ambientLight;
        CachePostProcessing();
        ResetLights();

        if (rainAudioSource != null)
        {
            originalRainVolume = rainAudioSource.volume;
        }
    }

    private void CachePostProcessing()
    {
        if (hasCachedPostProcessing) return;

        if (postProcessVolume == null)
        {
            postProcessVolume = FindAnyObjectByType<Volume>();
        }

        if (postProcessVolume != null && postProcessVolume.profile != null)
        {
            if (postProcessVolume.profile.TryGet(out colorAdjustments))
            {
                originalExposure = colorAdjustments.postExposure.value;
                hasCachedPostProcessing = true;
            }
        }
    }

    private void OnEnable()
    {
        if (StoryEventBus.Instance != null)
        {
            StoryEventBus.Instance.OnThunderRequested -= TriggerThunder;
            StoryEventBus.Instance.OnThunderRequested += TriggerThunder;
        }
    }

    private void Start()
    {
        if (StoryEventBus.Instance != null)
        {
            StoryEventBus.Instance.OnThunderRequested -= TriggerThunder;
            StoryEventBus.Instance.OnThunderRequested += TriggerThunder;
        }

        CachePostProcessing();
    }

    private void OnDisable()
    {
        if (StoryEventBus.Instance != null)
        {
            StoryEventBus.Instance.OnThunderRequested -= TriggerThunder;
        }

        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
            flashRoutine = null;
        }

        if (duckingRoutine != null)
        {
            StopCoroutine(duckingRoutine);
            duckingRoutine = null;
        }

        ResetLights();

        if (rainAudioSource != null)
        {
            rainAudioSource.volume = originalRainVolume;
        }
    }

    [ContextMenu("Test Thunder")]
    public void TriggerThunder()
    {
        if (rainAudioSource != null)
        {
            if (duckingRoutine != null) StopCoroutine(duckingRoutine);
            duckingRoutine = StartCoroutine(AudioDuckingRoutine());
        }

        if (thunderAudioSource != null)
        {
            thunderAudioSource.Play();
        }

        if (impulseSource != null)
        {
            impulseSource.GenerateImpulse();
        }

        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
            flashRoutine = null;
        }
        flashRoutine = StartCoroutine(FlashLightRoutine());
    }

    private void SetFlashIntensity(float normalizedFactor)
    {
        if (lightningLight != null)
        {
            lightningLight.enabled = normalizedFactor > 0.001f;
            lightningLight.intensity = maxIntensity * normalizedFactor;
        }

        if (indoorFillLight != null)
        {
            indoorFillLight.enabled = normalizedFactor > 0.001f;
            indoorFillLight.intensity = indoorFillIntensity * normalizedFactor;
        }

        if (colorAdjustments != null)
        {
            colorAdjustments.postExposure.value = originalExposure + exposureBoost * normalizedFactor;
        }

        if (normalizedFactor > 0.001f)
        {
            RenderSettings.ambientLight = Color.Lerp(originalAmbient, flashAmbientColor, normalizedFactor);
        }
        else
        {
            RenderSettings.ambientLight = originalAmbient;
        }
    }

    private void ResetLights()
    {
        if (lightningLight != null)
        {
            lightningLight.intensity = 0f;
            lightningLight.enabled = false;
        }

        if (indoorFillLight != null)
        {
            indoorFillLight.intensity = 0f;
            indoorFillLight.enabled = false;
        }

        if (colorAdjustments != null)
        {
            colorAdjustments.postExposure.value = originalExposure;
        }

        RenderSettings.ambientLight = originalAmbient;
    }

    private IEnumerator FlashLightRoutine()
    {
        CachePostProcessing();

        // 1. Initial pre-flash leader stroke
        SetFlashIntensity(0.65f);
        yield return new WaitForSeconds(0.05f);

        // 2. Micro-pause between strokes
        SetFlashIntensity(0.12f);
        yield return new WaitForSeconds(0.03f);

        // 3. Main return stroke - maximum blinding flash
        SetFlashIntensity(1.0f);

        float elapsedTime = 0f;
        while (elapsedTime < flashDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / flashDuration);
            // Power of 1.5 creates persistent, photorealistic lightning decay
            float factor = Mathf.Pow(1f - t, 1.5f);
            SetFlashIntensity(factor);
            yield return null;
        }

        ResetLights();
        flashRoutine = null;
    }

    private IEnumerator AudioDuckingRoutine()
    {
        rainAudioSource.volume = rainDuckedVolume;
        yield return new WaitForSeconds(duckLeadTime + 1.5f);

        float elapsedTime = 0f;
        while (elapsedTime < duckLeadTime)
        {
            elapsedTime += Time.deltaTime;
            rainAudioSource.volume = Mathf.Lerp(rainDuckedVolume, originalRainVolume, elapsedTime / duckLeadTime);
            yield return null;
        }

        rainAudioSource.volume = originalRainVolume;
        duckingRoutine = null;
    }
}
