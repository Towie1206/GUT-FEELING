using System;
using System.Collections.Generic;
using UnityEngine;

public class StoryEventBus : MonoBehaviour, IStoryEventService
{
    public static IStoryEventService Instance { get; private set; }

    public event Action<DialogueData> OnDialogueRequested;
    public event Action<SMSData> OnSMSRequested;
    public event Action<StoryFlag> OnStoryFlagChanged;

    // Dùng HashSet để lưu danh sách các cờ đã bật (tránh trùng lặp, tra cứu tức thì O(1))
    private readonly HashSet<StoryFlag> activeFlags = new HashSet<StoryFlag>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }
    }

    public bool HasFlag(StoryFlag flag)
    {
        return activeFlags.Contains(flag);
    }

    public void SetFlag(StoryFlag flag, bool value)
    {
        if (value)
            activeFlags.Add(flag);
        else
            activeFlags.Remove(flag);

        OnStoryFlagChanged?.Invoke(flag);

    }

    public void RequestDialogue(DialogueData data)
    {
        if (data != null)
            OnDialogueRequested?.Invoke(data);
    }
    public void RequestSMS(SMSData data)
    {
        if (data != null)
            OnSMSRequested?.Invoke(data);
    }
}