using UnityEngine;
using FMODUnity;
using NUnit.Framework;
using FMOD.Studio;

public class FMODSndSnRandomizer : MonoBehaviour
{
    [Header("FMOD Event")] 
    [SerializeField] private EventReference fmodEvent;

    [Header("Timers")]
    [SerializeField] private float timeRemaining;
    public float minTimeRandom;
    public float maxTimeRandom;
    private bool isTimerComplete;

    private StudioEventEmitter studioEventEmitter;
    private FMOD.Studio.EventInstance instance;
    FMOD.Studio.PLAYBACK_STATE pS;
    private Bounds volBounds;
    private Vector3 randomLocation;
    public GameObject newFMODObject;
    private GameObject newSpawn;
    private Vector3 ogScale;

    void Awake()
    {
        
        //newFMODObject = //new GameObject("newFmodObj");
        newSpawn = Instantiate(newFMODObject, GameObject.Find("AllSpawns").transform);
        volBounds = this.GetComponent<Collider>().bounds;
        randomLocation = GetRandomPointInBounds(volBounds);
        newSpawn.transform.position = randomLocation;
        RuntimeManager.AttachInstanceToGameObject(instance, newSpawn, newSpawn.GetComponent<Collider>());
        studioEventEmitter = newSpawn.GetComponent<StudioEventEmitter>();
        studioEventEmitter.EventReference = fmodEvent;
        instance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
        //instance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
    }

    void Update()
    {
        // if the timer is complete
        if (timeRemaining <= 0 && !studioEventEmitter.IsPlaying()/*(PlaybackState(instance) != FMOD.Studio.PLAYBACK_STATE.PLAYING)*/ )
        {
            // play the fmod event
            PlayFMODEvent();
            // and reset the timer
            ResetTimer();
        } else
        {
            // otherwise, start the function to check & adjust timer status
            timeRemaining -= Time.deltaTime;
            //Debug.Log("Time Remaining: " + timeRemaining);
            //Timer();
        }
    }

    FMOD.Studio.PLAYBACK_STATE PlaybackState(FMOD.Studio.EventInstance thisSound)
    {
        thisSound.getPlaybackState(out pS);
        return pS;
    }

   /* private void Timer()
    {
        // if the time remaining between event plays is greater than 0
        if (timeRemaining > 0)
        {
            // tick the time down
            timeRemaining -= Time.deltaTime;
            Debug.Log("Time Remaining: " + timeRemaining);
        } else
        {
            // otherwise, the timer is done, actually!
            isTimerComplete = true;
        }
    }*/

    private void PlayFMODEvent()
    {
        randomLocation = GetRandomPointInBounds(volBounds);
        newSpawn.transform.position = randomLocation;
        studioEventEmitter.Play();
        //instance.start();
    }

    private void ResetTimer()
    {
        timeRemaining = Random.Range(minTimeRandom, maxTimeRandom);
    }

   public Vector3 GetRandomPointInBounds(Bounds bounds)
    {
       /* float minX = bounds.size.x * -0.5f;
        float minY = bounds.size.y * -0.5f;
        float minZ = bounds.size.z * -0.5f;*/

        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z));

    }

}
