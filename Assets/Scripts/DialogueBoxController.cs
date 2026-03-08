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

    // typewriter effect
    float charactersPerSecond = 60;

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

        // get ui elements
        dialogueBox = GetComponent<UIDocument>().rootVisualElement;
        speakerName = dialogueBox.Q<Label>("speakerName");
        dialogueText = dialogueBox.Q<Label>("dialogueText");
        nextButton = dialogueBox.Q<Button>("nextLine");

        dialogueBox.AddToClassList("hide");
    }

    void OnEnable()
    {
        nextButton.clicked += OnNextButtonClicked;
    }

    void OnDisable()
    {
        nextButton.clicked -= OnNextButtonClicked;
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
            StartCoroutine(TypeText(dialogue[i]));
           
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

    IEnumerator TypeText(string line)
    {
        float timer = 0;
        float interval = 1 / charactersPerSecond;
        string textBuffer = null;
        char[] chars = line.ToCharArray();
        int i = 0;

        while (i < chars.Length)
        {
            if (timer < Time.deltaTime)
            {
                textBuffer += chars[i];
                dialogueText.text = textBuffer;
                timer += interval;
                i++;
            }
            else
            {
                timer -= Time.deltaTime;
                yield return null;
            }
        }
    }

    private void OnNextButtonClicked()
    {
        nextLineTriggered = true;
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
