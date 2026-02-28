using UnityEngine;

[System.Serializable]
public class PlayerSaveData
{
    public int sceneNum;
    public Vector3 position;

    public PlayerSaveData(int sceneNum, Vector3 position)
    {
        this.sceneNum = sceneNum;
        this.position = position;
    }
}
