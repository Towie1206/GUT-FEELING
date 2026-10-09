using UnityEngine;

public class FlashlightSway : MonoBehaviour
{
    [Header("Sway Settings")]
    [SerializeField] private float sens = 2f;          // Để 2f giống hệt BatPan
    [SerializeField] private float maxLeftRot = 10f;   // Tối đa 10 độ sang trái
    [SerializeField] private float maxRightRot = 50f;  // Tối đa 50 độ sang phải

    private float currentYaw = 0f;

    // Khi đèn được nhặt lên (swayComponent.enabled = true) -> reset góc về 0
    private void OnEnable()
    {
        currentYaw = 0f;
        transform.localRotation = Quaternion.identity;
    }

    public void UpdateSway(float mouseX)
    {
        // 1. CỘNG DỒN theo đúng công thức của BatPan!
        currentYaw += mouseX * sens;

        // 2. Giới hạn trong khoảng [-10, 50] độ của tay phải
        currentYaw = Mathf.Clamp(currentYaw, -maxLeftRot, maxRightRot);

        // 3. Xoay đèn quanh trục Y (chuẩn 100% BatPan)
        transform.localRotation = Quaternion.Euler(0f, currentYaw, 0f);
    }
}