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

        rb.AddForce(throwDirection * 7.5f, ForceMode.Impulse);
    }

    public void OnSway(float lookDelta)
    {
        swayComponent?.UpdateSway(lookDelta);
    }
}
