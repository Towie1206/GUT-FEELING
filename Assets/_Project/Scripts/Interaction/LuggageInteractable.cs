using System.Collections;
using UnityEngine;

public class LuggageInteractable : MonoBehaviour, IHoldInteractable
{
    [Header("Luggage Settings")]
    [SerializeField] private float openDuration = 2.0f;
    [SerializeField] private Transform lidTransform;
    [SerializeField] private float openAngle = -110f;
    [SerializeField] private float openSpeed = 2.5f;
    [SerializeField] private GameObject[] itemsInside;
    [SerializeField] private AudioSource zipperSound;

    [Header("Story Event")]
    [SerializeField] private DialogueData afterOpenDialogue;

    private bool isOpened;

    public bool IsOpened => isOpened;
    public float HoldDuration => openDuration;
    public string ActionPrompt => isOpened ? string.Empty : "Đang kéo khóa Vali...";
    public bool CanHold => !isOpened;

    public void OnHoldStart()
    {
        if (zipperSound != null)
            zipperSound.Play();
    }

    public void OnHoldProgress(float normalizedProgress)
    {
        // TODO (Tùy chọn):
        // Có thể rung nhẹ nắp vali hoặc cập nhật hiệu ứng theo normalizedProgress (0.0 -> 1.0)
    }

    public void OnHoldCancel()
    {
        if (zipperSound != null)
            zipperSound.Stop();
    }

    public void OnHoldComplete(Player player)
    {

        if (isOpened == true)
            return;

        isOpened = true;
        StartCoroutine(OpenLidRoutine());
        foreach (var item in itemsInside)
            item.SetActive(true);

        StoryEventBus.Instance.SetFlag(StoryFlag.LuggageOpened, true);
        StoryEventBus.Instance.RequestDialogue(afterOpenDialogue);
    }

    private IEnumerator OpenLidRoutine()
    {
        Quaternion startRotation = lidTransform.localRotation;
        Quaternion targetRotation = startRotation * Quaternion.Euler(openAngle, 0, 0);
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * openSpeed;

            lidTransform.localRotation = Quaternion.Slerp(startRotation, targetRotation, t);
            yield return null;
        }

        lidTransform.localRotation = targetRotation;
    }
}
