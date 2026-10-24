using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PhoneSMSUI : MonoBehaviour
{
    [SerializeField] private StoryEventBus storyEventBus;
    [SerializeField] private NotificationBannerUI notificationBanner;

    [Header("Cinematic Transition")]
    [SerializeField] private RectTransform phoneRectTransform;
    [SerializeField] private Volume blurVolume;
    [SerializeField] private float slideDuration = 0.35f;

    [Header("Phone References")]
    [SerializeField] private ScrollRect chatScrollRect;
    [SerializeField] private Transform contentTransform;

    [Header("Message Bubble Prefabs")]
    [SerializeField] private GameObject rowNemPrefab;
    [SerializeField] private GameObject rowNamPrefab;

    [Header("Settings & Audio")]
    [SerializeField] private AudioSource sendSound;
    [SerializeField] private AudioSource notificationSound;

    [Header("Phone Menu (Màn hình danh bạ)")]
    [SerializeField] private GameObject phoneMenuPanel;
    [SerializeField] private TextMeshProUGUI chatHeaderNameText;

    [Header("Data")]
    [SerializeField] private SMSData data;
    [SerializeField] private Player player;

    private int unreadCount = 0;
    private bool isPhoneOpen = false;

    private Coroutine slideCoroutine;
    private Coroutine conversationCoroutine;

    private Vector2 onScreenPos = new Vector2(-48f, 0f);
    private Vector2 offScreenPos = new Vector2(-48f, -750f);

    private readonly Dictionary<SMSSender, List<GameObject>> chatHistory = new Dictionary<SMSSender, List<GameObject>>();
    private SMSSender? currentOpenContact = null;

    private void Awake()
    {
        phoneRectTransform.anchoredPosition = offScreenPos;
    }

    private void OnEnable()
    {
        player.OnPhoneToggled += HandlePhoneToggled;
        storyEventBus.OnSMSRequested += StartConversation;
    }

    private void OnDisable()
    {
        player.OnPhoneToggled -= HandlePhoneToggled;
        storyEventBus.OnSMSRequested -= StartConversation;
    }

    private void HandlePhoneToggled(bool show)
    {
        isPhoneOpen = show;
        PlaySlideAnimation(show);

        if (show)
        {
            unreadCount = 0;
            notificationBanner.Hide();
        }
    }

    public void PlaySlideAnimation(bool show)
    {
        if (slideCoroutine != null) StopCoroutine(slideCoroutine);
        slideCoroutine = StartCoroutine(SlideRoutine(show));
    }

    private IEnumerator SlideRoutine(bool show)
    {
        Vector2 startPos = phoneRectTransform.anchoredPosition;
        Vector2 targetPos = show ? onScreenPos : offScreenPos;
        float startWeight = blurVolume != null ? blurVolume.weight : 0f;
        float targetWeight = show ? 1f : 0f;
        float elapsed = 0;

        while (elapsed < slideDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / slideDuration);

            phoneRectTransform.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
            if (blurVolume != null) blurVolume.weight = Mathf.Lerp(startWeight, targetWeight, t);

            yield return null;
        }

        phoneRectTransform.anchoredPosition = targetPos;
        if (blurVolume != null) blurVolume.weight = targetWeight;
    }

    [ContextMenu("Test SMS")]
    private void TestSMS() => StartConversation(data);

    public void StartConversation(SMSData newData)
    {
        if (newData == null) return;

        // Cho chạy ngầm cuộc hội thoại luôn (KHÔNG tự mở điện thoại lớn lên)
        if (conversationCoroutine != null) StopCoroutine(conversationCoroutine);
        conversationCoroutine = StartCoroutine(ConversationRoutine(newData));
    }

    private IEnumerator ConversationRoutine(SMSData data)
    {
        foreach (var message in data.Messages)
        {
            if (message.sender == SMSSender.Nam)
            {
                // NHÁNH 1: LƯỢT CỦA PLAYER (NAM) 
                // Đợi máy mở VÀ đang mở đúng hộp thư của người này mới tự gõ trả lời!
                yield return new WaitUntil(() => isPhoneOpen && currentOpenContact == owner);
                yield return new WaitForSeconds(message.delay);
                // Phòng hờ đang gõ mà người chơi lại bấm Tab cất máy, thì đợi mở lại mới gửi
                yield return new WaitUntil(() => isPhoneOpen && currentOpenContact == owner);

                SpawnBubble(rowNamPrefab, message.GetText,data.ContactOwner);
                if (sendSound != null)
                    sendSound.Play();
                ScrollToBottom();
            }
            else
            {
                // NHÁNH 2: LƯỢT CỦA NPC (NEM, 113, CHỦ NHÀ) 
                yield return new WaitForSeconds(message.delay);

                SpawnBubble(rowNemPrefab, message.GetText, data.ContactOwner);
                if (notificationSound != null)
                    notificationSound.Play();
                ScrollToBottom();

                // Nếu lúc NPC gửi tới mà người chơi ĐANG CẤT MÁY (!isPhoneOpen):
                if (!isPhoneOpen || currentOpenContact != owner)
                {
                    unreadCount++;
                    notificationBanner.Show(message.sender.ToString(), unreadCount);
                }
            }
        }
    }

    private void SpawnBubble(GameObject prefab, string message, SMSSender contact)
    {
        GameObject newBubble = Instantiate(prefab, contentTransform);
        TextMeshProUGUI textComponent = newBubble.GetComponentInChildren<TextMeshProUGUI>();
        textComponent.text = message;
        if (textComponent.preferredWidth > 220f)
        {
            LayoutElement layout = newBubble.GetComponentInChildren<LayoutElement>();
            layout.preferredWidth = 220f;
        }

        if (!chatHistory.ContainsKey(contact))
        {
            chatHistory[contact] = new List<GameObject>();
        }
        chatHistory[contact].Add(newBubble); //add value
        newBubble.SetActive(currentOpenContact == contact); //Nếu người chơi đang mở đúng hộp thư của người này thì mới hiện, đang xem người khác thì ẩn đi

    }

    private void ScrollToBottom()
    {
        Canvas.ForceUpdateCanvases();
        chatScrollRect.verticalNormalizedPosition = 0f;
    }

    public void OpenPhoneMenu()
    {
        currentOpenContact = null;
        phoneMenuPanel.SetActive(true);
    }
    public void OpenNemChat() => OpenChatWith(SMSSender.Nem);
    public void OpenKiEnChat() => OpenChatWith(SMSSender.KiEn);
    private void OpenChatWith(SMSSender contact)
    {
        currentOpenContact = contact;
        phoneMenuPanel.SetActive(false);

        if (chatHeaderNameText.text != contact.ToString())
            chatHeaderNameText.text = contact.ToString();

        foreach (var pair in chatHistory)
        {
            bool isSelectedContact = pair.Key == contact;
            foreach (GameObject bubble in pair.Value)
            {
                bubble.SetActive(isSelectedContact);
            }
        }
        ScrollToBottom();
    }
}