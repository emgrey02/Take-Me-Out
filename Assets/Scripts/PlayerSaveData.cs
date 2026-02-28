using UnityEngine;

[System.Serializable]
public class PlayerSaveData
{
    public int sceneNum;
    public float[] position;

    public PlayerSaveData(int sceneNum, Vector3 position)
    {
        this.sceneNum = sceneNum;
        this.position = new float[3];
        this.position[0] = position.x;
        this.position[1] = position.y;
        this.position[2] = position.z;
    }
}
