using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private Transform camPivot;

    [Header("Look Details")]
    [SerializeField] private float mouseSensitivity = 1f;
    [SerializeField] private float maxLookAngle = 80f;
    [SerializeField] private float minLookAngle = -80f;

    private float pitch;

    public void Look(Vector2 lookInput)
    {
        HandleHorizontalLook(lookInput.x);
        HandleVerticalLook(lookInput.y);
    }

    public void HandleHorizontalLook(float horizontalInput)
    {
        transform.Rotate(Vector3.up * horizontalInput * mouseSensitivity);
    }

    public void HandleVerticalLook(float verticalInput)
    {
        pitch -= verticalInput * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, minLookAngle, maxLookAngle);

        camPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }
}
