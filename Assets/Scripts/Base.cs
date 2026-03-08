using UnityEngine;
using UnityEngine.InputSystem;

public class Base : MonoBehaviour
{
    public InputReader inputReader;

    public int baseNum { get; set; }
    public GameObject EnterScenePrompt;
    public bool inBaseArea = false;

    void Start()
    {
        inputReader.InteractEvent += OnInteract;
    }

    void OnInteract(bool Interacted)
    {
        if (Interacted & inBaseArea)
        {
            Debug.Log("Send to first scene experience");
            //Transform player = GameObject.FindWithTag("Player").GetComponent<Transform>();
            //player.rotation = new Quaternion(0, 130f, 0, 0);
            //player.position = new Vector3 (0, .9f, 0);
            //Physics.SyncTransforms();
            GameManager.Instance.MoveToScene(baseNum + 1);
        }
    }

    void OnTriggerEnter(Collider player)
    {
        Debug.Log("Player entered area around base");
        EnterScenePrompt.SetActive(true);
        inBaseArea = true;
    }

    void OnTriggerExit(Collider player)
    {
        Debug.Log("Player left area around base");
        EnterScenePrompt.SetActive(false);
        inBaseArea = false;
    }

    void OnDisable()
    {
        inputReader.InteractEvent -= OnInteract;
    }
}
