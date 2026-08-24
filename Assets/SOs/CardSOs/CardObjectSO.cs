using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "Card Object", menuName = "Scriptable Objects/Card Object")]
public class CardObjectSO : ScriptableObject
{
    public string cardName;
    public Vector3 spawnPoint;
    public int cardIndex;
    public string cardImage;
    public string materialName;
}
