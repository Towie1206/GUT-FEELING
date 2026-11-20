using System;
using UnityEngine;
using UnityEngine.Rendering;

// Quản lý Panel Minigame 2D gồm 5 cần gạt cầu dao.
public class FuseBoxMinigameUI : MonoBehaviour
{
    [SerializeField] private GameObject minigamePanel;
    [SerializeField] private BreakerLeverUI[] breakerLevers;
    [SerializeField] private Volume blurVolume;

    public event Action OnAllLeversLocked;

    private void Awake()
    {
        if (minigamePanel != null)
            minigamePanel.SetActive(false);
    }

    private void OnEnable()
    {
        foreach (var lever in breakerLevers)
        {
            lever.OnLeverLocked += HandleLeverLocked;
        }
    }

    private void OnDisable()
    {
        foreach (var lever in breakerLevers)
        {
            lever.OnLeverLocked -= HandleLeverLocked;
        }
    }

    public void Show()
    {
        minigamePanel.SetActive(true);
        if (blurVolume != null) blurVolume.weight = 1f;

       Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void Hide()
    {
        minigamePanel.SetActive(false);
        if (blurVolume != null) blurVolume.weight = 0f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void HandleLeverLocked()
    {

        foreach(var lever in breakerLevers)
        {
            if (!lever.IsLocked)
                return; 
        }
        Hide();
        OnAllLeversLocked?.Invoke();
    }

}
