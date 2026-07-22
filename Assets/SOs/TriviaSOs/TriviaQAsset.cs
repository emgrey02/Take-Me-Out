using UnityEngine;

[CreateAssetMenu(fileName = "TriviaQAsset", menuName = "Scriptable Objects/TriviaQAsset")]
public class TriviaQAsset : ScriptableObject
{
    [TextArea]
    public string question;

    [TextArea]
    public string[] answers;

    public TriviaAAsset answer1;
    public TriviaAAsset answer2;
    public TriviaAAsset answer3;
    public TriviaAAsset answer4;
}
