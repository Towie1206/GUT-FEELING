using System;

/// <summary>
/// Hợp đồng chung cho tất cả các câu đố (Puzzles) trong trò chơi (Cầu dao, Minigame PC, Két sắt, Sạc điện thoại).
/// Đảm bảo nguyên lý Open/Closed (OCP) và Liskov Substitution (LSP).
/// </summary>
public interface IPuzzle
{
    /// <summary>
    /// Mã định danh duy nhất của câu đố.
    /// </summary>
    string PuzzleID { get; }

    /// <summary>
    /// Trạng thái câu đố đã được giải thành công hay chưa.
    /// </summary>
    bool IsSolved { get; }

    /// <summary>
    /// Sự kiện phát ra khi người chơi hoàn thành câu đố.
    /// </summary>
    event Action<IPuzzle> OnPuzzleSolved;

    /// <summary>
    /// Kiểm tra điều kiện thắng của câu đố.
    /// </summary>
    void EvaluatePuzzle();

    /// <summary>
    /// Đưa câu đố về trạng thái ban đầu.
    /// </summary>
    void ResetPuzzle();
}
