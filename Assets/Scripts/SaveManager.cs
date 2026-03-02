using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    private string inventorySaveFilePath;
    private string playerSaveFilePath;

    void Awake()
    {
        inventorySaveFilePath = Path.Combine(Application.persistentDataPath, "inventoryData.json");
        playerSaveFilePath = Path.Combine(Application.persistentDataPath, "playerData.json");
    }

    public void SaveInventory(Inventory inv)
    {
        string json = JsonUtility.ToJson(inv, true); // 'true' for pretty print
        File.WriteAllText(inventorySaveFilePath, json);
        Debug.Log("Inventory Saved to " + inventorySaveFilePath);
    }

    public Inventory LoadInventory()
    {
        Debug.Log("Seeing if file exists at " + inventorySaveFilePath);
        Debug.Log(inventorySaveFilePath);
        if (File.Exists(inventorySaveFilePath))
        {
            string json = File.ReadAllText(inventorySaveFilePath);
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
        File.WriteAllText(inventorySaveFilePath, "");
    }

    public void SaveGraphicsQuality(int qualityLevel)
    {
        PlayerPrefs.SetInt("GraphicsQuality", qualityLevel);
        PlayerPrefs.Save();
        Debug.Log("Graphics Quality Saved");
    }

    public int LoadGraphicsQuality()
    {
        if (PlayerPrefs.HasKey("GraphicsQuality"))
        {
            Debug.Log("Graphics Quality found in player prefs");
            Debug.Log("Set Graphics Qual index: " + PlayerPrefs.GetInt("GraphicsQuality"));
            return PlayerPrefs.GetInt("GraphicsQuality");
        }
        else
        {
            Debug.LogWarning("No graphics quality saved to player prefs");
            return 12;
        }
    }

    public void SavePlayerData(int ls, int ms)
    {
        PlayerSaveData data = new PlayerSaveData(ls, ms);
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(playerSaveFilePath, json);
        Debug.Log("Player Data Saved to:" + playerSaveFilePath);
    }

    public PlayerSaveData LoadPlayerData()
    {
        if (File.Exists(playerSaveFilePath))
        {
            string json = File.ReadAllText(playerSaveFilePath);
            Debug.Log("printing json read from file: " + json);
            PlayerSaveData data = JsonUtility.FromJson<PlayerSaveData>(json);
            return data;
        }
        else {
            Debug.LogWarning("Saved PlayerData not found!");
            return null;
        }
    }

}
