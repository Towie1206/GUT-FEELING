using System;
using System.Collections;
using UnityEngine;

public class WardrobeDoorInteractable : MonoBehaviour, IInteractable
{
    [Header("Door Configuration")]
    [Tooltip("Transform của cánh cửa xoay quanh trục Y")]
    [SerializeField] private Transform doorTransform;

    [Tooltip("Góc mở mục tiêu quanh trục Y (trái: -90 đến -100, phải: +90 đến +100)")]
    [SerializeField] private float openAngleY = -90f;

    [Tooltip("Góc đóng quanh trục Y (mặc định: 0)")]
    [SerializeField] private float closedAngleY = 0f;

    [Tooltip("Tốc độ xoay mở/đóng cửa")]
    [SerializeField] private float swingSpeed = 3.5f;

    [Header("Audio (Tùy chọn)")]
    [SerializeField] private AudioSource doorAudioSource;
    [SerializeField] private AudioClip openSqueakSound;
    [SerializeField] private AudioClip closeSqueakSound;

    // Trạng thái hiện tại
    private bool isOpen = false;
    private Coroutine swingCoroutine;

    public bool IsOpen => isOpen;

    public string InteractionPrompt => isOpen ? "E: Đóng tủ" : "E: Mở tủ";

    public event Action<bool> OnDoorStateChanged;

    private void Awake()
    {
        if (doorTransform == null)
            doorTransform = transform;
    }

    public void Interact(Player player)
    {
        // Toggle trạng thái mở/đóng
        isOpen = !isOpen;

        // Phát âm thanh tiếng cót két cửa gỗ cũ
        PlayDoorSound(isOpen);

        // Kích hoạt animation xoay cửa
        if (swingCoroutine != null)
            StopCoroutine(swingCoroutine);
        swingCoroutine = StartCoroutine(SwingDoorRoutine(isOpen ? openAngleY : closedAngleY));

        // Bắn event ra ngoài hệ thống
        OnDoorStateChanged?.Invoke(isOpen);

        // TODO (User thực hiện):
        // 1. Kiểm tra điều kiện StoryFlag nếu cửa tủ bị khóa hoặc hé mở bí ẩn bởi Squatter.
        // 2. Phát âm thanh tiếng thở hoặc tiếng cào sau hốc tường khi mở tủ lần đầu.
    }

    private IEnumerator SwingDoorRoutine(float targetAngleY)
    {
        Quaternion targetRot = Quaternion.Euler(0f, targetAngleY, 0f);
        float angularSpeed = Mathf.Max(30f, swingSpeed * 60f);

        while (Quaternion.Angle(doorTransform.localRotation, targetRot) > 0.05f)
        {
            doorTransform.localRotation = Quaternion.RotateTowards(
                doorTransform.localRotation,
                targetRot,
                angularSpeed * Time.deltaTime
            );
            yield return null;
        }

        doorTransform.localRotation = targetRot;
        swingCoroutine = null;
    }

    private void PlayDoorSound(bool opening)
    {
        if (doorAudioSource == null) return;

        AudioClip clipToPlay = opening ? openSqueakSound : closeSqueakSound;
        if (clipToPlay != null)
        {
            doorAudioSource.PlayOneShot(clipToPlay);
        }
    }
}
