using System;
using UnityEngine;

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

    protected virtual void CompletePuzzle()
    {
        if(IsSolved) return;
        IsSolved = true;
        RaisePuzzleSolved();
        if(setStoryFlagOnSolve && StoryEventBus.Instance != null)
        {
            StoryEventBus.Instance.SetFlag(flagToTriggerOnSolve, true);
        }
        if(solvedDialogue != null)
        {
            StoryEventBus.Instance.RequestDialogue(solvedDialogue);
        }
    }

    //Hàm hỗ trợ phát sự kiện hoàn thành câu đố cho các Observer lắng nghe.
    protected virtual void RaisePuzzleSolved() => OnPuzzleSolved?.Invoke(this);
}
