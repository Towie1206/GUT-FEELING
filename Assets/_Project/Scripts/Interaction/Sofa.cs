using UnityEngine;

public class Sofa : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform sitPoint;
    [SerializeField] private Transform standPoint;
    
    public string InteractionPrompt => "[E] Sit down";

    public void Interact(Player player)
    {
        player.movement.SitToSofa(sitPoint, standPoint);
        player.stateMachine.ChangeState(player.sofaState);
    }

}
