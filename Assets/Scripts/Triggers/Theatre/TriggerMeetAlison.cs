using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.Playables;
using FMODUnity;

public class TriggerMeetAlison:MonoBehaviour
{
    public InputReader inputReader;

    // objects with timelines
    public GameObject cameraMove;

    // director that controls cameraMove timeline
    public PlayableDirector firstDirector;

    // SO dialogue asset
    public DialogueAsset dialogue;

    // FMOD References
    [SerializeField] EventReference enterDialogue;
    [SerializeField] EventReference dialogueExit;


    void OnEnable()
    {
        firstDirector.paused += OnFirstDirectorPaused;
    }

    void OnDisable() {
        firstDirector.paused -= OnFirstDirectorPaused;  
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
        // FMOD
        // Play enter dialogue sfx
        RuntimeManager.PlayOneShot(enterDialogue);
        DialogueBoxController.OnDialogueEnded += LeaveConversation;
        Debug.Log("triggering cutscene");
        cameraMove.SetActive(true);
        Camera.main.GetComponent<CinemachineBrain>().enabled = true;
    }


    private void LeaveConversation()
    {
        Debug.Log("leaving conversation");
        // FMOD
        // Play leave convo sfx
        RuntimeManager.PlayOneShot(dialogueExit);
        Camera.main.GetComponent<CinemachineBrain>().enabled = false;


        // so player cant go through this dialogue again
        GetComponent<Collider>().enabled = false;
        
        // make alison follow
        GetComponent<AlisonFollow>().enabled = true;

        // stop timeline animation
        cameraMove.SetActive(false);
        firstDirector.Stop();

        // dont listen anymore to dialogueboxcontroller
        DialogueBoxController.OnDialogueEnded -= LeaveConversation;
    }

}
