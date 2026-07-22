using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.Playables;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class StartFishing : MonoBehaviour
{
    [SerializeField] InputReader inputReader;

    // object with timeline
    public GameObject cameraMove;

    // director that controls cameraMove timeline
    public PlayableDirector firstDirector;

    // fishing slider game
    private bool increasing = true;
    public bool sliderWithinBar;
    private bool fishCaught = false;
    public int sliderSpeed = 100;

    public VisualElement fishingSliderUI;
    public GameObject fishingSliderObj;
    public Slider fishingSlider;
    public VisualElement bkgd;
    private FishAsset chosenFish;

    private VisualElement currentBar;
    public VisualElement hardBar;
    public VisualElement mediumBar;
    public VisualElement easyBar;

    public Vector3 sliderPos;
    public Vector3 sliderRot;

    // fishing prompt
    public bool promptIsShowing;
    public GameObject fishingPrompt;

    // animations
    private Animator poleAnim;
    private Animator textAnim;

    public bool poleUp;

    public FishAsset[] fish = new FishAsset[4];
    public List<FishAsset> fishPool = new();

    private GameObject instantiatedFish;

    public Vector3 fishPos;
    private bool isFishing;

    public WhichPole whichPole;

    void OnEnable()
    {
        firstDirector.paused += OnFirstDirectorPaused;
    }

    void OnDisable()
    {
        Debug.Log("Disabling StartFishing script");
        firstDirector.paused -= OnFirstDirectorPaused;
        DialogueBoxController.OnDialogueEnded -= LeaveConversation;
        inputReader.InteractEvent -= OnInteract;
        inputReader.LeaveEvent -= OnLeave;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // get fishing slider game ui elements
        fishingSliderUI = fishingSliderObj.GetComponent<UIDocument>().rootVisualElement;
        fishingSlider = fishingSliderUI.Q<Slider>("fishing-game-slider");
        bkgd = fishingSliderUI.Q<VisualElement>("bkgd");
        hardBar = fishingSliderUI.Q<VisualElement>("hardbar");
        mediumBar = fishingSliderUI.Q<VisualElement>("mediumbar");
        easyBar = fishingSliderUI.Q<VisualElement>("easybar");

        // create fish pool based on weights
        for (int i=0; i < fish.Length; i++) {
            Debug.Log(fish[i].weight);
            int weight = fish[i].weight;

            for (int j=0; j < weight; j++) {
                Debug.Log("adding fish to fish pool");
                fishPool.Add(fish[i]);
            }
        }
    }

    // DialogueBoxController calls this when the user selects that they want to fish
    public void PrepareToFish()
    {
        inputReader.InteractEvent += OnInteract;
        inputReader.LeaveEvent += OnLeave;

        // make sure proposal prompt is not showing
        Debug.Log("deactivating proposal prompt");
        ProposalPromptController.Instance.DeactivateProposalText();

        // start cutscene cameras
        cameraMove.SetActive(true);
        Camera.main.GetComponent<CinemachineBrain>().enabled = true;
        
        isFishing = false;
    }

    // when camera moves to fishing position, show prompt and move pole up
    void OnFirstDirectorPaused(PlayableDirector aDirector)
    {
        Debug.Log("first director paused");
        if (firstDirector == aDirector)
        {
            // show fish prompt
            textAnim = fishingPrompt.GetComponent<Animator>();
            textAnim.SetBool("hide", false);
            promptIsShowing = true;

            // trigger pole animation
            poleAnim = gameObject.GetComponent<Animator>();
            poleAnim.SetBool("poleUp", true);
            poleUp = true;

        }
    }

    void OnInteract(bool Interacted)
    {
        // use presses E to interact/ fish when prompt is showing
        if (Interacted & promptIsShowing)
        {
            HidePrompt();
            
            // start fishing
            fishCaught = false;

            // show slider ui
            fishingSliderObj.transform.position = sliderPos;
            fishingSliderObj.transform.rotation = Quaternion.Euler(sliderRot);
            fishingSlider.RemoveFromClassList("remove");
            bkgd.RemoveFromClassList("remove");
            
            Fish();
        }

        // during fishing, if user presses E when slider is within bar, they catch the fish
        if (Interacted & sliderWithinBar)
        {
            Debug.Log("caught the fish!");
            fishCaught = true;

            // instantiate fish prefab
            GameObject fish = chosenFish.fishPrefab;
            instantiatedFish = Instantiate(fish, fishPos, fish.transform.rotation);

            // start dialogue
            Debug.Log("Subscribing to DialogueBoxController OnDialogueEnded event in StartFishing");
            DialogueBoxController.OnDialogueEnded += LeaveConversation;
            DialogueBoxController.instance.StartDialogue(chosenFish.dialogue);

            sliderWithinBar = false;
        }
    }

    private void Fish()
    {

        // randomly choose fish from fish pool
        chosenFish = fishPool[new System.Random().Next(0, fishPool.Count - 1)];
        

        // set pos of green space on slider
        int w = chosenFish.weight;
        Debug.Log("fish weight is " + w);

        switch (w)
        {
            case 2:
                // width of 20
                Debug.Log("hard bar chosen");
                currentBar = hardBar;
                hardBar.RemoveFromClassList("remove");
                hardBar.style.left = new System.Random().Next(0, 280);
                Debug.Log(hardBar.resolvedStyle.left);
                break;
            case 3:
                // width of 40
                Debug.Log("medium bar chosen");
                currentBar = mediumBar;
                mediumBar.RemoveFromClassList("remove");
                mediumBar.style.left = new System.Random().Next(0, 260);
                Debug.Log(mediumBar.resolvedStyle.left);
                break;
            case 4:
                // width of 60
                Debug.Log("easy bar chosen");
                currentBar = easyBar;
                easyBar.RemoveFromClassList("remove");
                easyBar.style.left = new System.Random().Next(0, 240);
                Debug.Log(easyBar.resolvedStyle.left);
                break;
            default:
                break;
        }

        isFishing = true;

       
    }

    void Update()
    {
        // fishing slider game!
        if (isFishing && !fishCaught)
        {
            // move slider btwn 0 and 100 back and forth at sliderSpeed
            if (increasing && fishingSlider.value < 100)
            {
                fishingSlider.value += sliderSpeed * Time.deltaTime;
            }
            else if (increasing && fishingSlider.value >= 100)
            {
                increasing = false;
            }
            else if (!increasing && fishingSlider.value > 0)
            {
                fishingSlider.value -= sliderSpeed * Time.deltaTime;
            }
            else if (!increasing && fishingSlider.value <= 0)
            {
                increasing = true;
            }

            // detect when slider is within green bar
            // multiply slider value by 3 since slider is 300px
            if (fishingSlider.value * 3 > currentBar.resolvedStyle.left && fishingSlider.value * 3 < currentBar.resolvedStyle.left + currentBar.resolvedStyle.width)
            {
                sliderWithinBar = true;
            } else
            {
                sliderWithinBar = false;
            }
        }

    }

    public void HidePrompt()
    {
        promptIsShowing = false;
        textAnim.SetBool("hide", true);
    }

    public void StopFishing()
    {
        // stop fishing
        // move fishing rod back to groundPos
        poleAnim.SetBool("poleUp", false);

        // turn off cameras
        cameraMove.SetActive(false);
        Camera.main.GetComponent<CinemachineBrain>().enabled = false;

        HidePrompt();

        // not fishing anymore
        isFishing = false;

        StopAllCoroutines();

        // turn pole dialogue script back on
        GetComponent<TriggerPoleDialogue>().enabled = true;

        Debug.Log("hiding rod prompt");
        GetComponent<TriggerPoleDialogue>().HideRodPrompt();

        // turn Proposal Prompt to active if it was active before we started fishing
        ProposalPromptController.Instance.ActivateProposalText();

        // turn this fishing script off
        this.enabled = false;
    }

    // user pressed V to stop fishing
    void OnLeave(bool Left)
    {
        if (Left & promptIsShowing)
        {
            StopFishing();
        }
    }

    // called after catching a fish and finishing dialogue
    void LeaveConversation()
    {
        Debug.Log("DialogueBoxController OnDialogueEnded callback triggered");
        Debug.Log("calling leaveConversation from StartFishing");
        if (isFishing)
        {
            Debug.Log("isFishing is true");

            // hide fishing game
            fishingSlider.AddToClassList("remove");
            bkgd.AddToClassList("remove");

            // show prompt again
            //fishingPrompt.SetActive(true);
            textAnim.SetBool("hide", false);
            promptIsShowing = true;

            // destroy fish ( throw back into water? )
            Debug.Log(instantiatedFish);
            Destroy(instantiatedFish);

            // hide fishing game bar
            currentBar.AddToClassList("remove");
            currentBar.style.left = 0;
        }
    }
}
