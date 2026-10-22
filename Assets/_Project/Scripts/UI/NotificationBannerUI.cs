using System.Collections;
using TMPro;
using UnityEngine;

public class NotificationBannerUI : MonoBehaviour
{
    [SerializeField] private RectTransform bannerRectTransform;
    [SerializeField] private TextMeshProUGUI bannerSenderText;
    [SerializeField] private TextMeshProUGUI bannerPreviewText;
    [SerializeField] private Vector2 onScreenPos = Vector2.zero;
    [SerializeField] private Vector2 offScreenPos = new Vector2(0f, 100f);
    [SerializeField] private float slideDuration = 0.35f;
    [SerializeField] private float notificationDuration = 3f;

    private Coroutine bannerCoroutine;

    private void Awake()
    {
        bannerRectTransform.anchoredPosition = offScreenPos;
    }

    public void Show(string senderName, int unreadCount)
    {
        // TODO: Gán text cho bannerSenderText và bannerPreviewText, rồi chạy Coroutine trượt xuống (true)
        bannerSenderText.text = senderName;
        bannerPreviewText.text = $"{unreadCount} Text Messages";

        if (bannerCoroutine != null) StopCoroutine(bannerCoroutine);
        bannerCoroutine = StartCoroutine(SlideRoutine(true));
    }
    public void Hide()
    {
        if (bannerCoroutine != null) StopCoroutine(bannerCoroutine);
        bannerCoroutine = StartCoroutine(SlideRoutine(false));
    }

    private IEnumerator SlideRoutine(bool show)
    {
        Vector2 startPos = bannerRectTransform.anchoredPosition;
        Vector2 targetPos = show ? onScreenPos : offScreenPos;
        float elapsed = 0;

        while (elapsed < slideDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / slideDuration);

            bannerRectTransform.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);

            yield return null;
        }

        bannerRectTransform.anchoredPosition = targetPos;
        if (show)
        {
            yield return new WaitForSeconds(notificationDuration);

            show = !show;

            startPos = bannerRectTransform.anchoredPosition;
            targetPos = show ? onScreenPos : offScreenPos;
            elapsed = 0;

            while (elapsed < slideDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.SmoothStep(0f, 1f, elapsed / slideDuration);

                bannerRectTransform.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);

                yield return null;
            }

            bannerRectTransform.anchoredPosition = targetPos;
        }
    }
}
