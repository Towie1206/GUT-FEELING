using UnityEngine;

public abstract class HoldableItemBase : MonoBehaviour, IInteractable, IHoldable, IPlaceable
{
    [Header("Base Item Settings")]
    [SerializeField] protected string itemName = "Vật phẩm";
    [SerializeField] protected string placeableID = "DefaultID";
    [SerializeField] protected float throwForce = 5f;
    [SerializeField] private Vector3 heldScale = new Vector3(0.15f, 0.15f, 0.15f);
    [SerializeField] protected Vector3 heldPositionOffset = Vector3.zero;
    [SerializeField] protected Vector3 heldRotationOffset = Vector3.zero;

    [Header("Sway Settings")]
    [SerializeField] protected float swayAmount = 1.5f; //độ nhạy nghiêng theo chuột
    [SerializeField] protected float maxSwayAngle = 10f; //góc nghiêng tối đa để cổ tay không bị vặn quá đà
    [SerializeField] protected float swaySmoothness = 8f; //tốc độ đàn hồi về vị trí cũ

    protected Rigidbody rb;
    protected Collider col;
    protected bool isHeld;
    protected Vector3 originalScale;

    public string PlaceableID => placeableID; // Mã định danh để ItemSocket kiểm tra xem vật phẩm có khớp với ổ cắm hay không.

    public bool IsHeld => isHeld; // Cho biết vật phẩm hiện đang được cầm trên tay Player hay không.

    // Câu nhắc tương tác hiển thị trên màn hình khi tia Raycast chiếu vào.
    public virtual string InteractionPrompt => isHeld ? string.Empty : $"[E] Pick Up {itemName}";

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        originalScale = transform.localScale;
    }

    public virtual void Interact(Player player)
    {
        if (isHeld == true)
            return;

        player.hand.Equip(this);
    }

    public virtual void OnEquip(Transform handSlot)
    {

        isHeld = true;
        transform.SetParent(handSlot);

        transform.localPosition = heldPositionOffset;
        transform.localRotation = Quaternion.Euler(heldRotationOffset);
        transform.localScale = heldScale;

        rb.isKinematic = true;
        rb.useGravity = false;
        col.enabled = false;

    }

    public virtual void OnPlace(Transform socketPoint)
    {

        isHeld = false;
        transform.SetParent(socketPoint);

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = originalScale;

        rb.isKinematic = true;
        col.enabled = true;
    }

    public virtual void OnDrop(Vector3 throwDirection)
    {
        transform.SetParent(null);
        isHeld = false;
        transform.localScale = originalScale;
        rb.isKinematic = false;
        rb.useGravity = true;
        col.enabled = true;
        rb.AddForce(throwDirection * throwForce, ForceMode.Impulse);
    }

    public virtual void OnUse()
    {
        // TODO (Tùy chọn ở lớp con): Ghi đè (override) nếu vật phẩm có hành động khi bấm Chuột Trái
    }

    public virtual void OnSway(float lookDelta)
    {
        float swayAngle = Mathf.Clamp(lookDelta * swayAmount, -maxSwayAngle, maxSwayAngle); //nếu ko di chuyển chuột thì góc nghiêng = 0, nếu di chuyển chuột thì góc nghiêng theo hướng di chuyển
        Quaternion targetRotation = Quaternion.Euler(heldRotationOffset) * Quaternion.Euler(0f, swayAngle, swayAngle);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * swaySmoothness);
    }
}
