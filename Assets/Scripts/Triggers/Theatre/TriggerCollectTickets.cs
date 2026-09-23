using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.Playables;
using FMODUnity;

public class TriggerCollectTickets:MonoBehaviour
{
    public InputReader inputReader;

    // objects with timelines
    public GameObject cameraMove;

    // director that controls cameraMove timeline
    public PlayableDirector firstDirector;

    // SO dialogue asset
    public DialogueAsset dialogue;
    public DialogueAsset noTicketDialogue;

    public TrackTheaterEvents theaterTracker;

    // FMOD Events
    [SerializeField] EventReference enterDialogue;
    [SerializeField] EventReference dialogueExit;

    void OnEnable()
    {
        firstDirector.paused += OnFirstDirectorPaused;
    }

    void OnFirstDirectorPaused(PlayableDirector aDirector)
    {
        Debug.Log("first director paused");
        if (firstDirector == aDirector)
        {
            if (theaterTracker.isTicketBought()) {
                DialogueBoxController.instance.StartDialogue(dialogue);
            } else {
                DialogueBoxController.instance.StartDialogue(noTicketDialogue);
            }
        }
    }

    void OnTriggerEnter(Collider player)
    {
        Debug.Log("Player entered buy ticket area");
        // FMOD
        // Play enter dialogue sfx
        RuntimeManager.PlayOneShot(enterDialogue);
        DialogueBoxController.OnDialogueEnded += LeaveConversation;
        Debug.Log("triggering cutscene");
        inputReader.DisablePlayerControls();
        cameraMove.SetActive(true);
        Camera.main.GetComponent<CinemachineBrain>().enabled = true;
    }


    void LeaveConversation()
    {
        Debug.Log("leaving conversation from TriggerCollectTickets");
        // FMOD
        // Play dialogue exit sfx
        RuntimeManager.PlayOneShot(dialogueExit);
        if (GameManager.Instance.GetSceneId() == 3)
        {
            Camera.main.GetComponent<CinemachineBrain>().enabled = false;
            // so player cant go through this dialogue again
            if (theaterTracker.isTicketBought()) {
                gameObject.GetComponent<Collider>().enabled = false;
                firstDirector.Stop();
            } else {
                firstDirector.time = 0;
                cameraMove.SetActive(false);
            }

        }
    }

    void OnDisable() {
        firstDirector.paused -= OnFirstDirectorPaused;
        DialogueBoxController.OnDialogueEnded -= LeaveConversation;
    }
}
