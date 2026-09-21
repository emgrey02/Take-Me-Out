using UnityEngine;
using UnityEngine.UIElements; 
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FMODUnity;
using FMOD.Studio;

public class TheaterScreenController : MonoBehaviour
{

    [SerializeField] InputReader inputReader;

    // ui elements
    public VisualElement theaterScreen;
    public Label question;
    public List<Button> options = new List<Button>();
    
    public Label answer;
    public Label info;
    public Image img;
    public Button nextBtn;

    public VisualElement questionCtn;
    public VisualElement answerCtn;
    public VisualElement finalScreenCtn;

    public TriviaQAsset currentQuestion;
    public TriviaAAsset currentAnswer;

    public static event Action OnTriviaOver;

    // FMOD References
    [SerializeField] EventReference triviaMusic;
    [SerializeField] EventReference optionHover;
    [SerializeField] EventReference optionSelect;
    [SerializeField] EventReference continueText;
    public EventInstance musicInstance;
    public int musicFinished = 0;
    public string musicParameter;


    void Awake()
    {
        // get ui elements
        theaterScreen = GetComponent<UIDocument>().rootVisualElement;

        questionCtn = theaterScreen.Q<VisualElement>("question-ctn");
        answerCtn = theaterScreen.Q<VisualElement>("answer-ctn");
        finalScreenCtn = theaterScreen.Q<VisualElement>("finalscreen-ctn");

        question = theaterScreen.Q<Label>("question");
        options = theaterScreen.Query<Button>(className: "button-answer").ToList();

        answer = theaterScreen.Q<Label>("answer");
        img = theaterScreen.Q<Image>("image");
        info = theaterScreen.Q<Label>("info-text");
        nextBtn = theaterScreen.Q<Button>(className: "button");

        questionCtn.AddToClassList("remove");
        answerCtn.AddToClassList("remove");
        finalScreenCtn.AddToClassList("remove");

        musicInstance = RuntimeManager.CreateInstance(triviaMusic);

    }

    void OnEnable()
    {
        for (int i=0; i < options.Count; i++) {
            options[i].RegisterCallback<ClickEvent>(OnOptionClicked);
            options[i].RegisterCallback<MouseOverEvent>(OnOptionHover);
        }
    }

    void OnDisable()
    {
        for (int i=0; i < options.Count; i++) {
            options[i].UnregisterCallback<ClickEvent>(OnOptionClicked);
            options[i].UnregisterCallback<MouseOverEvent>(OnOptionHover);
        }
    }

    public void StartTrivia(TriviaQAsset t)
    {
        DisplayQuestion(t);
        // FMOD
        // Start music
        musicInstance.start();
    }

    public void EndTrivia()
    {
        Debug.Log("trivia ended");
        // FMOD
        // Stop music
        RuntimeManager.StudioSystem.setParameterByName(musicParameter, 1f, false);
        questionCtn.AddToClassList("remove");
        answerCtn.AddToClassList("remove");
        finalScreenCtn.RemoveFromClassList("remove");
    }

    public void DisplayQuestion(TriviaQAsset t)
    {
        questionCtn.RemoveFromClassList("remove");
        answerCtn.AddToClassList("remove");

        currentQuestion = t;
        // display question
        question.text = t.question;

        // display answer options
        for (int j=0; j < options.Count; j++) {
            if (t.answers.Length > j) {
                options[j].visible = true;
                options[j].text = t.answers[j];
            } else {
                options[j].visible = false;
            }
        }
    }

    public void DisplayAnswer(TriviaAAsset t)
    {
        questionCtn.AddToClassList("remove");
        answerCtn.RemoveFromClassList("remove");

        currentAnswer = t;

        // display answer
        answer.text = t.answer;

        // display image
        img.sprite = Resources.Load<Sprite>(t.image);

        // display info text
        info.text = t.info;

        // listen for button click
        nextBtn.RegisterCallback<ClickEvent>(OnButtonClicked);

        // display correct button text
        if (t.correct && !currentAnswer.next) {
            nextBtn.text = "Finish";
        } else if (t.correct) 
        {
            nextBtn.text = "Next Question";
        } else {
            nextBtn.text = "Try Again";
        }
    }
    

    private void OnButtonClicked(ClickEvent evt)
    {
        // FMOD
        RuntimeManager.PlayOneShot(continueText);
        if (currentAnswer.next == null) {
            EndTrivia();
            OnTriviaOver?.Invoke();
        } else {
            DisplayQuestion(currentAnswer.next);
        }
    }

    private void OnOptionHover(MouseOverEvent evt)
    {
        // FMOD
        RuntimeManager.PlayOneShot(optionHover);
    }

    private void OnOptionClicked(ClickEvent evt) 
    {
        Debug.Log("option clicked");
        // FMOD
        RuntimeManager.PlayOneShot(optionSelect);
        for (int i=0; i < options.Count; i++) {
            if (evt.target == options[i]) {
                switch (i) 
                {
                    case 0:
                        DisplayAnswer(currentQuestion.answer1);
                        break;
                    case 1:
                        DisplayAnswer(currentQuestion.answer2);
                        break;
                    case 2:
                        DisplayAnswer(currentQuestion.answer3);
                        break;
                    case 3:
                        DisplayAnswer(currentQuestion.answer4);
                        break;
                    default:
                        break;
                }
                        
            }
        }
    }
}
