using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactDistance = 3f;
    [SerializeField] private LayerMask interactableLayer;

    private Player player;
    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private IInteractable currentInteractable;

    public void TryToInteract()
    {
        // tạo tia chiếu từ vị trí của camera người chơi theo hướng nhìn của camera
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, interactDistance, interactableLayer))
        {
            IInteractable interactable = hitInfo.collider.GetComponent<IInteractable>();

            interactable?.Interact(player);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * interactDistance);
    }
}
