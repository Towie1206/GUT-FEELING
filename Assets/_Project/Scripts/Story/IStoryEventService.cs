using System;

public interface IStoryEventService
{
    event Action<StoryFlag> OnStoryFlagChanged;
    event Action<DialogueData> OnDialogueRequested;
    event Action<SMSData> OnSMSRequested;

    bool HasFlag(StoryFlag flag);
    void SetFlag(StoryFlag flag, bool value);

    void RequestDialogue(DialogueData data);
    void RequestSMS(SMSData data);
}