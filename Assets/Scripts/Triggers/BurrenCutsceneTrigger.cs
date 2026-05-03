using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.Playables;

public class BurrenCutsceneTrigger : MonoBehaviour
{
    public InputReader inputReader;

    public GameObject EnterCutscenePrompt;
    public bool inTableArea = false;

    // objects with timelines
    public GameObject cameraMove;
    public GameObject sipAndFinishAnimation;

    // director that controls cameraMove timeline
    public PlayableDirector firstDirector;

    // director that controls sipandFinishAnimation timeline
    public PlayableDirector secondDirector;

    // SO dialogue asset
    public DialogueAsset dialogue;

    void OnEnable()
    {
        firstDirector.paused += OnFirstDirectorPaused;
        secondDirector.stopped += OnSecondDirectorStopped;
        DialogueBoxController.OnDialogueEnded += LeaveConversation;
    }

    void OnFirstDirectorPaused(PlayableDirector aDirector)
    {
        if (firstDirector == aDirector)
        {
            DialogueBoxController.instance.StartDialogue(dialogue);
        }
    }

    void OnSecondDirectorStopped(PlayableDirector aDirector)
    {
        if (secondDirector == aDirector)
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
            cameraMove.SetActive(true);
            Camera.main.GetComponent<CinemachineBrain>().enabled = true;
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

    void LeaveConversation()
    {
        sipAndFinishAnimation.SetActive(true);
    }

    void OnDisable()
    {
        inputReader.InteractEvent -= OnInteract;
        firstDirector.paused -= OnFirstDirectorPaused;
        secondDirector.stopped -= OnSecondDirectorStopped;
        DialogueBoxController.OnDialogueEnded -= LeaveConversation;
    }
}
