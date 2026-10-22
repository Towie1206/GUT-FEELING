using System.Collections;
using UnityEngine;

public class StoryTrigger : MonoBehaviour
{
    [Header("Trigger Settings")]
    [Tooltip("Nếu tích chọn, trigger sẽ tự nổ ngay khi load Scene mà không cần người chơi bước vào")]
    [SerializeField] private bool triggerOnStart = false;

    [Tooltip("Nếu tích chọn, sự kiện chỉ nổ 1 lần duy nhất trong toàn bộ màn chơi")]
    [SerializeField] private bool triggerOnce = true;

    [Header("Story Flag Condition (Optional)")]
    [Tooltip("Có cần điều kiện cốt truyện trước đó không? (Ví dụ: phải nhặt được chìa khóa mới kích hoạt)")]
    [SerializeField] private bool requireFlag = false;
    [SerializeField] private StoryFlag requiredFlag;

    [Header("Story Flag To Set")]
    [Tooltip("Có bật cờ cốt truyện mới sau khi kích hoạt không?")]
    [SerializeField] private bool setFlagOnTrigger = true;
    [SerializeField] private StoryFlag flagToSet;

    [Header("Content Sequence (Nhịp kịch bản)")]
    [SerializeField] private DialogueData dialogueToPlay;

    [Tooltip("Thời gian chờ (giây) trước khi tin nhắn tới, để người chơi kịp đọc xong độc thoại")]
    [SerializeField] private float smsDelay = 3.5f;
    [SerializeField] private SMSData smsToPlay;

    // Biến cờ nội bộ để chặn việc kích hoạt trùng lặp
    private bool hasTriggered = false;
    private Collider triggerCollider;

    private void Awake()
    {
        triggerCollider = GetComponent<Collider>();
    }

    private IEnumerator Start()
    {
        // Đợi hết frame đầu tiên
        yield return null;

        if (triggerOnStart)
        {
            ExecuteTrigger();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggerOnStart) return;

        // Tìm kiếm Player ở component cha (vì Collider nằm ở GameObject con Capsule)
        Player player = other.GetComponentInParent<Player>();
        if (player != null)
        {
            Debug.Log($"[StoryTrigger] Đã phát hiện Player bước vào vùng kích hoạt: {gameObject.name}");
            ExecuteTrigger();
        }
    }

    public void ExecuteTrigger()
    {
        // Nếu đã từng chạy rồi và chỉ cho chạy 1 lần -> Hủy lệnh
        if (triggerOnce && hasTriggered) return;

        // Nếu có điều kiện cờ cốt truyện mà cờ đó chưa bật -> Hủy lệnh
        if (requireFlag && !StoryEventBus.Instance.HasFlag(requiredFlag)) return;

        // Đánh dấu đã kích hoạt thành công
        hasTriggered = true;

        if (triggerOnce && triggerCollider != null)
        {
            triggerCollider.enabled = false;
        }

        // Bật cờ sự kiện lên EventBus nếu có cấu hình
        if (setFlagOnTrigger)
        {
            StoryEventBus.Instance.SetFlag(flagToSet, true);
        }

        // Bắt đầu chuỗi nhịp kịch bản tuần tự (Sequencing) qua Coroutine
        StartCoroutine(PlayStorySequenceRoutine());
    }

    private IEnumerator PlayStorySequenceRoutine()
    {
        // 1. Bắn Độc thoại / Thoại nếu có dữ liệu
        if (dialogueToPlay != null)
        {
            StoryEventBus.Instance.RequestDialogue(dialogueToPlay);
        }

        // 2. Xử lý tin nhắn SMS
        if (smsToPlay != null)
        {
            // Nếu trước đó đã có thoại, bắt buộc phải chờ một khoảng thời gian (smsDelay)
            // để người chơi đọc xong câu chữ rồi điện thoại mới reo lên
            if (dialogueToPlay != null)
            {
                yield return new WaitForSeconds(smsDelay);
            }

            // Gửi yêu cầu tin nhắn đến điện thoại
            StoryEventBus.Instance.RequestSMS(smsToPlay);
        }
    }
}