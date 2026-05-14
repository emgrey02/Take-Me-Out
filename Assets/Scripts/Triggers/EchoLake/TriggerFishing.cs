using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.Playables;

public class TriggerFishing : MonoBehaviour
{
    public InputReader inputReader;

    public GameObject RodPrompt;
    public bool inRodArea = false;

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
        if (Interacted & inRodArea)
        {
            // trigger cutscene
            DialogueBoxController.OnDialogueEnded += LeaveConversation;
            Debug.Log("triggering cutscene");
            RodPrompt.SetActive(false);
            cameraMove.SetActive(true);
            Camera.main.GetComponent<CinemachineBrain>().enabled = true;
        }
    }

    void OnTriggerEnter(Collider player)
    {
        Debug.Log("Player entered fishing rod area");
        RodPrompt.SetActive(true);
        inRodArea = true;
    }

    void OnTriggerExit(Collider player)
    {
        Debug.Log("Player left fishing rod area");
        RodPrompt.SetActive(false);
        inRodArea = false;
    }

    void LeaveConversation()
    {
        Debug.Log("leaving conversation");
        Camera.main.GetComponent<CinemachineBrain>().enabled = false;

        cameraMove.SetActive(false);
        firstDirector.Stop();
        inRodArea = false;
    }

    void OnDisable()
    {
        inputReader.InteractEvent -= OnInteract;
        firstDirector.paused -= OnFirstDirectorPaused;
        DialogueBoxController.OnDialogueEnded -= LeaveConversation;
    }
}
