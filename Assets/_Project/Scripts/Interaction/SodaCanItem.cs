using System.Collections;
using UnityEngine;

public class SodaCanItem : HoldableItemBase
{
    [Header("Sip Pose (local space của heldPoint)")]
    [SerializeField] private Vector3 sipLocalPos = new(0.05f, -0.04f, -0.10f);
    [Tooltip("Mỗi phần tử = góc nghiêng của 1 ngụm. Ngụm sau nghiêng nhiều hơn vì lon vơi.")]
    [SerializeField] private Vector3[] sipLocalEulerPerSip;
    [SerializeField] private float raiseDuration = 0.85f;
    [SerializeField] private float sipHoldDuration = 1.5f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip openCanClip; // "tách... xì" chỉ phát ở ngụm đầu
    [SerializeField] private AudioClip sipClip;

    [Header("Story")]
    [SerializeField] private DialogueData firstSipDialogue;

    private int sipsTaken;
    private bool isDrinking;
    private bool isOpened;

    public bool IsHalfEmpty => sipsTaken >= sipLocalEulerPerSip.Length;

    public override string InteractionPrompt => isHeld ? string.Empty : "[E] Pick up soda can";

    public override void OnUse()
    {
        if (!isHeld || isDrinking || IsHalfEmpty)
            return;
        StartCoroutine(SipRoutine());
    }

    public override void OnSway(float lookDelta)
    {
        if (isDrinking)
            return;
        base.OnSway(lookDelta);
    }

    private IEnumerator SipRoutine()
    {
        isDrinking = true;
        if (!isOpened)
        {
            isOpened = true;
            if (audioSource != null && openCanClip != null)
            {
                audioSource.PlayOneShot(openCanClip);
                yield return new WaitForSeconds(openCanClip.length);
            }
        }

        float elapsed = 0f;
        Vector3 startPos = transform.localPosition;
        Quaternion startRot = transform.localRotation;
        while (elapsed < raiseDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / raiseDuration);
            transform.localPosition = Vector3.Lerp(startPos, sipLocalPos, t);
            transform.localRotation = Quaternion.Slerp(startRot, Quaternion.Euler(sipLocalEulerPerSip[sipsTaken]), t);
            yield return null;
        }

        if (audioSource != null && sipClip != null)
            audioSource.PlayOneShot(sipClip);
        elapsed = 0f;
        while (elapsed < sipHoldDuration)
        {
            elapsed += Time.deltaTime;
            float bob = Mathf.Sin(Time.time * 18f) * 2.5f;
            transform.localRotation = Quaternion.Euler(sipLocalEulerPerSip[sipsTaken].x, sipLocalEulerPerSip[sipsTaken].y, sipLocalEulerPerSip[sipsTaken].z + bob);
            yield return null;
        }

        elapsed = 0f;
        startPos = transform.localPosition;
        startRot = transform.localRotation;
        while (elapsed < raiseDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / raiseDuration);
            transform.localPosition = Vector3.Lerp(startPos, heldPositionOffset, t);
            transform.localRotation = Quaternion.Slerp(startRot, Quaternion.Euler(heldRotationOffset), t);
            yield return null;
        }
        isDrinking = false;
        sipsTaken++;
        if (sipsTaken == 1)
            StoryEventBus.Instance.RequestDialogue(firstSipDialogue);

    }
}