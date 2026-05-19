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

    public Vector3 fishingPos;
    public Vector3 fishingRot;

    public Vector3 groundPos;

    public FishAsset[] fish = new FishAsset[4];

    public List<FishAsset> fishPool = new();

    private GameObject instantiatedFish;

    public Vector3 fishPos;

    void OnEnable()
    {
        firstDirector.paused += OnFirstDirectorPaused;
    }

    void OnDisable()
    {
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

    public void PrepareToFish()
    {
        inputReader.InteractEvent += OnInteract;
        inputReader.LeaveEvent += OnLeave;

        // start cutscene cameras
        cameraMove.SetActive(true);
        Camera.main.GetComponent<CinemachineBrain>().enabled = true;
        
        // set fishing pole location
        transform.position = fishingPos;
        transform.rotation = Quaternion.Euler(fishingRot);
        
    }

    void OnInteract(bool Interacted)
    {
        if (Interacted & promptIsShowing)
        {   
            // hide world text
            fishingPrompt.SetActive(false);
            promptIsShowing = false;

            // start fishing
            StopAllCoroutines();
            StartCoroutine("Fish");
        }
    }

    void OnLeave(bool Left)
    {
        if (Left & promptIsShowing)
        {
            // stop fishing
            // move fishing rod back to groundPos
            transform.position = groundPos;
            transform.rotation = Quaternion.identity;

            // turn off cameras
            cameraMove.SetActive(false);
            Camera.main.GetComponent<CinemachineBrain>().enabled = false;

            // turn off fishing prompt
            fishingPrompt.SetActive(false);
            promptIsShowing = false;
           
            //StopAllCoroutines();

            // turn dialogue script back on
            GetComponent<TriggerPoleDialogue>().enabled = true;

            // V important before disabling the script - will still listen even when disabled
            DialogueBoxController.OnDialogueEnded -= LeaveConversation;

            // turn this script off
            this.enabled = false;
        }
    }

     void OnFirstDirectorPaused(PlayableDirector aDirector)
    {
        Debug.Log("first director paused");
        if (firstDirector == aDirector)
        {
            // show fish prompt
            fishingPrompt.SetActive(true);
            promptIsShowing = true;
        }
    }

    IEnumerator Fish() {
        // random amount of time to wait for fish to catch
        yield return new WaitForSeconds(new System.Random().Next(2,6));

        // randomly choose fish from fish pool
        FishAsset fishAsset = fishPool[new System.Random().Next(0, fishPool.Count - 1)];
        GameObject fish = fishAsset.fishPrefab;
        instantiatedFish = Instantiate(fish, fishPos, fish.transform.rotation);

        // start dialogue
        DialogueBoxController.OnDialogueEnded += LeaveConversation;
        DialogueBoxController.instance.StartDialogue(fishAsset.dialogue);
    }

    private void LeaveConversation()
    {
        Debug.Log("setting fishing prompt to active!");
        // show prompt
        fishingPrompt.SetActive(true);
        promptIsShowing = true;

        // destroy fish ( throw back into water? )
        Destroy(instantiatedFish);
    }
}
