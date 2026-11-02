#pragma warning disable CS0414, CS0649
using UnityEngine;
using TMPro;

/// <summary>
/// Câu đố Sạc Pin & Mở Khóa Mật Khẩu 6 Số trên chiếc iPhone cũ của nạn nhân trước (Tuần 19 - 20).
/// Người chơi đặt iPhone vào ổ cắm sạc (ItemSocket), nhìn ngày sinh trên CMND bị gạch mặt (Soi 3D Inspect)
/// để nhập mật khẩu 6 số và đọc tin nhắn nháp chưa kịp gửi: "Có ai đó đang sống trong tủ quần áo của tôi..."
/// </summary>
public class PhoneChargingPuzzle : BasePuzzle, IInteractable
{
    [Header("Charger Socket & Passcode")]
    [SerializeField] private ItemSocket phoneChargerSocket;
    [SerializeField] private string correctPasscode = "140898"; // Ngày sinh trên CMND của người thuê trước

    [Header("Lockscreen UI References")]
    [SerializeField] private GameObject lockscreenCanvasUI;
    [SerializeField] private TextMeshProUGUI passcodeDisplayText;
    [SerializeField] private GameObject draftMessageScreenUI;

    [Header("Audio")]
    [SerializeField] private AudioSource plugInChargeAudio;
    [SerializeField] private AudioSource keypadBeepAudio;
    [SerializeField] private AudioSource unlockSuccessAudio;
    [SerializeField] private AudioSource wrongCodeBuzzAudio;

    [Header("Story Feedback")]
    [SerializeField] private DialogueData needPhoneOnChargerDialogue;

    private string currentInputCode = string.Empty;
    private bool isLockscreenOpen;
    private Player currentPlayer;

    public bool IsPhonePluggedIn => phoneChargerSocket != null && phoneChargerSocket.HasItem;
    public bool IsLockscreenOpen => isLockscreenOpen;

    public string InteractionPrompt
    {
        get
        {
            // TODO:
            // 1. Nếu đang mở màn hình khóa (isLockscreenOpen) -> trả về string.Empty
            // 2. Nếu đã mở khóa xong (IsSolved) -> "[E] Đọc lại tin nhắn trong iPhone cũ"
            // 3. Nếu đã đặt điện thoại lên đế sạc (IsPhonePluggedIn) -> "[E] Nhập mật khẩu mở khóa iPhone"
            // 4. Nếu chưa cắm sạc -> "[E] Ổ cắm sạc điện thoại"
            return isLockscreenOpen ? string.Empty : "[E] Kiểm tra Sạc điện thoại";
        }
    }

    private void Update()
    {
        // TODO:
        // Nếu đang mở màn hình khóa (isLockscreenOpen) và người chơi bấm phím Thoát (Tab / Chuột phải):
        // -> Gọi CloseLockscreen() để người chơi có thể thoát ra đi soi lại ngày sinh trên CMND!
    }

    public void Interact(Player player)
    {
        // TODO:
        // 1. Kiểm tra nếu chưa đặt iPhone cũ vào phoneChargerSocket (!IsPhonePluggedIn):
        //    - Bắn độc thoại needPhoneOnChargerDialogue ("Phải đặt chiếc điện thoại sập nguồn vào dây sạc đã.")
        //    - return;
        // 2. Lưu tham chiếu currentPlayer = player
        // 3. Bật cờ StoryEventBus.Instance.SetFlag(StoryFlag.PhonePluggedIn, true)
        // 4. Gọi OpenLockscreen()
    }

    /// <summary>
    /// Mở giao diện màn hình khóa iPhone, khóa góc nhìn 3D của Player và hiện con trỏ chuột để bấm số.
    /// </summary>
    public void OpenLockscreen()
    {
        // TODO:
        // 1. Đặt isLockscreenOpen = true
        // 2. Chuyển currentPlayer sang trạng thái khóa di chuyển/xoay 3D (vd: PlayerCutsceneState)
        //    để khi di chuột bấm nút mật khẩu không làm xoay camera 3D!
        // 3. Mở khóa con trỏ chuột: Cursor.lockState = CursorLockMode.None; Cursor.visible = true;
        // 4. Bật lockscreenCanvasUI.SetActive(true) (hoặc draftMessageScreenUI nếu IsSolved == true)
    }

    /// <summary>
    /// Gắn hàm này vào nút [X / Đóng] trên UI màn hình khóa để thoát ra ngoài (tránh Soft-lock khi chưa biết mật khẩu!).
    /// </summary>
    public void CloseLockscreen()
    {
        // TODO:
        // 1. Đặt isLockscreenOpen = false
        // 2. Tắt lockscreenCanvasUI.SetActive(false) và draftMessageScreenUI.SetActive(false)
        // 3. Trả currentPlayer về trạng thái di chuyển tự do:
        //    currentPlayer.stateMachine.ChangeState(currentPlayer.freeState)
    }

    /// <summary>
    /// Gắn hàm này vào sự kiện OnClick của các nút bấm từ 0 -> 9 trên UI bàn phím khóa.
    /// </summary>
    public void AppendDigit(string digit)
    {
        // TODO:
        // 1. Nếu IsSolved == true hoặc currentInputCode.Length >= 6 thì return
        // 2. Cộng chuỗi currentInputCode += digit và cập nhật lên passcodeDisplayText
        // 3. Phát tiếng bấm phím (keypadBeepAudio)
        // 4. Nếu đủ 6 ký tự (currentInputCode.Length == 6) -> Gọi EvaluatePuzzle()
    }

    /// <summary>
    /// Gắn hàm này vào nút Xóa (Clear/Backspace) trên UI màn hình khóa.
    /// </summary>
    public void ClearInput()
    {
        // TODO:
        // Đặt lại currentInputCode = string.Empty và cập nhật passcodeDisplayText
    }

    public override void EvaluatePuzzle()
    {
        // TODO:
        // 1. So sánh currentInputCode với correctPasscode
        // 2. Nếu ĐÚNG:
        //    - Phát âm thanh mở khóa (unlockSuccessAudio)
        //    - Chuyển sang màn hình tin nhắn nháp (draftMessageScreenUI.SetActive(true))
        //    - Bật cờ StoryFlag.PhoneUnlocked và StoryFlag.LastTenantMessageRead
        //    - Gọi CompletePuzzle()
        // 3. Nếu SAI:
        //    - Phát âm thanh báo lỗi (wrongCodeBuzzAudio)
        //    - Gọi ClearInput() để người chơi nhập lại
    }

    public override void ResetPuzzle()
    {
        // TODO:
        // Xóa currentInputCode và gọi CloseLockscreen()
    }
}
