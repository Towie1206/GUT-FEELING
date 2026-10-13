using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FireExtinguisherItem : MonoBehaviour, IInteractable, IHoldable, IHoldInteractable
{
    [SerializeField] private Transform triggerLever;
    [SerializeField] private GameObject safetyPin;
    [SerializeField] private ParticleSystem co2SprayParticle;
    [SerializeField] private AudioSource sprayAudioSource;
    [SerializeField] private AudioSource stuckAudioSource;
    [SerializeField] private AudioSource pullPinAudioSource;

    [SerializeField] private float leverPressAngle = 12f;
    [SerializeField] private float throwForce = 7f;
    [SerializeField] private string pickupPrompt = "Nhấn [E] để nhặt Bình cứu hỏa";

    [SerializeField] private GameObject hoseStowed;
    [SerializeField] private GameObject hoseAimed;
    [SerializeField] private Vector3 pinPullOffset = new Vector3(0.05f, 0f, 0f);

    // Trạng thái nội bộ
    private Rigidbody rb;
    private Collider col;
    private bool isHeld = false;
    private bool isSpraying = false;
    private bool isPinPulled = false;
    private Quaternion originalLeverRotation;
    private Quaternion pressedLeverRotation;

    private Vector3 initialPinLocalPos;


    [SerializeField] private Vector3 holdRotationOffset = new Vector3(0f, 90f, 0f);

    public bool IsHeld => isHeld;
    public bool IsPinPulled => isPinPulled;
    public string InteractionPrompt => isHeld ? string.Empty : pickupPrompt;

    public bool CanHold => isHeld && !isPinPulled;

    public float HoldDuration => 1.5f; // Giữ 1.5 giây để rút chốt
    public string ActionPrompt => "Đang rút chốt an toàn...";

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        originalLeverRotation = triggerLever.localRotation;

        pressedLeverRotation = originalLeverRotation * Quaternion.Euler(leverPressAngle, 0f, 0f);


        this.enabled = false;

        if (hoseStowed != null) hoseStowed.SetActive(true);
        if (hoseAimed != null) hoseAimed.SetActive(false);

        initialPinLocalPos = safetyPin.transform.localPosition;
    }

    private void Update()
    {
        // Nếu đang xịt thì Lerp về góc ép, ngược lại thì Lerp đàn hồi về góc ban đầu
        Quaternion targetRot = isSpraying ? pressedLeverRotation : originalLeverRotation;

        triggerLever.localRotation = Quaternion.Lerp(triggerLever.localRotation, targetRot, Time.deltaTime * 15f);
    }

    public void Interact(Player player)
    {
        if (isHeld) return;
        player.hand.Equip(this);
    }

    public void OnEquip(Transform heldPoint)
    {
        isHeld = true;
        this.enabled = true;

        transform.SetParent(heldPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(holdRotationOffset);

        rb.isKinematic = true;
        rb.useGravity = false;
        col.enabled = false;

        if (hoseStowed != null) hoseStowed.SetActive(false);
        if (hoseAimed != null) hoseAimed.SetActive(true);

    }

    public void OnUse()
    {
        if (!isHeld) return;
        if (!isPinPulled)
        {
            if (stuckAudioSource != null) stuckAudioSource.Play();
            return;
        }
        else
        {
            Toggle();
            if (isSpraying)
            {
                co2SprayParticle.Play();
                if (sprayAudioSource != null) sprayAudioSource.Play();
            }
            else
            {
                co2SprayParticle.Stop();
                if (sprayAudioSource != null) sprayAudioSource.Stop();
            }
        }
    }

    public void OnDrop(Vector3 throwDirection)
    {
        isHeld = false;
        this.enabled = false;

        transform.SetParent(null);

        isSpraying = false;
        if (co2SprayParticle != null) co2SprayParticle.Stop();
        if (sprayAudioSource != null) sprayAudioSource.Stop();

        rb.isKinematic = false;
        rb.useGravity = true;
        col.enabled = true;

        rb.linearVelocity = Vector3.zero;
        transform.position += throwDirection * 0.5f;
        rb.AddForce(throwDirection * throwForce, ForceMode.Impulse);

        if (hoseStowed != null) hoseStowed.SetActive(true);
        if (hoseAimed != null) hoseAimed.SetActive(false);
    }

    public void OnSway(float lookDelta)
    {
        // TODO (Tùy chọn): Đung đưa bình cứu hỏa (nếu có gắn component Sway)
    }

    public void PullSafetyPin()
    {
        safetyPin.SetActive(false);
        if (pullPinAudioSource != null) pullPinAudioSource.Play();
        isPinPulled = true;
    }

    private void Toggle()
    {
        isSpraying = !isSpraying;
    }

    public void OnHoldStart()
    {
        if (isPinPulled) return;
        if(pullPinAudioSource != null) pullPinAudioSource.Play();
    }

    public void OnHoldProgress(float normalizedProgress)
    {
        safetyPin.transform.localPosition = Vector3.Lerp(initialPinLocalPos, initialPinLocalPos + pinPullOffset, normalizedProgress);
    }
    public void OnHoldComplete(Player player)
    {
        PullSafetyPin();
    }
    public void OnHoldCancel()
    {
        safetyPin.transform.localPosition = initialPinLocalPos;
    }
}
