using UnityEngine;
using UnityEngine.UIElements; 
using Unity.Cinemachine;
using UnityEngine.Playables;
using System.Collections;

public class TriggerEnterTheater : MonoBehaviour
{
    public InputReader inputReader;

    public GameObject EnterPrompt;
    public bool inEnterArea = false;

    // objects with timelines
    public GameObject cameraMove;

    // director that controls cameraMove timeline
    public PlayableDirector firstDirector;

    //public TrackTheaterEvents theaterTracker;
    public GameObject Screen;
    public VisualElement theaterScreen;

    public TheaterScreenController triviaController;

    public TriviaQAsset firstQuestion;

    private GameObject player;

    public GameObject panelToBlack;
    public GameObject panelToClear;

    public GameObject endConvoTrigger;
    public GameObject otherTrigger;

    void OnEnable()
    {
        firstDirector.paused += OnFirstDirectorPaused;
        TheaterScreenController.OnTriviaOver += EndTrivia;
    }

    void Awake() {
        theaterScreen = Screen.GetComponent<UIDocument>().rootVisualElement;
        theaterScreen.visible = false;
    }

    void OnFirstDirectorPaused(PlayableDirector aDirector)
    {
        Debug.Log("first director paused");
        // so player cant go through this again for both theater entrances
        GetComponent<Collider>().enabled = false;
        otherTrigger.GetComponent<Collider>().enabled = false;
        // trigger trivia game
        theaterScreen.visible = true;
        inputReader.DisablePlayerControls();
        triviaController.StartTrivia(firstQuestion);
    }

    void Start()
    {
        inputReader.InteractEvent += OnInteract;
    }

    void OnInteract(bool Interacted)
    {
        if (Interacted & inEnterArea)
        {
            // trigger cutscene
            Debug.Log("triggering cutscene");
            EnterPrompt.SetActive(false);
            cameraMove.SetActive(true);
            Camera.main.GetComponent<CinemachineBrain>().enabled = true;
            inEnterArea = false;
        }
    }

    void OnTriggerEnter(Collider player)
    {
        Debug.Log("Player entered theater entrance area");
        EnterPrompt.SetActive(true);
        inEnterArea = true;
    }

    void OnTriggerExit(Collider player)
    {
        Debug.Log("Player left theater entrance area");
        EnterPrompt.SetActive(false);
        inEnterArea = false;
    }

    void EndTrivia()
    {
        // fade-out-in
        StartCoroutine("FadeOut");
    }

    IEnumerator FadeOut()
    {
        panelToBlack.SetActive(true);

        // movie takes time, keep black screen up for a bit
        yield return new WaitForSeconds(12);

        // set camera back to player view
        Camera.main.GetComponent<CinemachineBrain>().enabled = false;

        // remove theaterScreen
        theaterScreen.style.display = DisplayStyle.None;
        
        // set player location
        player = GameObject.FindWithTag("Player");
        player.transform.position = new Vector3(-5.6f, -.5f, 27f);
        Physics.SyncTransforms();

        // Fade in now
        panelToClear.SetActive(true);
        panelToBlack.SetActive(false);

        StartCoroutine("FadeIn");
    }

    IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(4);
        inputReader.EnablePlayerControls();
        panelToClear.SetActive(false);
        endConvoTrigger.SetActive(true);
        
    }


    void OnDisable()
    {
        inputReader.InteractEvent -= OnInteract;
        firstDirector.paused -= OnFirstDirectorPaused;
        TheaterScreenController.OnTriviaOver -= EndTrivia;
    }
}

