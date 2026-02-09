using UnityEngine;
using UnityEngine.InputSystem;

public class Base : MonoBehaviour
{
    public InputAction Interact;
    public int baseNum;
    public GameObject EnterScenePrompt;
    public bool inBaseArea = false;
    public SceneController sceneController;

    void Start()
    {
        Interact = InputSystem.actions.FindAction("Interact");
    }

    void Update()
    {
        if (Interact.triggered & inBaseArea)
        {
            Debug.Log("Send to first scene experience");
            Transform player = GameObject.FindWithTag("Player").GetComponent<Transform>();
            player.rotation = new Quaternion(0, 130f, 0, 0);
            player.position = new Vector3 (0, .9f, 0);
            Physics.SyncTransforms();
            sceneController.MoveToScene(baseNum + 1);
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
}
