using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue Asset", menuName = "Scriptable Objects/Dialogue")]
public class DialogueAsset : ScriptableObject
{
    public Speakers[] speaker;

    [TextArea]
    public string[] dialogue;

    [TextArea]
    public string[] options;

    public DialogueAsset option1;
    public DialogueAsset option2;
    public DialogueAsset option3;
    public DialogueAsset option4;
}
