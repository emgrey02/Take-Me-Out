using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.Playables;

public class TriggerPoleDialogue : MonoBehaviour
{
    public InputReader inputReader;

    public WhichPole whichPole;

    public GameObject RodPrompt;
    public bool inRodArea = false;

    // object with timeline
    public GameObject cameraMove;

    // director that controls cameraMove timeline
    public PlayableDirector firstDirector;

    // SO dialogue asset
    public DialogueAsset dialogue;

    public GameObject alison;
    public int alisonRotOffset;
    public GameObject secondCam;

    public Vector3 alisonLoc;

    void OnEnable()
    {
        firstDirector.paused += OnFirstDirectorPaused;
        inputReader.InteractEvent += OnInteract;
    } 

    void Start()
    {
        GetComponent<StartFishing>().enabled = false;
    }

    void OnFirstDirectorPaused(PlayableDirector aDirector)
    {
        Debug.Log("first director paused");
        if (firstDirector == aDirector)
        {
            // begin dialogue
            DialogueBoxController.instance.StartDialogue(dialogue);
        }
    }

    private void OnInteract(bool Interacted)
    {
        if (Interacted & inRodArea)
        {
            whichPole.currentPole = gameObject;
            DialogueBoxController.OnDialogueEnded += LeaveConversation;
            Debug.Log("triggering cutscene");
            RodPrompt.SetActive(false);

            // set alison location
            alison.GetComponent<AlisonFollow>().enabled = false;
            alison.transform.rotation = Quaternion.Euler(new Vector3(0f, -secondCam.transform.eulerAngles.y-alisonRotOffset, 0f));
            alison.transform.position = alisonLoc;

            StopAllCoroutines();

            // start cutscene cameras
            cameraMove.SetActive(true);
            Camera.main.GetComponent<CinemachineBrain>().enabled = true;
        }
    }

    private void OnTriggerEnter(Collider player)
    {
        Debug.Log("Player entered fishing rod area");
        RodPrompt.SetActive(true);
        inRodArea = true;
    }

    private void OnTriggerExit(Collider player)
    {
        Debug.Log("Player left fishing rod area");
        RodPrompt.SetActive(false);
        inRodArea = false;
    }

    //Vector3(68.2454758,1.67139399,98.1349945)
    //Vector3(30.0000114,30.0000076,270)

    //fish
    //Vector3(68.1356354,1.75,98.9417496)

    //prompt
    //Vector3(66.6949997,2.08999991,97.0899963)

    private void LeaveConversation()
    {
        Debug.Log("leaving conversation");

        // shouldnt be necessary but just in case, turn off prompt
        RodPrompt.SetActive(false);

        // turn off virtual camera
        Camera.main.GetComponent<CinemachineBrain>().enabled = false;

        // make alison follow us again
        alison.GetComponent<AlisonFollow>().enabled = true;

        // turn off camera movement timeline
        cameraMove.SetActive(false);

        // stop timeline
        firstDirector.Stop();

        // set this boolean to false
        inRodArea = false;  
        
        // turn collider off
        //GetComponent<Collider>().enabled = false;

        DialogueBoxController.OnDialogueEnded -= LeaveConversation;
    }

    private void OnDisable()
    {
        inputReader.InteractEvent -= OnInteract;
        firstDirector.paused -= OnFirstDirectorPaused;
        DialogueBoxController.OnDialogueEnded -= LeaveConversation;
    }
}
