using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Controls the sliding glass door leading to the balcony.
/// Follows SOLID principles: manages sliding door movement and passage blocking,
/// while querying story state via StoryEventBus.
/// </summary>
public class BalconyDoor : MonoBehaviour, IInteractable
{
    [Header("Sliding Movement")]
    [Tooltip("The moving panel transform that slides open")]
    [SerializeField] private Transform slidingPanel;

    [Tooltip("Local position of the sliding panel when completely closed")]
    [SerializeField] private Vector3 closedLocalPosition = new Vector3(0f, 0f, -1.05f);

    [Tooltip("Local position of the sliding panel when opened")]
    [SerializeField] private Vector3 openLocalPosition = Vector3.zero;

    [Tooltip("Speed of sliding open / closed")]
    [SerializeField] private float slideSpeed = 2.5f;

    [Header("Doorway Collision")]
    [Tooltip("Collider blocking player from walking through the opening when closed")]
    [SerializeField] private Collider doorBlockerCollider;

    [Header("Audio")]
    [SerializeField] private AudioSource doorAudio;
    [SerializeField] private AudioClip openClip;
    [SerializeField] private AudioClip closeClip;

    [Header("Story Prerequisites")]
    [SerializeField] private StoryFlag requiredNoodleFlag = StoryFlag.NoodleCooked;
    [SerializeField] private StoryFlag requiredTVFlag = StoryFlag.TVNewsWatched;
    [SerializeField] private DialogueData notReadyDialogue;

    private bool isOpen;
    private Coroutine slideCoroutine;

    public bool IsOpen => isOpen;

    public event Action<bool> OnDoorStateChanged;

    public string InteractionPrompt => isOpen ? "[E] Close balcony door" : "[E] Open balcony door";

    private void Awake()
    {
        if (slidingPanel != null)
        {
            slidingPanel.localPosition = closedLocalPosition;
        }

        if (doorBlockerCollider != null)
        {
            doorBlockerCollider.enabled = true;
        }
    }

    public void Interact(Player player)
    {
        // If door is closed, check prerequisites before allowing it to open
        if (!isOpen)
        {
            bool hasNoodle = StoryEventBus.Instance == null || StoryEventBus.Instance.HasFlag(requiredNoodleFlag);
            bool hasTV = StoryEventBus.Instance == null || StoryEventBus.Instance.HasFlag(requiredTVFlag);

            if (!hasNoodle || !hasTV)
            {
                if (notReadyDialogue != null && StoryEventBus.Instance != null)
                {
                    StoryEventBus.Instance.RequestDialogue(notReadyDialogue);
                }
                return;
            }
        }

        ToggleDoor();
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;

        PlayAudio(isOpen ? openClip : closeClip);

        if (slideCoroutine != null)
        {
            StopCoroutine(slideCoroutine);
        }

        Vector3 targetPos = isOpen ? openLocalPosition : closedLocalPosition;
        slideCoroutine = StartCoroutine(SlideRoutine(targetPos, isOpen));

        OnDoorStateChanged?.Invoke(isOpen);
    }

    private IEnumerator SlideRoutine(Vector3 targetLocalPos, bool opening)
    {
        // When opening, unblock the doorway immediately so player can pass through
        if (opening && doorBlockerCollider != null)
        {
            doorBlockerCollider.enabled = false;
        }

        if (slidingPanel != null)
        {
            Vector3 startPos = slidingPanel.localPosition;
            float t = 0f;

            while (t < 1f)
            {
                t += Time.deltaTime * slideSpeed;
                slidingPanel.localPosition = Vector3.Lerp(startPos, targetLocalPos, Mathf.SmoothStep(0f, 1f, t));
                yield return null;
            }

            slidingPanel.localPosition = targetLocalPos;
        }

        // When closing, re-enable doorway collision once fully shut
        if (!opening && doorBlockerCollider != null)
        {
            doorBlockerCollider.enabled = true;
        }

        slideCoroutine = null;
    }

    private void PlayAudio(AudioClip clip)
    {
        if (doorAudio == null) return;

        if (clip != null)
        {
            doorAudio.PlayOneShot(clip);
        }
        else if (doorAudio.clip != null)
        {
            doorAudio.Play();
        }
    }
}
