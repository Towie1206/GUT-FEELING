using Unity.Cinemachine;
using UnityEngine;

public class BrokenKettleInteractable : MonoBehaviour, IInteractable
{
    [Header("Audio & Effects")]
    [SerializeField] private AudioSource sparkAudio;
    [SerializeField] private ParticleSystem sparkVFX;

    [Header("Dialogue")]
    [SerializeField] private DialogueData brokenDialogue;

    [SerializeField] private DialogueData alreadyBrokenDialogue;

    [Header("Camera")]
    [SerializeField] private CinemachineImpulseSource source;

    private bool hasInteracted;

    public string InteractionPrompt => hasInteracted ? "It's already broken" : "[E] to Use";

    public void Interact(Player player)
    {
        if (hasInteracted)
        {
            StoryEventBus.Instance.RequestDialogue(alreadyBrokenDialogue);
            return;
        }
        if (sparkVFX != null)
            sparkVFX.Play();
        if (sparkAudio != null)
            sparkAudio.Play();
        hasInteracted = true;

        StoryEventBus.Instance.SetFlag(StoryFlag.KettleFoundBroken, true);
        StoryEventBus.Instance.RequestDialogue(brokenDialogue);

        source.GenerateImpulse();
    }
}
