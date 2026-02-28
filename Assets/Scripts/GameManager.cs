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
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;
        int sceneID = SceneManager.GetActiveScene().buildIndex;
        Vector3 pos;
        Quaternion rot;

        switch (sceneID)
        {
            case 1:
                pos = new Vector3(0, 1.1f, 0);
                rot = Quaternion.Euler(0, 45, 0);
                break;
            case 2:
                pos = new Vector3(-8f, 1f, 0);
                rot = Quaternion.Euler(0, -20, 0);
                break;
            default:
                pos = new Vector3(0, 0, 0);
                rot = Quaternion.Euler(0, 0, 0);
                break;
        }

        Instantiate(playerPrefab, pos, rot);
        Instantiate(inventoryPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        Instantiate(mainmenuPrefab, new Vector3(0, 0, 0), Quaternion.identity);
    }

    void OnEnable()
    {
    }

    public void MoveToScene(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }
}
