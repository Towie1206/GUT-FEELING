#pragma warning disable CS0414, CS0649
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// Đặt tại cửa kính lùa ra Ban công cuối Hồi 1.
/// Kiểm tra người chơi đã hoàn thành việc nhà (ăn mì, xem TV) chưa trước khi kích hoạt
/// Cutscene kinh hoàng: Bóng người mặc áo mưa đứng dưới sân chung cư nhìn lên phòng Nam.
/// </summary>
public class BalconyCutsceneTrigger : MonoBehaviour, IInteractable
{
    [Header("Story Prerequisites")]
    [SerializeField] private StoryFlag requiredNoodleFlag = StoryFlag.NoodleCooked;
    [SerializeField] private StoryFlag requiredTVFlag = StoryFlag.TVNewsWatched;
    [SerializeField] private DialogueData notReadyDialogue;

    [Header("Cutscene References")]
    [SerializeField] private PlayableDirector balconyTimeline;
    [SerializeField] private StoryFlag flagOnComplete = StoryFlag.SeenStrangerInRain;
    [SerializeField] private DialogueData afterCutsceneDialogue;

    private bool hasTriggered;

    public bool HasTriggered => hasTriggered;

    public string InteractionPrompt => hasTriggered ? string.Empty : "[E] Đóng cửa kính Ban công";

    public void Interact(Player player)
    {
        // TODO:
        // 1. Nếu đã kích hoạt rồi (hasTriggered == true) thì return
        // 2. Kiểm tra điều kiện tiên quyết qua StoryEventBus.Instance:
        //    - Phải có đủ cờ requiredNoodleFlag (đã ăn mì) VÀ requiredTVFlag (đã xem bản tin bão)
        //    - Nếu CHƯA ĐỦ: Gọi StoryEventBus.Instance.RequestDialogue(notReadyDialogue) rồi return
        // 3. Nếu ĐÃ ĐỦ điều kiện:
        //    - Đánh dấu hasTriggered = true
        //    - Gọi CutsceneController.Instance.PlayTimeline(balconyTimeline, OnCutsceneFinished)
    }

    private void OnCutsceneFinished()
    {
        // TODO:
        // 1. Bật cờ khép lại Hồi 1: StoryEventBus.Instance.SetFlag(flagOnComplete, true)
        // 2. Bắn câu độc thoại hoảng hốt của Nam sau khi bóng người biến mất trong ánh chớp:
        //    StoryEventBus.Instance.RequestDialogue(afterCutsceneDialogue)
    }
}
