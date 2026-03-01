using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.Playables;

public class BurrenCutsceneTrigger : MonoBehaviour
{
    public InputReader inputReader;

    public GameObject EnterCutscenePrompt;
    public bool inTableArea = false;
    public GameObject Cutscene;
    public PlayableDirector director;

    void OnEnable()
    {
        director.stopped += OnPlayableDirectorStopped;
    }

    void OnPlayableDirectorStopped(PlayableDirector aDirector)
    {
        if (director == aDirector)
        {
            // go back to baseball field
            GameManager.Instance.MoveToScene(1);
        }
    }

    void Start()
    {
        inputReader.InteractEvent += OnInteract;
    }

    void OnInteract(bool Interacted)
    {
        if (Interacted & inTableArea)
        {
            // trigger cutscene
            EnterCutscenePrompt.SetActive(false);
            Cutscene.SetActive(true);
            Camera.main.GetComponent<CinemachineBrain>().enabled = true;
            //GameManager.Instance.MoveToScene(baseNum + 1);
        }
    }

    void OnTriggerEnter(Collider player)
    {
        Debug.Log("Player entered area around table");
        EnterCutscenePrompt.SetActive(true);
        inTableArea = true;
    }

    void OnTriggerExit(Collider player)
    {
        Debug.Log("Player left area around table");
        EnterCutscenePrompt.SetActive(false);
        inTableArea = false;
    }

    void OnDisable()
    {
        inputReader.InteractEvent -= OnInteract;
        director.stopped -= OnPlayableDirectorStopped;
    }
}
