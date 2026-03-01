using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject inventoryPrefab;
    public GameObject mainmenuPrefab;

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
        
        // instantiate prefabs based on scene num
        int sceneID = SceneManager.GetActiveScene().buildIndex;

        // change player location in baseball field depending on which scene they came from
        int prevSceneID = PlayerPrefs.GetInt("PrevSceneNum", 6);
        Vector3 pos;
        Quaternion rot;

        switch (sceneID)
        {
            case 1:
                // baseball field
                switch (prevSceneID)
                {
                    // from main menu
                    case 0:
                        pos = new Vector3(0, 1.1f, 0);
                        rot = Quaternion.Euler(0, 45, 0);
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
            case 2:
                // the burren
                pos = new Vector3(-8f, 1f, 0);
                rot = Quaternion.Euler(0, 100, 0);
                break;
            default:
                pos = new Vector3(0, 0, 0);
                rot = Quaternion.Euler(0, 0, 0);
                break;
        }

        if (sceneID == 0)
        {
            // main menu
            Instantiate(mainmenuPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            Instantiate(inventoryPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        }
        else
        {
            player = Instantiate(playerPrefab, pos, rot);
            Instantiate(inventoryPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            Instantiate(mainmenuPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        }
    }

    public void MoveToScene(int sceneID, float[] playerPos = null)
    {
        // save previous scene id to player prefs
        PlayerPrefs.SetInt("PrevSceneNum", GameManager.Instance.GetSceneId());
        PlayerPrefs.Save();

        // load scene
        SceneManager.LoadScene(sceneID);
    }

    public int GetSceneId()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

}
