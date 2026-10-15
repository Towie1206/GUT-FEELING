using System;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactDistance = 3f;
    [SerializeField] private LayerMask interactableLayer;

    [Header("Crosshair UI")]
    [SerializeField] private GameObject activeCrosshair;

    public event Action<string> OnHoldStarted;
    public event Action<float> OnHoldProgressChanged;
    public event Action OnHoldEnded;

    public event Action<string> OnInteractableFound;
    public event Action OnInteractableLost;

    private IInteractable currentInteractable;
    private IHoldInteractable currentHoldInteractable;
    private Collider lastHitCollider;


    private bool isHolding;
    private float currentHoldTimer;
    private bool isWorldHold;

    private void Awake()
    {

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
            if (hitInfo.collider == lastHitCollider) return;
            lastHitCollider = hitInfo.collider;

            IInteractable interactable = hitInfo.collider.GetComponent<IInteractable>();
            IHoldInteractable holdInteractable = hitInfo.collider.GetComponent<IHoldInteractable>();

            if (isHolding && isWorldHold && currentHoldInteractable != holdInteractable)
                CancelHold();

            if (interactable != null || holdInteractable != null)
            {
                currentInteractable = interactable;

                if (!isHolding || isWorldHold)
                    currentHoldInteractable = holdInteractable;

                string prompt = currentInteractable.InteractionPrompt;
                OnInteractableFound?.Invoke(prompt);

                activeCrosshair.SetActive(true);
            }
            else
            {
                OnInteractableLost.Invoke();
                ClearInteraction();
            }
        }
        else
        {
            OnInteractableLost.Invoke();
            ClearInteraction();
        }
    }

    private void ClearInteraction()
    {
        if (isHolding && isWorldHold) // CHỈ TỰ HỦY NẾU ĐANG LÀ WORLD HOLD (NHÌN ĐỒ NGOÀI PHÒNG):
            CancelHold();

        currentInteractable = null;

        if (!isHolding || isWorldHold)     // Đang giữ đồ trên tay thì KHÔNG ĐƯỢC XÓA currentHoldInteractable:
            currentHoldInteractable = null;

        lastHitCollider = null;
        activeCrosshair.SetActive(false);
    }

    public void TryToInteract(Player player)
    {
        currentInteractable?.Interact(player);
    }

    // 1. Khi bấm [E] (Tự lấy đồ trước mặt tia Raycast):
    public void StartHoldWorld()
    {
        if (currentHoldInteractable == null || !currentHoldInteractable.CanHold || isHolding) return;

        isWorldHold = true;
        isHolding = true;
        currentHoldTimer = 0f;
        currentHoldInteractable.OnHoldStart();
        OnHoldStarted?.Invoke(currentHoldInteractable.ActionPrompt);
    }

    // 2. Khi bấm [R] (Tự lấy đồ đang cầm trên tay của Player):
    public void StartHoldHand(Player player)
    {
        // Tự thò tay vào PlayerHand lấy đồ, em không cần truyền cái gì từ ngoài vào!
        if (player.hand.CurrentItemHold is IHoldInteractable handItem)
        {
            if (!handItem.CanHold || isHolding) return;

            isWorldHold = false;
            currentHoldInteractable = handItem;
            isHolding = true;
            currentHoldTimer = 0f;
            handItem.OnHoldStart();
            OnHoldStarted?.Invoke(handItem.ActionPrompt);
        }
    }

    public void UpdateHold(Player player)
    {
        if (isHolding)
        {
            currentHoldTimer += Time.deltaTime;
            float t = currentHoldTimer / currentHoldInteractable.HoldDuration;
            currentHoldInteractable.OnHoldProgress(t); // phát cho item biết tiến trình giữ đã thay đổi
            OnHoldProgressChanged?.Invoke(t); // phát cho ui rằng tiến trình giữ đã thay đổi để cập nhật UI

            if (t >= 1f)
            {
                isHolding = false;
                currentHoldTimer = 0f;
                OnHoldEnded?.Invoke();
                currentHoldInteractable.OnHoldComplete(player);
            }
        }
    }

    public void CancelHold()
    {
        if (!isHolding) return;

        currentHoldInteractable?.OnHoldCancel();
        isHolding = false;
        currentHoldTimer = 0f;
        OnHoldEnded?.Invoke();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * interactDistance);
    }
}
