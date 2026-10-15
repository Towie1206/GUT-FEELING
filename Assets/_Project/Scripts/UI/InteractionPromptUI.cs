using TMPro;
using UnityEngine;

public class InteractionPromptUI : MonoBehaviour
{
    [SerializeField] private GameObject promptContainer;
    [SerializeField] private TMP_Text promptText;

    [SerializeField] private PlayerInteraction playerInteraction;

    private void OnEnable()
    {
        playerInteraction.OnInteractableFound += Show;
        playerInteraction.OnInteractableLost += Hide;
    }

    private void OnDisable()
    {
        playerInteraction.OnInteractableFound -= Show;
        playerInteraction.OnInteractableLost -= Hide;
    }

    private void Awake()
    {
        promptContainer.SetActive(false);
    }

    private void Show(string actionName)
    {
        promptContainer.SetActive(true);
        promptText.text = actionName;
    }

    private void Hide()
    {
        promptContainer.SetActive(false);
        promptText.text = string.Empty;
    }
}
