#pragma warning disable CS0414, CS0649
using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

/// <summary>
/// Hiệu ứng sấm sét điện ảnh trong đêm bão Hồi 1 & Hồi 2.
/// Tích hợp CinemachineImpulseSource, chớp sáng kép (Pre-flash + Main flash)
/// và Audio Ducking hạ âm lượng mưa trước khi sấm nổ.
/// </summary>
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
        // TODO:
        // Tạo hiệu ứng chớp sét 2 nhịp chân thực:
        // 1. Nhịp mồi (Pre-flash):
        //    - lightningLight.intensity = maxIntensity * 0.5f
        //    - yield return new WaitForSeconds(0.04f)
        //    - lightningLight.intensity = 0f
        //    - yield return new WaitForSeconds(0.05f)
        // 2. Nhịp giáng chính (Main strike):
        //    - lightningLight.intensity = maxIntensity
        //    - Lerp mượt mà từ maxIntensity về 0 theo thời gian flashDuration
        yield return null;
    }

    private IEnumerator AudioDuckingRoutine()
    {
        // TODO:
        // 1. Hạ volume rainAudioSource xuống rainDuckedVolume
        // 2. Chờ tiếng sấm vang lên: yield return new WaitForSeconds(duckLeadTime + 1.5f)
        // 3. Khôi phục dần volume rainAudioSource về originalRainVolume
        yield return null;
    }
}
