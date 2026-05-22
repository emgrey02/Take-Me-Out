using UnityEngine;

public class Base : MonoBehaviour
{
    public InputReader inputReader;

    [SerializeField]
    private int _baseNum;
    public int BaseNum
    { 
        get
        {
            return _baseNum;
        }
        set
        {
            _baseNum = value;
        }
    }

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
            Debug.Log("Sending to new scene");
            GameManager.Instance.MoveToScene(BaseNum + 1);
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
