using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.Playables;
using System.Collections.Generic;
using System.Collections;

public class StartFishing : MonoBehaviour
{
    [SerializeField] InputReader inputReader;

    // object with timeline
    public GameObject cameraMove;

    // director that controls cameraMove timeline
    public PlayableDirector firstDirector;

    public bool promptIsShowing;
    public GameObject fishingPrompt;

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
        // create fish pool based on weights
        for (int i=0; i < fish.Length; i++) {
            Debug.Log(fish[i].weight);
            int weight = fish[i].weight;

            for (int j=0; j < weight; j++) {
                Debug.Log("adding fish to fish pool");
                fishPool.Add(fish[i]);
            }
        }
       PrepareToFish();
    }

    // DialogueBoxController calls this when the user selects that they want to fish
    public void PrepareToFish()
    {
        inputReader.InteractEvent += OnInteract;
        inputReader.LeaveEvent += OnLeave;

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

            // set fishing pole location
            // start animation of fishing pole moving to fishing position
            //transform.position = fishingPos;
            //transform.rotation = Quaternion.Euler(fishingRot);
        }
    }

    // use presses E to interact/ fish when prompt is showing
    void OnInteract(bool Interacted)
    {
        if (Interacted & promptIsShowing)
        {
            HidePrompt();

            // start fishing
            StopAllCoroutines();
            StartCoroutine("Fish");
        }
    }

    IEnumerator Fish()
    {
        isFishing = true;
        // random amount of time to wait for fish to catch
        yield return new WaitForSeconds(new System.Random().Next(2, 6));

        // randomly choose fish from fish pool
        FishAsset fishAsset = fishPool[new System.Random().Next(0, fishPool.Count - 1)];
        GameObject fish = fishAsset.fishPrefab;
        instantiatedFish = Instantiate(fish, fishPos, fish.transform.rotation);

        // start dialogue
        Debug.Log("Subscribing to DialogueBoxController OnDialogueEnded event in StartFishing");
        DialogueBoxController.OnDialogueEnded += LeaveConversation;
        DialogueBoxController.instance.StartDialogue(fishAsset.dialogue);
    }

    public void HidePrompt()
    {
        // hide world text
        //fishingPrompt.SetActive(false);
        
        promptIsShowing = false;
        textAnim.SetBool("hide", true);
    }

    public void StopFishing()
    {
        // stop fishing
        // move fishing rod back to groundPos
        poleAnim.SetBool("poleUp", false);
        //transform.position = groundPos;
        //transform.rotation = Quaternion.identity;

        // turn off cameras
        cameraMove.SetActive(false);
        Camera.main.GetComponent<CinemachineBrain>().enabled = false;

        // hide world text
        textAnim.SetBool("hide", true);
        promptIsShowing = false;

        // not fishing anymore
        isFishing = false;

        StopAllCoroutines();

        // V important before disabling the script - will still listen even when disabled
        //Debug.Log("Unsubscribing to DialogueBoxController OnDialogueEnded event in StartFishing");
        //DialogueBoxController.OnDialogueEnded -= LeaveConversation;

        // turn pole dialogue script back on
        GetComponent<TriggerPoleDialogue>().enabled = true;

        // turn this fishing script off
        this.enabled = false;
    }

    void OnLeave(bool Left)
    {
        if (Left & promptIsShowing)
        {
            StopFishing();
        }
    }

    private void LeaveConversation()
    {
        Debug.Log("DialogueBoxController OnDialogueEnded callback triggered");
        Debug.Log("calling leaveConversation from StartFishing");
        if (isFishing)
        {
            Debug.Log("isFishing is true");
            // show prompt
            //fishingPrompt.SetActive(true);
            textAnim.SetBool("hide", false);
            promptIsShowing = true;

            // destroy fish ( throw back into water? )
            Destroy(instantiatedFish);
        }
    }
}
