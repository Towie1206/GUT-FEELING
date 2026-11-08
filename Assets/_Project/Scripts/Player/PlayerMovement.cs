using System;
using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private float sprintSpeed = 2.5f;
    [SerializeField] private float crouchSpeed = 1f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckRadius = 0.25f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody rb;

    private Transform currentStandPoint;
    public bool IsSitting => rb.isKinematic;

    public float currentSpeed { get; private set; }

    // Events (Dành cho Audio bước chân & AI Stalker lắng nghe)
    public event Action<float> OnFootstepTaken; // truyền loudness (độ ồn)

    public Vector2 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        currentSpeed = moveSpeed;
    }

    public void SetMoveInput(Vector2 input)
    {
        moveInput = Vector2.ClampMagnitude(input, 1f);
    }

    public void UpdateMovementSpeed(bool isSprinting, bool isCrouching)
    {
        if (isCrouching)
            currentSpeed = crouchSpeed;
        else if (isSprinting)
            currentSpeed = sprintSpeed;
        else
            currentSpeed = moveSpeed;
    }


    private void FixedUpdate()
    {
        if (rb.isKinematic) return;
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector3 moveDirection = transform.right * moveInput.x + transform.forward * moveInput.y;

        Vector3 currentVelocity = rb.linearVelocity;

        rb.linearVelocity = new Vector3(moveDirection.x * currentSpeed, currentVelocity.y, moveDirection.z * currentSpeed);
    }

    public void Stop()
    {
        moveInput = Vector2.zero;
    }

    public void SitToSofa(Transform sitPoint, Transform standPoint)
    {
        transform.position = sitPoint.position;
        transform.rotation = sitPoint.rotation;
        currentStandPoint = standPoint;
        rb.isKinematic = true;
        Stop();
    }

    public void StandUpFromSofa()
    {
        transform.position = currentStandPoint.position;
        rb.isKinematic = false;
    }
}
