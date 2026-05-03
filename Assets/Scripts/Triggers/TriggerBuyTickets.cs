using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.Playables;

public class TriggerBuyTickets : MonoBehaviour
{
    public InputReader inputReader;

    public GameObject TalkPrompt;
    public bool inTalkArea = false;

    // objects with timelines
    public GameObject cameraMove;

    // director that controls cameraMove timeline
    public PlayableDirector firstDirector;

    // SO dialogue asset
    public DialogueAsset dialogue;

    public TrackTheaterEvents theaterTracker;

    void OnEnable()
    {
        firstDirector.paused += OnFirstDirectorPaused;
    }

    void OnFirstDirectorPaused(PlayableDirector aDirector)
    {
        Debug.Log("first director paused");
        if (firstDirector == aDirector)
        {
            DialogueBoxController.instance.StartDialogue(dialogue);
        }
    }

    void Start()
    {
        inputReader.InteractEvent += OnInteract;
    }

    void OnInteract(bool Interacted)
    {
        if (Interacted & inTalkArea)
        {
            // trigger cutscene
            DialogueBoxController.OnDialogueEnded += LeaveConversation;
            Debug.Log("triggering cutscene");
            TalkPrompt.SetActive(false);
            cameraMove.SetActive(true);
            Camera.main.GetComponent<CinemachineBrain>().enabled = true;
        }
    }

    void OnTriggerEnter(Collider player)
    {
        Debug.Log("Player entered buy ticket area");
        TalkPrompt.SetActive(true);
        inTalkArea = true;
    }

    void OnTriggerExit(Collider player)
    {
        Debug.Log("Player left buy ticket area");
        TalkPrompt.SetActive(false);
        inTalkArea = false;
    }

    void LeaveConversation()
    {
        Debug.Log("leaving conversation");
        Camera.main.GetComponent<CinemachineBrain>().enabled = false;
        theaterTracker.buyTicket();
        // so player cant go through this dialogue again
        gameObject.GetComponent<Collider>().enabled = false;
        cameraMove.SetActive(false);
        firstDirector.Stop();
        inTalkArea = false;
    }

    void OnDisable()
    {
        inputReader.InteractEvent -= OnInteract;
        firstDirector.paused -= OnFirstDirectorPaused;
        DialogueBoxController.OnDialogueEnded -= LeaveConversation;
    }
}
