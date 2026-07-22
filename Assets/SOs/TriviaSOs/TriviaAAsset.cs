using UnityEngine;

[CreateAssetMenu(fileName = "TriviaAAsset", menuName = "Scriptable Objects/TriviaAAsset")]
public class TriviaAAsset : ScriptableObject
{
    public bool correct;

    public string answer;

    public string image;

    [TextArea]
    public string info;

    public TriviaQAsset next;
}
