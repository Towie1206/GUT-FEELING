using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;  

// Quản lý duy nhất 1 cần gạt 2D theo cơ chế PULL & HOLD 
public class BreakerLeverUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("UI & Settings")]
    [SerializeField] private Slider leverSlider;
    [SerializeField] private Image statusLedImage;
    [SerializeField] private RectTransform shakeTarget; // Kéo chính cái Handle của Slider vào đây để rung

    [Header("Threshold & Timing")]
    [SerializeField] private float pullThreshold = .95f;
    [SerializeField] private float requiredHoldTime = .5f;
    [SerializeField] private float snapBackSpeed = 8f;

    [Header("Juice & Feedback Settings")]
    [SerializeField] private float shakeIntensity = 3.5f;   // Biên độ rung (pixel)
    [SerializeField] private Sprite unlockedSprite; // Hình ảnh cho trạng thái chưa khóa
    [SerializeField] private Sprite lockedSprite;   // Hình ảnh cho trạng thái đã khóa

    [Header("Audio")]
    [SerializeField] private AudioSource thresholdCatchAudio; // Tiếng "cạch" nhẹ khi vừa kéo kịch nóc
    [SerializeField] private AudioSource lockSnapAudio;

    private bool isHoldingMouse;
    private bool isLocked;
    private float holdTimer;
    private bool hasPlayedCatchAudio;
    private Vector2 originalHandlePos;

    public bool IsLocked => isLocked;
    public float LeverValue => leverSlider != null ? leverSlider.value : 0f;
    public event Action OnLeverLocked;

    private void Awake()
    {
        if (shakeTarget != null) originalHandlePos = shakeTarget.anchoredPosition;
        if (statusLedImage != null) statusLedImage.sprite = unlockedSprite;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isLocked) return;
        isHoldingMouse = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isLocked) return;
        isHoldingMouse = false;
        holdTimer = 0f;
        hasPlayedCatchAudio = false;

        // Trả lại vị trí handle nếu buông tay giữa chừng
        if (shakeTarget != null) shakeTarget.anchoredPosition = originalHandlePos;
    }

    void Update()
    {
        if (isLocked) return;

        if (isHoldingMouse && leverSlider.value >= pullThreshold)
        {
            if (!hasPlayedCatchAudio)
            {
                if (thresholdCatchAudio != null) thresholdCatchAudio.Play();
                hasPlayedCatchAudio = true;
            }

            holdTimer += Time.deltaTime;
            ApplyVibration();

            if (holdTimer >= requiredHoldTime)
                LookLever();
        }

        else if (isHoldingMouse && leverSlider.value < pullThreshold)
            holdTimer = 0f;

        else if (!isHoldingMouse && leverSlider.value > 0f)
        {
            leverSlider.value = Mathf.MoveTowards(leverSlider.value, 0f, snapBackSpeed * Time.deltaTime);
        }
    }

    private void ApplyVibration()
    {
        //Tạo độ rung ngẫu nhiên cho Handle quanh vị trí gốc originalHandlePos:
        float randomX = Random.Range(-1f, 1f);
        float randomY = Random.Range(-1f, 1f);

        Vector2 offset = new Vector2(randomX, randomY) * shakeIntensity;

        shakeTarget.anchoredPosition = originalHandlePos + offset;
    }

    private void LookLever()
    {
        isLocked = true;
        leverSlider.value = 1f;
        leverSlider.interactable = false;

        if (shakeTarget != null) shakeTarget.anchoredPosition = originalHandlePos;

        if (statusLedImage != null) statusLedImage.sprite = lockedSprite;

        if (lockSnapAudio != null) lockSnapAudio.Play();

        // TODO: Chạy Coroutine nảy nhẹ scale của Handle (Punch Scale từ 1.0 -> 1.2 -> 1.0 trong 0.1s)
        StartCoroutine(Routine_PunchScale());
        OnLeverLocked?.Invoke();
    }

    private IEnumerator Routine_PunchScale()
    {
        float duration = .12f;
        float elapsedTime = 0f;
        Vector3 originalScale = Vector3.one;
        while(elapsedTime < duration)
        {
            float t =Mathf.Clamp01(elapsedTime / duration);
            float scaleMultiplier = 1f + Mathf.Sin(t * Mathf.PI) * .2f;  // sin(0 -> pi) tự động nảy từ 0 -> 1 -> 0
            if (shakeTarget != null) shakeTarget.localScale = originalScale + new Vector3(scaleMultiplier, scaleMultiplier, 0f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        shakeTarget.localScale = originalScale;
    }


    private void ResetLever()
    {
        isLocked = false;
        leverSlider.value = 0f;
        leverSlider.interactable = true;
        holdTimer = 0f;
        hasPlayedCatchAudio = false;
        if (statusLedImage != null) statusLedImage.sprite = unlockedSprite;
        if (shakeTarget != null) shakeTarget.anchoredPosition = originalHandlePos;
    }
}
