#pragma warning disable CS0414, CS0649
using System.Collections;
using UnityEngine;

/// <summary>
/// Câu đố Hộp Cầu Dao (Fuse Box) trong phòng kho ở Hồi 2 (Tuần 14 - 15).
/// Kết hợp với 2 ItemSocket (khe cắm Cầu chì Đỏ & Cầu chì Vàng). Khi cắm đủ 2 cầu chì
/// và người chơi gạt cần dao (Interact), nguồn điện toàn căn hộ sẽ được khôi phục (StoryFlag.PowerRestored).
/// </summary>
public class FuseBoxPuzzle : BasePuzzle, IInteractable
{
    [Header("Fuse Sockets")]
    [SerializeField] private ItemSocket redFuseSocket;
    [SerializeField] private ItemSocket yellowFuseSocket;

    [Header("Box Door & Lever Transforms")]
    [SerializeField] private Transform boxCoverDoor;
    [SerializeField] private Transform breakerSwitchLever;
    [SerializeField] private float doorOpenAngle = -100f;
    [SerializeField] private float leverUpAngle = -45f;

    [Header("Audio & VFX")]
    [SerializeField] private AudioSource switchFlipAudio;
    [SerializeField] private ParticleSystem sparkVFX;
    [SerializeField] private DialogueData missingFusesDialogue;

    private bool isBoxOpened;

    public bool IsBoxOpened => isBoxOpened;

    public string InteractionPrompt
    {
        get
        {
            // TODO:
            // 1. Nếu câu đố đã giải xong (IsSolved) -> trả về string.Empty
            // 2. Nếu chưa mở nắp hộp (!isBoxOpened) -> trả về "[E] Mở nắp Hộp cầu dao"
            // 3. Nếu đã mở nắp hộp -> trả về "[E] Gạt cần cầu dao"
            return IsSolved ? string.Empty : "[E] Tương tác Hộp cầu dao";
        }
    }

    public void Interact(Player player)
    {
        // TODO:
        // 1. Nếu IsSolved == true thì return
        // 2. Nếu nắp hộp chưa mở (!isBoxOpened):
        //    - Đặt isBoxOpened = true, xoay mở nắp hộp boxCoverDoor
        //    - Bật cờ StoryEventBus.Instance.SetFlag(StoryFlag.FuseBoxOpened, true)
        //    - Bắn độc thoại missingFusesDialogue ("Thiếu mất 2 cái cầu chì rồi...")
        //    - return;
        // 3. Nếu nắp hộp đã mở -> Gọi EvaluatePuzzle() để kiểm tra và gạt cần dao
    }

    public override void EvaluatePuzzle()
    {
        // TODO:
        // 1. Kiểm tra xem cả redFuseSocket.HasItem VÀ yellowFuseSocket.HasItem đã bằng true chưa
        // 2. Nếu CHƯA ĐỦ 2 cầu chì:
        //    - Phát hiệu ứng tia lửa điện xẹt (sparkVFX) + độc thoại nhắc tìm nốt cầu chì
        //    - return;
        // 3. Nếu ĐÃ ĐỦ 2 cầu chì:
        //    - Xoay cần gạt breakerSwitchLever lên góc leverUpAngle, phát âm thanh switchFlipAudio
        //    - Gọi CompletePuzzle() (Hàm của lớp cha BasePuzzle sẽ tự bật cờ StoryFlag.PowerRestored!)
    }

    public override void ResetPuzzle()
    {
        // TODO:
        // Đặt lại IsSolved = false và góc xoay cần gạt về trạng thái ban đầu nếu cần
    }
}
