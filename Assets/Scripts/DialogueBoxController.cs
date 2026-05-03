using UnityEngine;
using UnityEngine.UIElements; 
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum Speakers {
    Alison,
    Brad,
    Joe,
    Mia,
    You,
    Branch
};

public class DialogueBoxController : MonoBehaviour
{
    public static DialogueBoxController instance;

    [SerializeField] InputReader inputReader;

    public VisualElement dialogueBox;
    public VisualElement optionsPanel;
    public Label speakerName;
    public Label dialogueText;
    public Button nextButton;
    public List<Button> options = new List<Button>();
    public DialogueAsset currentDialogue;


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
        
        optionsPanel = dialogueBox.Q<VisualElement>("option-container");

        options = dialogueBox.Query<Button>(className: "option").ToList();

        nextButton = dialogueBox.Q<Button>("nextLine");

        dialogueBox.AddToClassList("hide");
    }

    void OnEnable()
    {
        nextButton.clicked += OnNextButtonClicked;
        for (int i=0; i < options.Count; i++) {
            //options[i].clicked += OnOptionClicked;
            options[i].RegisterCallback<ClickEvent>(OnOptionClicked);
        }
    }

    void OnDisable()
    {
        nextButton.clicked -= OnNextButtonClicked;
        for (int i=0; i < options.Count; i++) {
            options[i].UnregisterCallback<ClickEvent>(OnOptionClicked);
        }
    }

    public void StartDialogue(DialogueAsset d)
    {
        Debug.Log("starting dialogue with asset ");
        Debug.Log(d.dialogue[0]);
        optionsPanel.visible = false;
        currentDialogue = d;
        inputReader.DisablePlayerControls();
        dialogueBox.RemoveFromClassList("hide");
        StopAllCoroutines();
        StartCoroutine(RunDialogue(d));
    }

    public void EndDialogue()
    {
        Debug.Log("reseting dialogue ui");
        inputReader.EnablePlayerControls();
        dialogueBox.AddToClassList("hide");
        currentDialogue = null;
        speakerName.text = null;
        dialogueText.text = null;
    }

    IEnumerator RunDialogue(DialogueAsset d)
    {
        nextLineTriggered = false;

        for(int i = 0; i < d.speaker.Length; i++)
        {
            Debug.Log("going through speaker list");
            Debug.Log("step "+ i+ ": "+ d.speaker[i].ToString());
            

            // if there's a branch
            if (d.speaker[i] == Speakers.Branch) {
                Debug.Log("It's a branch!'");
                // time to show reply options
                //optionsPanel.visible = true;
                nextButton.AddToClassList("hide");

                for (int j=0; j < options.Count; j++) {
                    if (d.options.Length > j) {
                        options[j].visible = true;
                        options[j].text = d.options[j];
                    } else {
                        options[j].visible = false;
                    }
                }

            } else {
                Debug.Log("not a branch");
                Debug.Log("setting text");
                optionsPanel.visible = false;
                for (int j=0; j < options.Count; j++) {
                    options[j].visible = false; 
                }
                nextButton.RemoveFromClassList("hide");
                speakerName.text = d.speaker[i].ToString();
                StartCoroutine(TypeText(d.dialogue[i]));
                //dialogueText.text = d.dialogue[i];
            }

            if (i < d.dialogue.Length) {
                    dialogueText.text = d.dialogue[i];
            }
           
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
        Debug.Log("next button clicked");
        nextLineTriggered = true;
    }

    private void OnOptionClicked(ClickEvent evt) 
    {
        Debug.Log("option clicked");
        for (int i=0; i < options.Count; i++) {
            if (evt.target == options[i]) {
                switch (i) 
                {
                    case 0:
                        StartDialogue(currentDialogue.option1);
                        break;
                    case 1:
                        StartDialogue(currentDialogue.option2);
                        break;
                    case 2:
                        StartDialogue(currentDialogue.option3);
                        break;
                    case 3:
                        StartDialogue(currentDialogue.option4);
                        break;
                    default:
                        break;
                }
                        
            }
        }
    }
}
