using UnityEngine;

[CreateAssetMenu(fileName = "FishAsset", menuName = "Scriptable Objects/FishAsset")]
public class FishAsset : ScriptableObject
{
    public int weight;
    public GameObject fishPrefab;
    public DialogueAsset dialogue;
}
