public interface IHoldInteractable
{
    float HoldDuration { get; } // Thời gian (tính bằng giây) cần giữ phím để đạt 100% tiến trình.

    string ActionPrompt { get; } // Dòng chữ mô tả hành động hiển thị dưới thanh tiến trình (vd: "Đang mở chốt...", "Đang cạy ván...").

    bool CanHold { get; } //

    void OnHoldStart(); // Gọi khi người chơi bắt đầu nhấn giữ phím tương tác.

    void OnHoldProgress(float normalizedProgress); // "normalizedProgress" là tiến trình chuẩn hóa từ 0.0f đến 1.0f

    void OnHoldComplete(Player player);

    void OnHoldCancel(); // Gọi khi người chơi nhả phím giữa chừng hoặc quay góc nhìn đi chỗ khác trước khi đầy.

}
