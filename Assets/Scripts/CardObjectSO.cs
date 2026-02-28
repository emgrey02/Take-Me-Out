using UnityEngine;

[CreateAssetMenu(fileName = "CardPrefab", menuName = "Scriptable Objects/CardPrefab")]
public class CardObjectSO : ScriptableObject
{
    public string name;
    public Vector3 spawnPoint;
    public int cardIndex;
}
