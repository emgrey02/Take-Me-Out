using UnityEngine;

public class BacktoBaseballField : MonoBehaviour
{
    public InputReader inputReader;

    public GameObject EnterScenePrompt;
    public bool inArea = false;

    void Start()
    {
        inputReader.InteractEvent += OnInteract;
    }

    void OnInteract(bool Interacted)
    {
        if (Interacted & inArea)
        {
            Debug.Log("Send to baseball field");
            //Transform player = GameObject.FindWithTag("Player").GetComponent<Transform>();
            //player.rotation = new Quaternion(0, 130f, 0, 0);
            //player.position = new Vector3 (0, .9f, 0);
            //Physics.SyncTransforms();
            GameManager.Instance.MoveToScene(1);
        }
    }

    void OnTriggerEnter(Collider player)
    {
        Debug.Log("Player entered area");
        EnterScenePrompt.SetActive(true);
        inArea = true;
    }

    void OnTriggerExit(Collider player)
    {
        Debug.Log("Player left area");
        EnterScenePrompt.SetActive(false);
        inArea = false;
    }

    void OnDisable()
    {
        inputReader.InteractEvent -= OnInteract;
    }
}
