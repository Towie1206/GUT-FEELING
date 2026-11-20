using System.Collections;
using UnityEngine;

public class PowerOutageManager : MonoBehaviour
{

    [Header("Apartment Lighting")]
    [SerializeField] private Light[] apartmentLights;
    [SerializeField] private Renderer[] emissiveBulbRenderers;

    [Header("Outage Scare Effects")]
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
        StoryEventBus.Instance.OnStoryFlagChanged += HandleStoryFlagChanged;
    }

    private void OnDestroy()
    {
        if (StoryEventBus.Instance != null)
            StoryEventBus.Instance.OnStoryFlagChanged -= HandleStoryFlagChanged;
    }

    private void HandleStoryFlagChanged(StoryFlag flag)
    {
        if (flag == StoryFlag.PowerOutageOccurred)
        {
            StartCoroutine(TriggerPowerOutageRoutine());
        }
        else if (flag == StoryFlag.PowerRestored)
        {
            RestoreApartmentPower();
        }
        else
            return;
    }

    private IEnumerator TriggerPowerOutageRoutine()
    {
        isPowerOut = true;

        if (breakerTripAudio != null) breakerTripAudio.Play();
        StoryEventBus.Instance.RequestThunder();
        foreach (var light in apartmentLights) { if (light != null) light.enabled = false; }
        if (glassCupToKnockOver != null)
        {
            glassCupToKnockOver.isKinematic = false;
            glassCupToKnockOver.AddForce(cupKnockForce, ForceMode.Impulse);
        }

        foreach (var renderer in emissiveBulbRenderers)
        {
            if (renderer != null)
                renderer.enabled = false;
        }

        yield return new WaitForSeconds(1.8f);

        StoryEventBus.Instance.RequestDialogue(outageReactionDialogue);
    }

    public void RestoreApartmentPower()
    {
        // TODO:
        // 1. Đặt isPowerOut = false
        // 2. Phát âm thanh có điện lại (powerRestoreAudio.Play())
        // 3. Bật lại toàn bộ đèn trong mảng apartmentLights
        // 4. Bắn độc thoại: StoryEventBus.Instance.RequestDialogue(powerRestoredDialogue)
        isPowerOut = false;
        if (powerRestoreAudio != null) powerRestoreAudio.Play();
        foreach (var light in apartmentLights) { if (light != null) light.enabled = true; }
        foreach (var renderer in emissiveBulbRenderers) { if (renderer != null) renderer.enabled = true; }

        StoryEventBus.Instance.RequestDialogue(powerRestoredDialogue);
    }
}
