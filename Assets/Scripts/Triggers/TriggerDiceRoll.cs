using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Playables;

public class TriggerDiceRoll : MonoBehaviour
{
    public InputReader inputReader;

    public GameObject DiceRollPrompt;
    public bool inTableArea = false;

    public GameObject tableCam;

    // objects with timelines
    public GameObject cameraMove;
    public GameObject dice;
    private Animator diceRoll;

    // director that controls cameraMove timeline
    public PlayableDirector firstDirector;

    // director that controls diceRoll timeline
    //public PlayableDirector secondDirector;
    public GameObject SuccessText;

    void OnEnable()
    {
        firstDirector.paused += OnFirstDirectorPaused;
        //secondDirector.stopped += OnSecondDirectorStopped;
    }

    void OnFirstDirectorPaused(PlayableDirector aDirector)
    {
        if (firstDirector == aDirector)
        {
            //start dice roll animation
            diceRoll.SetBool("roll", true);
            //isRolling = true;
        }
    }

    void Start()
    {
        inputReader.InteractEvent += OnInteract;
        diceRoll = dice.GetComponent<Animator>();
    }

    void OnInteract(bool Interacted)
    {
        if (Interacted & inTableArea)
        {
            // trigger cutscene
            //DiceRollPrompt.SetActive(false);
            //cameraMove.SetActive(true);
            //Camera.main.GetComponent<CinemachineBrain>().enabled = true;
            cameraMove.SetActive(true);
            inputReader.DisablePlayerControls();
            DiceRollPrompt.SetActive(false);
            
        }
    }

    void OnTriggerEnter(Collider player)
    {
        Debug.Log("Player entered area around table");
        DiceRollPrompt.SetActive(true);
        inTableArea = true;
    }

    void OnTriggerExit(Collider player)
    {
        Debug.Log("Player left area around table");
        DiceRollPrompt.SetActive(false);
        inTableArea = false;
    }

    void OnDisable()
    {
        inputReader.InteractEvent -= OnInteract;
        firstDirector.paused -= OnFirstDirectorPaused;
    }

    void Update()
    {

        if (diceRoll.GetCurrentAnimatorStateInfo(0).IsName("dice-fly") && diceRoll.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            // dice roll animation finished
            Debug.Log("Dice roll animation finished");
            SuccessText.SetActive(true);
        }

        if (diceRoll.GetCurrentAnimatorStateInfo(0).IsName("dice-fly") && diceRoll.GetCurrentAnimatorStateInfo(0).normalizedTime >= 4.0f)
        {
            inputReader.EnablePlayerControls();
            GameManager.Instance.MoveToScene(6);
        }
    }
}

