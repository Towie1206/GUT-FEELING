using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// Attached to the balcony railing interactable zone.
/// Prompts the player to look down into the dark alley, triggering
/// the pivotal horror cutscene where the squatter is revealed.
/// </summary>
public class BalconyCutsceneTrigger : MonoBehaviour, IInteractable
{
    [Header("Story Prerequisites")]
    [SerializeField] private StoryFlag requiredNoodleFlag = StoryFlag.NoodleCooked;
    [SerializeField] private StoryFlag requiredTVFlag = StoryFlag.TVNewsWatched;
    [SerializeField] private DialogueData notReadyDialogue;

    [Header("Door Dependency")]
    [Tooltip("Balcony door must be open before player can look over railing")]
    [SerializeField] private BalconyDoor balconyDoor;

    [Header("Cutscene References")]
    [SerializeField] private PlayableDirector balconyTimeline;
    [SerializeField] private StoryFlag flagOnComplete = StoryFlag.SeenStrangerInRain;
    [SerializeField] private DialogueData afterCutsceneDialogue;

    [Header("Prompt Configuration")]
    [SerializeField] private string lookDownPrompt = "[E] Look down";

    private bool hasTriggered;

    public bool HasTriggered => hasTriggered;

    public string InteractionPrompt
    {
        get
        {
            if (hasTriggered)
            {
                return string.Empty;
            }

            // Must have door open
            if (balconyDoor != null && !balconyDoor.IsOpen)
            {
                return string.Empty;
            }

            // Story prerequisites check
            if (StoryEventBus.Instance != null)
            {
                if (!StoryEventBus.Instance.HasFlag(requiredNoodleFlag) || !StoryEventBus.Instance.HasFlag(requiredTVFlag))
                {
                    return string.Empty;
                }
            }

            return lookDownPrompt;
        }
    }

    public void Interact(Player player)
    {
        if (hasTriggered)
        {
            return;
        }

        if (balconyDoor != null && !balconyDoor.IsOpen)
        {
            return;
        }

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

        hasTriggered = true;

        if (balconyTimeline != null && CutsceneController.Instance != null)
        {
            CutsceneController.Instance.PlayTimeline(balconyTimeline, OnCutsceneFinished);
        }
    }

    private void OnCutsceneFinished()
    {
        if (StoryEventBus.Instance != null)
        {
            StoryEventBus.Instance.SetFlag(flagOnComplete, true);

            if (afterCutsceneDialogue != null)
            {
                StoryEventBus.Instance.RequestDialogue(afterCutsceneDialogue);
            }
        }
    }
}
