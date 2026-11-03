using UnityEngine;

public class TVRemoteInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private TVController targetTV;
    [SerializeField] private AudioSource clickRemote;
    public string InteractionPrompt => targetTV.CurrentChannel == TVController.TVChannelState.Off ? "[E] Turn on Tv" : "[E] Turn off Tv";

    public void Interact(Player player)
    {
        if (clickRemote != null)
            clickRemote.Play();
        targetTV.TogglePower();
    }

  
}
