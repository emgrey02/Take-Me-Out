using UnityEngine;
using UnityEngine.UIElements; 
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum Speakers {
    Alison,
    Brad,
    Michols,
    Mia,
    You,
    Branch
};

public class DialogueBoxController : MonoBehaviour
{
    public static DialogueBoxController instance;

    [SerializeField] InputReader inputReader;

    public VisualElement box;
    public VisualElement optionsPanel;
    public Label speakerName;
    public Label dialogueText;
    public Button nextButton;
    public List<Button> options = new List<Button>();
    public DialogueAsset currentDialogue;

    public WhichPole whichPole;

    public RingFound ringFound;

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
        VisualElement dialogueBox = GetComponent<UIDocument>().rootVisualElement;

        box = dialogueBox.Q<VisualElement>("Box");
        box.AddToClassList("hide");

        speakerName = dialogueBox.Q<Label>("speakerName");
        dialogueText = dialogueBox.Q<Label>("dialogueText");
        
        optionsPanel = dialogueBox.Q<VisualElement>("option-container");

        options = dialogueBox.Query<Button>(className: "option").ToList();

        nextButton = dialogueBox.Q<Button>("nextLine");

    }


    void OnEnable()
    {
        nextButton.clicked += OnNextButtonClicked;
        for (int i=0; i < options.Count; i++) {
            //options[i].clicked += OnOptionClicked;
            options[i].RegisterCallback<NavigationSubmitEvent>(OnOptionClicked);
            options[i].RegisterCallback<ClickEvent>(OnOptionClicked);
        }
    }

    void OnDisable()
    {
        nextButton.clicked -= OnNextButtonClicked;
        for (int i=0; i < options.Count; i++) {
            options[i].UnregisterCallback<NavigationSubmitEvent>(OnOptionClicked);
            options[i].UnregisterCallback<ClickEvent>(OnOptionClicked);
        }
    }

    private void ClearDialogueBox()
    {
        // hide all dialogue box text
        speakerName.text = null;
        dialogueText.text = null;
        for (int j=0; j < options.Count; j++) {
            options[j].text = null;
            options[j].visible = false; 
        }
        optionsPanel.visible = false;
    }

    public void StartDialogue(DialogueAsset d)
    {
        Debug.Log(d.name);
        Debug.Log("starting dialogue with asset");

        currentDialogue = d;

        ClearDialogueBox();
        
        inputReader.DisablePlayerControls();

        box.RemoveFromClassList("hide");
        
        StopAllCoroutines();
        StartCoroutine(RunDialogue(d));
    }

    public void ContinueDialogue(DialogueAsset d)
    {
        Debug.Log(d.name);
        Debug.Log("continuing dialogue");
        ClearDialogueBox();
        currentDialogue = d;

        StopAllCoroutines();
        StartCoroutine(RunDialogue(d));
    }

    public void EndDialogue()
    {
        Debug.Log("reseting dialogue ui");
        inputReader.EnablePlayerControls();
        box.AddToClassList("hide");

        if (currentDialogue.name == "lets-fish") {
            // start fishing script
            // swap scripts
            whichPole.currentPole.GetComponent<TriggerPoleDialogue>().enabled = false;
            StartFishing script = whichPole.currentPole.GetComponent<StartFishing>();
            script.enabled = true;
            script.PrepareToFish();
        }

        if (currentDialogue.name == "LakeWhitefish") 
        {
            StartFishing script = whichPole.currentPole.GetComponent<StartFishing>();
            Debug.Log("enable script to begin engagement ring dialogue");
            script.StopFishing();
            ringFound.enabled = true;
        }
 
        //ClearDialogueBox();
    }

    IEnumerator RunDialogue(DialogueAsset d)
    {
        nextLineTriggered = false;

        for(int i = 0; i < d.speaker.Length; i++)
        {
            Debug.Log("going through speaker list");
            
            // if there's a branch
            if (d.speaker[i] == Speakers.Branch) {
                Debug.Log("It's a branch!");

                // time to show reply options
                nextButton.AddToClassList("hide");

                for (int j=0; j < options.Count; j++) {
                    if (d.options.Length > j) {
                        options[j].visible = true;
                        options[j].text = d.options[j];
                    } else {
                        options[j].visible = false;
                    }
                }

                // have first option selected
                options[0].Focus();

            } else {
                Debug.Log("not a branch");
                Debug.Log("setting text");

                // hide options
                optionsPanel.visible = false;
                for (int j=0; j < options.Count; j++) {
                    options[j].visible = false; 
                }

                // show next button
                nextButton.RemoveFromClassList("hide");

                // set speaker name
                speakerName.text = d.speaker[i].ToString();

                dialogueText.text = d.dialogue[i];

                // start typing text
                //StartCoroutine(TypeText(d.dialogue[i]));

                // have next button selected
                nextButton.Focus();

            }

            if (i < d.dialogue.Length) {
                // keep dialogue text when showing options
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
        HandleOption(evt.target);
    }
    private void OnOptionClicked(NavigationSubmitEvent evt) 
    {
        HandleOption(evt.target);
        
    }
    private void HandleOption(IEventHandler target)
    {
        Debug.Log("option clicked");
        for (int i = 0; i < options.Count; i++)
        {
            if (target == options[i])
            {
                // hide all dialogue box text
                speakerName.text = null;
                dialogueText.text = null;
                optionsPanel.visible = false;
                for (int j = 0; j < options.Count; j++)
                {
                    options[j].text = null;
                    options[j].visible = false;
                }

                // send to next dialogue based on option
                switch (i)
                {
                    case 0:
                        ContinueDialogue(currentDialogue.option1);
                        break;
                    case 1:
                        ContinueDialogue(currentDialogue.option2);
                        break;
                    case 2:
                        ContinueDialogue(currentDialogue.option3);
                        break;
                    case 3:
                        ContinueDialogue(currentDialogue.option4);
                        break;
                    default:
                        break;
                }

            }
        }
    }
}
