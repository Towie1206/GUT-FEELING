using UnityEngine;

[CreateAssetMenu(fileName = "D_NewDialogue")]
public class DialogueData : ScriptableObject
{
    [SerializeField, TextArea(2, 5)]
    private string[] lines;

    public int LineCount => lines == null ? 0 : lines.Length;

    public string GetLine(int index) => lines[index];
}
