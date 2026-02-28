using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    private string saveFilePath;

    void Awake()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "inventoryData.json");
    }

    public void SaveInventory(Inventory inv)
    {
        string json = JsonUtility.ToJson(inv, true); // 'true' for pretty print
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Inventory Saved to " + saveFilePath);
    }

    public Inventory LoadInventory()
    {
        Debug.Log("Seeing if file exists at " + saveFilePath);
        Debug.Log(saveFilePath);
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            Debug.Log("printing json read from file: " + json);
            Inventory data = JsonUtility.FromJson<Inventory>(json);
            Debug.Log("Inventory Loaded");
            return data;
        }
        else
        {
            Debug.LogWarning("Saved Inventory not found!");
            return null;
        }
    }

    public void ClearInventory()
    {
        File.WriteAllText(saveFilePath, "");
    }

    public void SavePlayerData()
    {
        Vector3 playerPos = GameObject.FindWithTag("Player").transform.position;
        string posJson = JsonUtility.ToJson(playerPos, true);

        PlayerSaveData data = new PlayerSaveData(GameManager.Instance.GetSceneId(), playerPos);
        string json = JsonUtility.ToJson(data, true);
        PlayerPrefs.SetString("PlayerData", json);
        PlayerPrefs.Save();
    }

    public PlayerSaveData LoadPlayerData()
    {
        if (PlayerPrefs.HasKey("PlayerData"))
        {
            string json = PlayerPrefs.GetString("PlayerData");
            PlayerSaveData data = JsonUtility.FromJson<PlayerSaveData>(json);
            return data;
        }
        else {
            Debug.LogWarning("Saved PlayerData not found!");
            return null;
        }
    }

}
