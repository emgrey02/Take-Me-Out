using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.Playables;

public class TriggerEndConvo:MonoBehaviour
{
    public InputReader inputReader;

    public GameObject alison;

    // objects with timelines
    public GameObject cameraMove;

    // director that controls cameraMove timeline
    public PlayableDirector firstDirector;

    // SO dialogue asset
    public DialogueAsset dialogue;

    public TrackTheaterEvents theaterTracker;

    public GameObject backToBallTrigger;

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

    void OnTriggerEnter(Collider player)
    {
        Debug.Log("Player entered end convo area");
        DialogueBoxController.OnDialogueEnded += LeaveConversation;

        Debug.Log("Setting alison position, disabling Alison Follow component");
        alison.transform.position = new Vector3(-1f, 0f, 6.75f);
        alison.GetComponent<AlisonFollow>().enabled = false;
        alison.GetComponent<TriggerMeetAlison>().enabled = false;

        Debug.Log("triggering cutscene");
        cameraMove.SetActive(true);
        Camera.main.GetComponent<CinemachineBrain>().enabled = true;
    }


    void LeaveConversation()
    {
        Debug.Log("leaving conversation");
        Camera.main.GetComponent<CinemachineBrain>().enabled = false;
        
        // so player cant go through this dialogue again
        gameObject.GetComponent<Collider>().enabled = false;
        firstDirector.Stop();

        // set return to baseball field trigger to active
        backToBallTrigger.SetActive(true);

    }

    void OnDisable() {
        firstDirector.paused -= OnFirstDirectorPaused;
        DialogueBoxController.OnDialogueEnded -= LeaveConversation;
    }
}

