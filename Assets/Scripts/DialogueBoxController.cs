using UnityEngine;
using UnityEngine.UIElements; 
using System;
using System.Collections;

public class DialogueBoxController : MonoBehaviour
{
    public static DialogueBoxController instance;

    [SerializeField] InputReader inputReader;

    public VisualElement dialogueBox;
    public Label speakerName;
    public Label dialogueText;
    public Button nextButton;

    bool nextLineTriggered = false;

    public static event Action OnDialogueEnded;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else {
            Destroy(this);
        }

        dialogueBox = GetComponent<UIDocument>().rootVisualElement;
        speakerName = dialogueBox.Q<Label>("speakerName");
        dialogueText = dialogueBox.Q<Label>("dialogueText");
        nextButton = dialogueBox.Q<Button>("nextLine");

        dialogueBox.AddToClassList("hide");
    }

    void OnEnable()
    {
        nextButton.clicked += OnNextButtonClicked;
        inputReader.InteractEvent += OnInteract;
    }

    public void StartDialogue(string[] dialogue, string name)
    {
        Debug.Log("starting dialogue");
        inputReader.DisablePlayerControls();
        speakerName.text = name;
        dialogueBox.RemoveFromClassList("hide");
        StopAllCoroutines();
        StartCoroutine(RunDialogue(dialogue, 0));
    }

    public void EndDialogue()
    {
        inputReader.EnablePlayerControls();
        dialogueBox.AddToClassList("hide");
        speakerName.text = null;
        dialogueText.text = null;
    }

    IEnumerator RunDialogue(string[] dialogue, int startPosition)
    {
        nextLineTriggered = false;

        for(int i = startPosition; i < dialogue.Length; i++)
        {
            dialogueText.text = dialogue[i];
            while (nextLineTriggered == false)
            {
                // Wait for the current line to be skipped
                yield return null;
            }
            nextLineTriggered = false;
        }

        OnDialogueEnded?.Invoke();
        EndDialogue();
    }

    private void OnNextButtonClicked()
    {
        nextLineTriggered = true;
    }

    private void OnInteract(bool Interacted)
    {
        if (Interacted)
        {
            nextLineTriggered = true;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
