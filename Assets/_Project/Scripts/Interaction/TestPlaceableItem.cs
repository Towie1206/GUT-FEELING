using UnityEngine;

public class TestPlaceableItem : MonoBehaviour, IInteractable, IHoldable, IPlaceable
{
    [SerializeField] private string itemName = "Khối Hộp Test";
    [SerializeField] private string placeableID = "TestCube";

    private Rigidbody rb;
    private Collider col;
    private bool isHeld = false;

    private Vector3 originalScale;

    public string PlaceableID => placeableID;
    public string InteractionPrompt => isHeld ? string.Empty : $"[E] Nhặt {itemName}";

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        originalScale = transform.localScale;
    }

    // 1. Nhặt lên tay khi người chơi bấm [E]
    public void Interact(Player player)
    {
        if (isHeld) return;
        player.hand.Equip(this);
    }

    public void OnEquip(Transform handSlot)
    {
        isHeld = true;
        transform.SetParent(handSlot);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        transform.localScale = originalScale;

        rb.isKinematic = true;
        col.enabled = false; // Tắt va chạm khi đang trên tay
    }

    // 2. Tự an tọa khi được đặt vào Socket
    public void OnPlace(Transform socketPoint)
    {
        isHeld = false;
        transform.SetParent(socketPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        transform.localScale = originalScale;

        rb.isKinematic = true;
        col.enabled = true; // BẬT LẠI COLLIDER để sau này người chơi có thể nhặt lại!
    }

    public void OnDrop(Vector3 throwDirection)
    {
        isHeld = false;
        transform.SetParent(null);
        rb.isKinematic = false;
        col.enabled = true;
        rb.AddForce(throwDirection * 5f, ForceMode.Impulse);
    }

    public void OnUse() { }
    public void OnSway(float lookDelta) { }
}