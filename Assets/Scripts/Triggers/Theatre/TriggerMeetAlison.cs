using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.Playables;

public class TriggerMeetAlison:MonoBehaviour
{
    public InputReader inputReader;

    // objects with timelines
    public GameObject cameraMove;

    // director that controls cameraMove timeline
    public PlayableDirector firstDirector;

    // SO dialogue asset
    public DialogueAsset dialogue;


    void OnEnable()
    {
        firstDirector.paused += OnFirstDirectorPaused;
    }

    void OnDisable() {
        firstDirector.paused -= OnFirstDirectorPaused;
        DialogueBoxController.OnDialogueEnded -= LeaveConversation;
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
        Debug.Log("Player entered alison area");
        DialogueBoxController.OnDialogueEnded += LeaveConversation;
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

        GetComponent<AlisonFollow>().enabled = true;
        GetComponent<Collider>().enabled = false;
        cameraMove.SetActive(false);
        firstDirector.Stop();
    }

}
