using System.Net;
using UnityEngine;

public class FlashlightItem : MonoBehaviour, IInteractable, IHoldable
{
    [Header("Components")]
    [SerializeField] private GameObject spotLight;
    [SerializeField] private AudioSource clickSound;
    [SerializeField] private FlashlightSway swayComponent;

    [Header("Interaction Text")]
    [SerializeField] private string promptText = "Nhấn [E] để nhặt Đèn pin";

    private Rigidbody rb;
    private Collider col;
    private bool isHeld = false;

    public string InteractionPrompt => isHeld ? string.Empty : promptText;
    public bool IsHeld => isHeld;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        if (spotLight != null) spotLight.SetActive(false);
        if (swayComponent != null) swayComponent.enabled = false;
    }

    public void Interact(Player player)
    {
        if(isHeld) return;

        player.hand.Equip(this);

    }   
    
    public void OnEquip(Transform heldPoint)
    {
        isHeld = true;
        
        transform.SetParent(heldPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        rb.isKinematic = true;
        rb.useGravity = false;
        col.enabled = false;
        swayComponent.enabled = true;
    }

    public void OnUse()
    {
        if (!isHeld || spotLight == null) return;

        spotLight.SetActive(!spotLight.activeSelf);
        clickSound.Play();
    }    

    public void OnDrop(Vector3 throwDirection)
    {
        isHeld = false;

        transform.SetParent(null);
        rb.isKinematic = false;
        rb.useGravity = true;
        col.enabled = true;
        swayComponent.enabled = false;

        // Reset vận tốc cũ để không bị quán tính cũ giật
        rb.linearVelocity = Vector3.zero;

        // Đẩy nhẹ đèn ra phía trước mặt một chút (0.2m) để không cấn vào Collider của Player
        transform.position += throwDirection * 0.2f;

        // Tác dụng lực ném theo đúng hướng mắt nhìn
        rb.AddForce(throwDirection * 10f, ForceMode.Impulse);

        // Tạo độ xoay lộn vòng ngẫu nhiên trên không trung cho chân thực
        rb.AddTorque(Random.insideUnitSphere * 2f, ForceMode.Impulse);
    }

    public void OnSway(float lookDelta)
    {
        swayComponent?.UpdateSway(lookDelta);
    }
}
