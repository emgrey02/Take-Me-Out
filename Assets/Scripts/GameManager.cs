using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject inventoryPrefab;
    public GameObject mainmenuPrefab;
    public GameObject basePrefab;

    private SaveManager SaveManager;
    private PlayerMovement PlayerController;

    private GameObject player;
    
    private static GameManager _instance;

    public static GameManager Instance 
    {
        get { return _instance; }
    }

    void Awake()
    {
        // make sure theres only one instance of GameManager
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;

        // get save manager
        SaveManager = GameObject.FindWithTag("SaveManager").GetComponent<SaveManager>();
        
        InstantiatePrefabs();   

        // get player controller
        PlayerController = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
    }

    // Instantiate prefabs for the current scene
    private void InstantiatePrefabs()
    {
        // get current & previous scene id
        int sceneID = SceneManager.GetActiveScene().buildIndex;
        int prevSceneID = PlayerPrefs.GetInt("PrevSceneNum", 6);

        // set player prefab position and rotation based on scene number
        Vector3 pos;
        Quaternion rot;
        switch (sceneID)
        {
            // change player location in baseball field depending on which scene they came from
            case 1:
                switch (prevSceneID)
                {
                    // from main menu
                    case 0:
                        pos = new Vector3(0, 1.1f, 0);
                        rot = Quaternion.Euler(0, 45, 0);
                        // add first base
                        Instantiate(basePrefab, new Vector3(27.2f, .05f, .2f), Quaternion.identity);
                        break;
                    // from first base exp
                    case 2:
                        pos = new Vector3(27.2f, 1.1f, 1.6f);
                        rot = Quaternion.Euler(0, 0, 0);
                        break;
                    // from second base exp
                    case 3:
                        pos = new Vector3(26.5f, 1.1f, 27.3f);
                        rot = Quaternion.Euler(0, -88, 0);
                        break;
                    // from third base exp
                    case 4:
                        pos = new Vector3(0, 1.1f, 25.8f);
                        rot = Quaternion.Euler(0, -180, 0);
                        break;
                    // from home base exp
                    case 5:
                        pos = new Vector3(0, 1.1f, 0);
                        rot = Quaternion.Euler(0, 45, 0);
                        break;
                    default:
                        pos = new Vector3(0, 1.1f, 0);
                        rot = Quaternion.Euler(0, 45, 0);
                        break;
                }
                break;
            // the burren
            case 2:
                pos = new Vector3(-8f, 1f, 0);
                rot = Quaternion.Euler(0, 100, 0);
                break;
            // somerville theater
            case 3:
                pos = new Vector3(2f, 1f, 2f);
                rot = Quaternion.Euler(0, 45, 0);
                break;
            default:
                pos = new Vector3(0, 0, 0);
                rot = Quaternion.Euler(0, 0, 0);
                break;
        }

        // instantiate prefabs
        Instantiate(playerPrefab, pos, rot);
        Instantiate(inventoryPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        Instantiate(mainmenuPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        
    }

    void Start()
    {
        // set initial data
        int sceneID = SceneManager.GetActiveScene().buildIndex;
        SetInitPlayerData();

        if (sceneID == 0)
        {   
            SetInitGraphicsQuality();
        }
    }

    private void SetInitPlayerData()
    {
        PlayerSaveData saveData = SaveManager.LoadPlayerData();
        if (saveData != null)
        {   
            Debug.Log("Setting Init Player Data");
            Debug.Log("look sensitivity: " + saveData.lookSensitivity);
            Debug.Log("move speed: " + saveData.moveSpeed);
            PlayerController.lookSensitivity = saveData.lookSensitivity;
            PlayerController.moveSpeed = saveData.moveSpeed;
        }
        else
        {
            PlayerController.lookSensitivity = 20;
            PlayerController.moveSpeed = 4;
        }
    }

    private void SetInitGraphicsQuality()
    {
        int graphicsQualIndex = SaveManager.LoadGraphicsQuality(); 
        if (graphicsQualIndex != 12)
        {
            Debug.Log("Setting initial graphics quality from Game Manager");
            QualitySettings.SetQualityLevel(graphicsQualIndex, true);
        }
    }

    // set graphics quality from menu
    public void SetGraphicsQuality(int graphicsQual)
    {
        QualitySettings.SetQualityLevel(graphicsQual, false);

        // save it
        SaveManager.SaveGraphicsQuality(graphicsQual);
    }

    // set player data from menu
    public void SetPlayerData(int ls, int ms)
    {
        PlayerController.lookSensitivity = ls;
        PlayerController.moveSpeed = ms;
        
        // save it
        SaveManager.SavePlayerData(ls, ms);
    }

    public PlayerSaveData GetPlayerData()
    {
        return SaveManager.LoadPlayerData();
    }

    public void MoveToScene(int sceneID, float[] playerPos = null)
    {
        // save previous scene id to player prefs
        PlayerPrefs.SetInt("PrevSceneNum", GameManager.Instance.GetSceneId());
        PlayerPrefs.Save();

        // load scene
        SceneManager.LoadScene(sceneID);
    }

    public void ClearInventory()
    {
        SaveManager.ClearInventory();
    }

    public int GetSceneId()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

}
