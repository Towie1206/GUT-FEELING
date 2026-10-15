using System.Net.NetworkInformation;
using UnityEngine;

public class ItemRetraction : MonoBehaviour
{
    [Header("Raycast Settings")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private float checkDistance = 0.7f;

    [Header("Retraction Motion")]
    [SerializeField] private float retractAmountZ = .4f;
    [SerializeField] private float lowerAmountY = .15f;
    [SerializeField] private float smoothSpeed = 12f;

    private Vector3 defaultLocalPos;

    private void Awake()
    {
        defaultLocalPos = transform.localPosition;
    }

    private void Update()
    {
        if (transform.childCount == 0)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, defaultLocalPos, Time.deltaTime * smoothSpeed);
            return;
        }
    }

}
