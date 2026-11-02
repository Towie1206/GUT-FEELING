#pragma warning disable CS0414, CS0649
using System.Collections;
using UnityEngine;

/// <summary>
/// Tấm gương phòng tắm và Chiếc Két Sắt Âm Tường Bí Mật (Tuần 17 - 18).
/// Sau khi người chơi phát hiện bí mật trong minigame 2D RoomEscape.exe (StoryFlag.BathroomSafeRevealed),
/// người chơi có thể đẩy tấm gương thật trong Toilet sang bên và mở két sắt lấy:
/// - Chìa khóa khắc chữ "Tủ áo"
/// - CMND bị gạch chéo mặt của người thuê trước (chứa ngày sinh làm mật khẩu iPhone)
/// - Chiếc điện thoại iPhone cũ bị sập nguồn
/// </summary>
public class BathroomMirrorSafe : BasePuzzle, IInteractable
{
    [Header("Mirror Slide Settings")]
    [SerializeField] private Transform mirrorTransform;
    [SerializeField] private Vector3 mirrorSlideOffset = new Vector3(0.65f, 0f, 0f);
    [SerializeField] private float slideSpeed = 2.0f;
    [SerializeField] private AudioSource mirrorSlideAudio;

    [Header("Safe Door & Contents (Or Delegated SafeKeypadPuzzle)")]
    [SerializeField] private SafeKeypadPuzzle linkedSafePuzzle;
    [SerializeField] private Transform safeDoorTransform;
    [SerializeField] private float safeDoorOpenAngle = -95f;
    [SerializeField] private AudioSource safeUnlockAudio;
    [SerializeField] private GameObject[] itemsInsideSafe; // Chìa khóa tủ áo, CMND cũ, iPhone cạn pin

    [Header("Story Prerequisites & Feedback")]
    [SerializeField] private StoryFlag requiredDiscoveryFlag = StoryFlag.BathroomSafeRevealed;
    [SerializeField] private DialogueData normalMirrorDialogue;

    private bool isMirrorSlidAside;

    public bool IsMirrorSlidAside => isMirrorSlidAside;

    public string InteractionPrompt
    {
        get
        {
            // TODO:
            // 1. Nếu đã mở két sắt xong (IsSolved) -> trả về string.Empty
            // 2. Nếu chưa phát hiện qua minigame PC (!StoryEventBus.Instance.HasFlag(requiredDiscoveryFlag)) -> "[E] Soi gương"
            // 3. Nếu đã phát hiện nhưng chưa đẩy gương (!isMirrorSlidAside) -> "[E] Đẩy tấm gương sang bên"
            // 4. Nếu đã đẩy gương lộ két sắt -> "[E] Mở Két sắt âm tường"
            return IsSolved ? string.Empty : "[E] Kiểm tra Gương phòng tắm";
        }
    }

    public void Interact(Player player)
    {
        // TODO:
        // 1. Nếu IsSolved == true thì return
        // 2. Kiểm tra nếu chưa có cờ requiredDiscoveryFlag:
        //    - Bắn độc thoại normalMirrorDialogue ("Chỉ là tấm gương phòng tắm hơi ố mờ...") và return
        // 3. Nếu đã có cờ nhưng chưa đẩy gương (!isMirrorSlidAside):
        //    - Đặt isMirrorSlidAside = true
        //    - Chạy Coroutine trượt mirrorTransform theo mirrorSlideOffset để lộ chiếc két sắt âm tường!
        //    - (Tùy chọn SRP): Nếu có gắn linkedSafePuzzle riêng trên két sắt -> Bật Collider của linkedSafePuzzle và CompletePuzzle() trượt gương
        //    - return;
        // 4. Nếu đã đẩy gương -> Gọi EvaluatePuzzle() để mở cửa két sắt (hoặc gọi linkedSafePuzzle.Interact(player))
    }

    public override void EvaluatePuzzle()
    {
        // TODO:
        // 1. Xoay mở cửa két sắt (safeDoorTransform) theo góc safeDoorOpenAngle, phát âm thanh safeUnlockAudio
        // 2. Bật SetActive(true) cho các vật phẩm bên trong (itemsInsideSafe: Chìa khóa tủ áo, CMND, iPhone cũ)
        // 3. Gọi CompletePuzzle() (Tự động bật cờ StoryFlag.BathroomSafeOpened và bắn độc thoại)
    }

    public override void ResetPuzzle()
    {
        // TODO:
        // Đưa gương và cửa két sắt về vị trí đóng ban đầu
    }
}
