using UnityEngine;

[System.Serializable]
public class PlayerSaveData
{
    public int lookSensitivity;
    public int moveSpeed;

    public PlayerSaveData(int lookSensitivity, int moveSpeed)
    {
        this.lookSensitivity = lookSensitivity;
        this.moveSpeed = moveSpeed;
    }
}
