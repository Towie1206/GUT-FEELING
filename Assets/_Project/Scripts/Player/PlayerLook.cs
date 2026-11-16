using Unity.Cinemachine;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private Transform camPivot;

    [Header("Look Details")]
    [SerializeField] private float mouseSensitivity = 1f;
    [SerializeField] private float maxLookAngle = 80f;
    [SerializeField] private float minLookAngle = -80f;

    [Header("Zoom Settings")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private CinemachineCamera playerVirtualCamera;
    [SerializeField] private float defaultFOV = 75f;
    [SerializeField] private float zoomFOV = 45f;
    [SerializeField] private float zoomSpeed = 10f; // Tốc độ co dãn FOV
    [SerializeField] private float zoomSensitivityMultiplier = 0.5f; // Khi zoom thì giảm độ nhạy chuột để lia cho đầm tay
    private bool isZooming;
    private float pitch;

    private void Awake()
    {
        if (playerVirtualCamera == null && camPivot != null)
        {
            playerVirtualCamera = camPivot.GetComponentInChildren<CinemachineCamera>();
        }
    }
    public void Look(Vector2 lookInput)
    {
        HandleHorizontalLook(lookInput.x);
        HandleVerticalLook(lookInput.y);
    }

    public void HandleHorizontalLook(float horizontalInput)
    {
        transform.Rotate(Vector3.up * horizontalInput * (isZooming ? zoomSensitivityMultiplier * mouseSensitivity : mouseSensitivity));
    }

    public void HandleVerticalLook(float verticalInput)
    {
        pitch -= verticalInput * (isZooming ? zoomSensitivityMultiplier * mouseSensitivity : mouseSensitivity);
        pitch = Mathf.Clamp(pitch, minLookAngle, maxLookAngle);

        camPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    public void SetZoom(bool zooming)
    {
        isZooming = zooming;
    }

    private void Update()
    {
        HandleZoom();
    }

    private void HandleZoom()
    {
        if (playerCamera == null) return;

        float targetFOV = isZooming ? zoomFOV : defaultFOV;
        if (playerVirtualCamera != null)
            playerVirtualCamera.Lens.FieldOfView = Mathf.Lerp(playerVirtualCamera.Lens.FieldOfView, targetFOV, Time.deltaTime * zoomSpeed);
        else
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * zoomSpeed);
    }

    public Transform CameraTransform => camPivot;

    public void ResetCameraTransform()
    {
        if (playerCamera != null)
        {
            playerCamera.transform.localPosition = Vector3.zero;
            playerCamera.transform.localRotation = Quaternion.identity;
        }
    }
}
