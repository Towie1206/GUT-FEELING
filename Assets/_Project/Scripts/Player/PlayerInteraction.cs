using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactDistance = 3f;
    [SerializeField] private LayerMask interactableLayer;

    [Header("Crosshair UI")]
    [SerializeField] private GameObject activeCrosshair;

    private IInteractable currentInteractable;
    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Start()
    {
        activeCrosshair.SetActive(false);
    }
    private void Update()
    {
        CheckInteraction();
    }

    private void CheckInteraction()
    {
        // tạo tia chiếu từ vị trí của camera người chơi theo hướng nhìn của camera
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        // 2. Bắn Raycast kiểm tra mỗi frame
        if (Physics.Raycast(ray, out RaycastHit hitInfo, interactDistance, interactableLayer))
        {
            IInteractable interactable = hitInfo.collider.GetComponent<IInteractable>();

            if(interactable != null)
            {
                currentInteractable = interactable;
                activeCrosshair.SetActive(true);
            }
            else
            {
                ClearInteraction();
            }
        }
        else
        {
            ClearInteraction();
        }
    }

    private void ClearInteraction()
    {
        if (currentInteractable != null)
        {
            currentInteractable = null;
        }
        activeCrosshair.SetActive(false);
    }

    public void TryToInteract()
    {
        currentInteractable?.Interact(player);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * interactDistance);
    }
}
