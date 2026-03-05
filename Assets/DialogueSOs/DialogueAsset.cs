using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue Asset", menuName = "Scriptable Objects/Dialogue")]
public class DialogueAsset : ScriptableObject
{
    [TextArea]
    public string[] dialogue;
    public string speaker;
}
