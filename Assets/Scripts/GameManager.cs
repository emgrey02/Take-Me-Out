using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject inventoryPrefab;
    public GameObject mainmenuPrefab;
    
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
        
        // instatiate prefabs based on scene num
        int sceneID = SceneManager.GetActiveScene().buildIndex;
        Vector3 pos;
        Quaternion rot;

        switch (sceneID)
        {
            case 1:
                // baseball field
                pos = new Vector3(0, 1.1f, 0);
                rot = Quaternion.Euler(0, 45, 0);
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
            Instantiate(playerPrefab, pos, rot);
            Instantiate(inventoryPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            Instantiate(mainmenuPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        }
    }

    public void MoveToScene(int sceneID, float[] playerPos = null)
    {
        if (playerPos != null)
        {
            //player.position = new Vector3 (0, .9f, 0);
            //Physics.SyncTransforms();
        }
        SceneManager.LoadScene(sceneID);
    }

    public int GetSceneId()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

}
