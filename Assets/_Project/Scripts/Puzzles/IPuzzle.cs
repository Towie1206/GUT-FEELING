using System;
public interface IPuzzle
{
    // ID duy nhất của câu đố.
    string PuzzleID { get; }
    // Trạng thái hoàn thành của câu đố.
    bool IsSolved { get; }
    //Sự kiện phát ra khi người chơi hoàn thành câu đố.
    event Action<IPuzzle> OnPuzzleSolved;
    // Kiểm tra điều kiện thắng của câu đố.
    void EvaluatePuzzle();
    //Đưa câu đố về trạng thái ban đầu.
    void ResetPuzzle();
}
