using System;
using UnityEngine;

/// <summary>
/// Lớp cơ sở trừu tượng (Template Method Pattern) cho mọi câu đố trong Hồi 2 và Hồi 3.
/// Tự động đồng bộ trạng thái giải đố với StoryEventBus mà lớp con không cần lặp lại code.
/// </summary>
public abstract class BasePuzzle : MonoBehaviour, IPuzzle
{
    [Header("Base Puzzle Settings")]
    [SerializeField] protected string puzzleID = "DefaultPuzzle";
    [SerializeField] protected bool setStoryFlagOnSolve = true;
    [SerializeField] protected StoryFlag flagToTriggerOnSolve;
    [SerializeField] protected DialogueData solvedDialogue;

    public string PuzzleID => puzzleID;
    public bool IsSolved { get; protected set; }

    public event Action<IPuzzle> OnPuzzleSolved;

    public abstract void EvaluatePuzzle();
    public abstract void ResetPuzzle();

    /// <summary>
    /// Gọi hàm này từ lớp con ngay khi điều kiện giải đố được thỏa mãn.
    /// </summary>
    protected virtual void CompletePuzzle()
    {
        // TODO:
        // 1. Nếu IsSolved == true thì return (chặn kích hoạt trùng lặp)
        // 2. Gán IsSolved = true
        // 3. Phát sự kiện bằng cách gọi RaisePuzzleSolved()
        // 4. Nếu setStoryFlagOnSolve == true và StoryEventBus.Instance != null:
        //    -> Gọi StoryEventBus.Instance.SetFlag(flagToTriggerOnSolve, true)
        // 5. Nếu solvedDialogue != null:
        //    -> Gọi StoryEventBus.Instance.RequestDialogue(solvedDialogue)
    }

    /// <summary>
    /// Hàm hỗ trợ phát sự kiện hoàn thành câu đố cho các Observer lắng nghe.
    /// </summary>
    protected virtual void RaisePuzzleSolved() => OnPuzzleSolved?.Invoke(this);
}
