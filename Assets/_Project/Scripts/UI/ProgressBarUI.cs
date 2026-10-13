using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject progressContainer;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private TextMeshProUGUI actionPromptText;

    [SerializeField] private PlayerInteraction playerInteraction;

    private void OnEnable()
    {
        playerInteraction.OnHoldStarted += Show;
        playerInteraction.OnHoldProgressChanged += UpdateProgress;
        playerInteraction.OnHoldEnded += Hide;
    }

    private void OnDisable()
    {
        playerInteraction.OnHoldStarted -= Show;
        playerInteraction.OnHoldProgressChanged -= UpdateProgress;
        playerInteraction.OnHoldEnded -= Hide;

    }

    private void Awake()
    {
        progressContainer.SetActive(false);
    }
    public void Show(string prompt)
    {
        progressContainer.SetActive(true);
        actionPromptText.text = prompt;
        progressSlider.value = 0f;
    }

    public void UpdateProgress(float normalizedValue)
    {
        progressSlider.value = Mathf.Clamp01(normalizedValue);
    }


    public void Hide()
    {
        progressContainer.SetActive(false);
        progressSlider.value = 0f;
    }
}

