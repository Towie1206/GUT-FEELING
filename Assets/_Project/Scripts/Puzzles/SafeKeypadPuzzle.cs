#pragma warning disable CS0414, CS0649
using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// Câu đố Mở Khóa Két Sắt Âm Tường sau gương phòng tắm (SafeKeypadPuzzle - Tuần 18).
/// Kế thừa BasePuzzle & IInteractable: Cho phép người chơi bấm mật mã (hoặc vặn khóa) mở cửa két sắt
/// để thu thập 3 vật phẩm mấu chốt: Chìa khóa "Tủ áo", CMND bị gạch mặt và chiếc iPhone cũ cạn pin.
/// </summary>
public class SafeKeypadPuzzle : BasePuzzle, IInteractable
{
    [Header("Safe Lock Settings")]
    [SerializeField] private bool requirePasscode = false;
    [SerializeField] private string safePasscode = "0404"; // Mã phòng 404 hoặc mật mã tùy chỉnh của User

    [Header("Keypad UI (Optional if requirePasscode = true)")]
    [SerializeField] private GameObject safeKeypadCanvasUI;
    [SerializeField] private TextMeshProUGUI codeDisplayText;

    [Header("Safe Door & Contents")]
    [SerializeField] private Transform safeDoorTransform;
    [SerializeField] private float safeDoorOpenAngle = -95f;
    [SerializeField] private float doorOpenSpeed = 2.5f;
    [SerializeField] private GameObject[] itemsInsideSafe; // Chìa khóa tủ áo, CMND cũ, iPhone cạn pin

    [Header("Audio")]
    [SerializeField] private AudioSource keypadBeepAudio;
    [SerializeField] private AudioSource safeUnlockAudio;
    [SerializeField] private AudioSource wrongCodeAudio;

    private string currentInput = string.Empty;
    private bool isKeypadOpen;
    private Player currentPlayer;

    public bool IsKeypadOpen => isKeypadOpen;

    public string InteractionPrompt
    {
        get
        {
            // TODO:
            // - Nếu đã mở két sắt (IsSolved) hoặc đang mở bảng bấm số (isKeypadOpen) -> trả về string.Empty
            // - Ngược lại -> trả về "[E] Mở Két sắt âm tường"
            return (IsSolved || isKeypadOpen) ? string.Empty : "[E] Mở Két sắt âm tường";
        }
    }

    public void Interact(Player player)
    {
        // TODO:
        // 1. Nếu IsSolved == true thì return
        // 2. Lưu currentPlayer = player
        // 3. Nếu !requirePasscode (mở trực tiếp khi phát hiện sau gương):
        //    -> Gọi EvaluatePuzzle() để mở cửa két sắt ngay
        // 4. Nếu requirePasscode == true:
        //    -> Gọi OpenKeypadUI() để người chơi nhập mã số két sắt
    }

    public void OpenKeypadUI()
    {
        // TODO:
        // 1. Đặt isKeypadOpen = true, bật safeKeypadCanvasUI.SetActive(true)
        // 2. Khóa di chuyển và góc nhìn 3D của currentPlayer (vd: chuyển sang PlayerCutsceneState)
        // 3. Mở khóa con trỏ chuột (Cursor.lockState = CursorLockMode.None; Cursor.visible = true) để bấm nút
    }

    public void CloseKeypadUI()
    {
        // TODO:
        // 1. Đặt isKeypadOpen = false, tắt safeKeypadCanvasUI.SetActive(false)
        // 2. Trả currentPlayer về trạng thái tự do (currentPlayer.stateMachine.ChangeState(currentPlayer.freeState))
    }

    public void AppendDigit(string digit)
    {
        // TODO:
        // 1. Nếu IsSolved == true hoặc currentInput.Length >= safePasscode.Length thì return
        // 2. Cộng chuỗi currentInput += digit, cập nhật codeDisplayText và phát keypadBeepAudio
        // 3. Nếu đủ độ dài mật mã -> Gọi EvaluatePuzzle()
    }

    public void ClearInput()
    {
        // TODO:
        // Xóa currentInput = string.Empty và cập nhật codeDisplayText
    }

    public override void EvaluatePuzzle()
    {
        // TODO:
        // 1. Nếu requirePasscode == true và currentInput != safePasscode:
        //    - Phát âm thanh báo lỗi wrongCodeAudio, gọi ClearInput() và return
        // 2. Nếu hợp lệ (hoặc !requirePasscode):
        //    - Nếu đang mở UI bàn phím thì gọi CloseKeypadUI()
        //    - Phát âm thanh mở két safeUnlockAudio, xoay mở cửa két safeDoorTransform theo góc safeDoorOpenAngle
        //    - Bật SetActive(true) cho toàn bộ vật phẩm trong mảng itemsInsideSafe (Chìa khóa Tủ áo, CMND, iPhone cũ)
        //    - Gọi CompletePuzzle() (Tự động bật cờ StoryFlag.BathroomSafeOpened!)
    }

    public override void ResetPuzzle()
    {
        // TODO:
        // Đặt lại currentInput = string.Empty và đóng cửa két sắt về góc ban đầu
    }
}
