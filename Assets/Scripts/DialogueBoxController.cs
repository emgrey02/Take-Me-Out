using UnityEngine;
using UnityEngine.UIElements; 
using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine.EventSystems;

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

    public PanelSettings PanelSettings;

    [SerializeField] InputReader inputReader;

    private Focusable currentFocus;

    // FMOD
    // events needed for dialogue
    [SerializeField] EventReference dialogueContinue;
    [SerializeField] EventReference optionHover;

    public VisualElement box;
    public VisualElement alisonBox;
    public VisualElement innerBox;

    public VisualElement optionsPanel;
    public Label speakerName;
    public Label dialogueText;
    public Image lightning;
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
        alisonBox = dialogueBox.Q<VisualElement>("AlisonBox");
        innerBox = dialogueBox.Q<VisualElement>("inner-box");
        box.AddToClassList("hide");
        alisonBox.AddToClassList("hide");
        innerBox.AddToClassList("hide");
   
        speakerName = dialogueBox.Q<Label>("speakerName");
        dialogueText = dialogueBox.Q<Label>("dialogueText");
        lightning = dialogueBox.Q<Image>("lightning-bar");
        
        optionsPanel = dialogueBox.Q<VisualElement>("option-container");

        options = dialogueBox.Query<Button>(className: "option").ToList();
        
        optionsPanel.visible = false;
        lightning.AddToClassList("hide");

        nextButton = dialogueBox.Q<Button>("nextLine");


    }


    void OnEnable()
    {
        nextButton.clicked += OnNextButtonClicked;
        for (int i=0; i < options.Count; i++) {
            //options[i].clicked += OnOptionClicked;
            options[i].RegisterCallback<NavigationSubmitEvent>(OnOptionClicked);
            options[i].RegisterCallback<ClickEvent>(OnOptionClicked);
            options[i].RegisterCallback<MouseEnterEvent>(OnOptionHover);
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
        lightning.AddToClassList("hide");
        
        HideOptions();
    }

    void Update()
    {
        currentFocus = GetComponent<UIDocument>().rootVisualElement.focusController.focusedElement;
        VisualElement currentFocusElement = currentFocus as VisualElement;
        Button focusedButton;
        if (currentFocus != null & currentFocus is Button)
        {
            focusedButton = currentFocus as Button;
            Debug.Log(focusedButton.name);
        } else
        {
            Debug.Log(currentFocus);
        }
    }

    private void HideOptions()
    {
        optionsPanel.visible = false;
        for (int j = 0; j < options.Count; j++)
        {
            options[j].text = null;
            options[j].visible = false;
        }
        lightning.AddToClassList("hide");
    }

    public void StartDialogue(DialogueAsset d)
    {
        Debug.Log(d.name);
        Debug.Log("starting dialogue with asset");

        currentDialogue = d;

        ClearDialogueBox();
        
        inputReader.DisablePlayerControls();

        box.RemoveFromClassList("hide");
        innerBox.RemoveFromClassList("hide");
        
        StopAllCoroutines();
        StartCoroutine(RunDialogue(d));
    }

    public void ContinueDialogue(DialogueAsset d)
    {
        RuntimeManager.PlayOneShot(dialogueContinue);
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
        alisonBox.AddToClassList("hide");
        innerBox.AddToClassList("hide");

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
                nextButton.style.opacity = 0;
                nextButton.tabIndex = -1;
                nextButton.SetEnabled(false);

                // show lighting bar
                lightning.RemoveFromClassList("hide");

                // show answer options
                optionsPanel.visible = true;
                for (int j=0; j < options.Count; j++) {
                    Debug.Log(options[j]);
                    if (d.options.Length > j) {
                        Debug.Log("showing option");
                        options[j].visible = true;
                        options[j].text = d.options[j];
                    } else {
                        Debug.Log("hiding option");
                        options[j].visible = false;
                    }
                }

                options[0].Focus();

                if (currentFocus == null || currentFocus is not Button)
                {
                    Debug.Log("currentFocus is null or not button, focusing on first option");
                    options[0].Focus();
                }


            } else {
                Debug.Log("not a branch");
                Debug.Log("setting text");

                // show next button
                nextButton.style.opacity = 1;
                nextButton.tabIndex = 0;
                nextButton.SetEnabled(true);

                // hide options
                HideOptions();

                // have next button selected
                Debug.Log("focusing on next btn");
                nextButton.Focus();

                // set speaker name
                speakerName.text = d.speaker[i].ToString();

                // if it's alison speaking, show her box, otherwise hide it
                if (d.speaker[i] == Speakers.Alison)
                {
                    alisonBox.RemoveFromClassList("hide");
                    box.AddToClassList("hide");
                }
                else
                {
                    alisonBox.AddToClassList("hide");
                    box.RemoveFromClassList("hide");
                }

                dialogueText.text = d.dialogue[i];

                if (currentFocus == null || currentFocus is not Button)
                {
                    Debug.Log("currentFocus is null or not button, focusing on next btn");
                    nextButton.Focus();
                }

                nextButton.Focus();

                //start typing text
                //StartCoroutine(TypeText(d.dialogue[i]));

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
        RuntimeManager.PlayOneShot(dialogueContinue);
    }
    private void OnOptionClicked(ClickEvent evt)
    {
        HandleOption(evt.target);
    }
    private void OnOptionClicked(NavigationSubmitEvent evt) 
    {
        HandleOption(evt.target);
        
    }
    private void OnOptionHover(MouseEnterEvent evt)
    {
        RuntimeManager.PlayOneShot(optionHover);
    }
    private void HandleOption(IEventHandler target)
    {
        Debug.Log("option clicked");
        for (int i = 0; i < options.Count; i++)
        {
            if (target == options[i])
            {

                ClearDialogueBox();

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
