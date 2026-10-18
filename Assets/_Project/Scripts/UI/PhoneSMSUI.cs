using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PhoneSMSUI : MonoBehaviour
{
    [Header("Cinematic Transition")]
    [SerializeField] private RectTransform phoneRectTransform; // Kéo PhoneFrame vào đây
    [SerializeField] private Volume blurVolume; // Kéo Volume làm mờ vào đây (nếu dùng Depth of Field)
    [SerializeField] private float slideDuration = 0.35f;

    [Header("Phone References")]
    [SerializeField] private GameObject phoneFrame;
    [SerializeField] private ScrollRect chatScrollRect;
    [SerializeField] private Transform contentTransform;

    [Header("Message Bubble Prefabs")]
    [SerializeField] private GameObject rowNemPrefab;
    [SerializeField] private GameObject rowNamPrefab;

    [Header("Settings & Audio")]
    [SerializeField] private float messageDelay = 2.5f;
    [SerializeField] private AudioSource sendSound;
    [SerializeField] private AudioSource notificationSound;

    [Header("Data")]
    [SerializeField] private SMSData data;
    [SerializeField] private Player player;

    private void OnEnable()
    {
        player.OnPhoneToggled += PlaySlideAnimation;
    }
    private void OnDisable()
    {
        player.OnPhoneToggled -= PlaySlideAnimation;
    }


    private Coroutine conversationCoroutine;
    private Vector2 onScreenPos = new Vector2(-48f, 0f);
    private Vector2 offScreenPos = new Vector2(-48f, -750f);
    private Coroutine slideCoroutine;


    private void Awake()
    {
        phoneFrame.SetActive(false);
    }

    [ContextMenu("Test SMS")]
    private void TestSMS() => StartConversation(data);

    public void PlaySlideAnimation(bool show)
    {
        if (slideCoroutine != null) StopCoroutine(slideCoroutine);
        slideCoroutine = StartCoroutine(SlideRoutine(show));
    }

    public void TogglePhone(bool show)
    {
        phoneFrame.SetActive(show);
    }

    public void StartConversation(SMSData data)
    {
        if (data == null) return;
        TogglePhone(true);
        if (conversationCoroutine != null) StopCoroutine(conversationCoroutine);
        conversationCoroutine = StartCoroutine(ConversationRoutine(data));
    }
    private IEnumerator ConversationRoutine(SMSData data)
    {
        foreach (var message in data.Messages)
        {
            yield return new WaitForSeconds(message.delay);

            if (message.sender == SMSSender.Nam)
            {
                SpawnBubble(rowNamPrefab, message.GetText);
                if (sendSound != null) sendSound.Play();
            }
            else
            {
                SpawnBubble(rowNemPrefab, message.GetText);
                if (notificationSound != null) notificationSound.Play();
            }
            ScrollToBottom();
        }

        yield return null;
    }

    private IEnumerator SlideRoutine(bool show)
    {
        if (show && phoneFrame != null) phoneFrame.SetActive(true);
        Vector2 startPos = phoneRectTransform.anchoredPosition;
        Vector2 targetPos = show ? onScreenPos : offScreenPos;
        float startWeight = blurVolume != null ? blurVolume.weight : 0f;
        float targetWeight = show ? 1f : 0f;
        float elapsed = 0;
        while (elapsed < slideDuration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.SmoothStep(0f, 1f, elapsed / slideDuration); //nếu elapsed / slideDuration < 0 trả về 0, > 1 trả về 1 

            phoneRectTransform.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);

            blurVolume.weight = Mathf.Lerp(startWeight, targetWeight, t);

            yield return null;
        }

        phoneRectTransform.anchoredPosition = targetPos;
        if (blurVolume != null) blurVolume.weight = targetWeight;
        if (!show && phoneFrame != null) phoneFrame.SetActive(false);
    }

    private void SpawnBubble(GameObject prefab, string message)
    {
        GameObject newBubble = Instantiate(prefab, contentTransform);
        TextMeshProUGUI textComponent = newBubble.GetComponentInChildren<TextMeshProUGUI>();
        textComponent.text = message;
        if (textComponent.preferredWidth > 220f)
        {
            LayoutElement layout = newBubble.GetComponentInChildren<LayoutElement>();
            layout.preferredWidth = 220f;
        }

    }

    private void ScrollToBottom()
    {
        Canvas.ForceUpdateCanvases();
        chatScrollRect.verticalNormalizedPosition = 0f;
    }
}
