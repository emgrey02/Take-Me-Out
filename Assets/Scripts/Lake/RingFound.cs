using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;

public class RingFound : MonoBehaviour
{
    [SerializeField] InputReader inputReader;

    public VisualElement promptCtn;
    public Button marryBtn;
    public Button walkBtn;

    public DialogueAsset proposalDialogue;
    //public bool promptIsShowing = false;

    // object with timeline
    public GameObject cameraMove;

    // director that controls cameraMove timeline
    public PlayableDirector firstDirector;

    private void OnEnable()
    {
        firstDirector.paused += OnFirstDirectorPaused;
        inputReader.InteractEvent += OnInteract;
    }
    // this script is enabled when the player catches the lake whitefish while fishing
    void Start()
    {
        VisualElement proposalPrompt = GetComponent<UIDocument>().rootVisualElement;
        promptCtn = proposalPrompt.Q<VisualElement>("panel-ctn");
        marryBtn = proposalPrompt.Q<Button>("marry");
        walkBtn = proposalPrompt.Q<Button>("walk");

        marryBtn.clicked += OnMarryBtnClicked;
        walkBtn.clicked += OnWalkBtnClicked;

        inputReader.DisablePlayerControls();
        promptCtn.RemoveFromClassList("remove");
    }

    void OnInteract(bool Interacted)
    {
        if (Interacted & ProposalPromptController.Instance.PromptIsShowing())
        {
            // deactivate proposal prompt
            ProposalPromptController.Instance.DeactivateProposalText();
            // trigger proposal timeline
            cameraMove.SetActive(true);
            Camera.main.GetComponent<CinemachineBrain>().enabled = true;
        }
    }

    void OnFirstDirectorPaused(PlayableDirector aDirector)
    {
        Debug.Log("first director paused");
        if (firstDirector == aDirector)
        {
            // start dialogue
            DialogueBoxController.OnDialogueEnded += LeaveConversation;
            DialogueBoxController.instance.StartDialogue(proposalDialogue);
        }
    }
    private void OnMarryBtnClicked()
    {
        Debug.Log("Marry button clicked");
        promptCtn.AddToClassList("remove");
       
        // trigger proposal timeline
        cameraMove.SetActive(true);
        Camera.main.GetComponent<CinemachineBrain>().enabled = true;
    }

    private void OnWalkBtnClicked()
    {
        // keep walking
        inputReader.EnablePlayerControls();
        promptCtn.AddToClassList("remove");
        ProposalPromptController.Instance.TriggerProposalPrompt();
    }

    void OnDisable()
    {
        DialogueBoxController.OnDialogueEnded -= LeaveConversation;
        firstDirector.paused -= OnFirstDirectorPaused;
    }

    private void LeaveConversation()
    {

        GameManager.Instance.MoveToScene(1);
        // stop proposal timeline
        //cameraMove.SetActive(false);
        //Camera.main.GetComponent<CinemachineBrain>().enabled = false;
    }
}
