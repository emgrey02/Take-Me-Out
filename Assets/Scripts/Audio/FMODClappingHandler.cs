using UnityEngine;
using FMODUnity;

public class FMODClappingHandler : MonoBehaviour
{
    [Header("FMOD Event")] 
    [SerializeField] private EventReference fmodEvent;

    public float clapping;
    private int pplClapping = 10;

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
        FMOD.RESULT result = RuntimeManager.StudioSystem.getParameterByName("readyToClap", out float value, out clapping);
        Debug.Log("clapping: " + clapping + "; fmod result: " + result);
        if (clapping == 1.0 && !studioEventEmitter.IsPlaying())
        {
            Debug.Log("clapping: " + clapping + " & we're in");
            PlayFMODEvent();
        }
    }

    FMOD.Studio.PLAYBACK_STATE PlaybackState(FMOD.Studio.EventInstance thisSound)
    {
        thisSound.getPlaybackState(out pS);
        return pS;
    }


    private void PlayFMODEvent()
    {
        for (int i = 0; i < pplClapping; i++) {
        randomLocation = GetRandomPointInBounds(volBounds);
        newSpawn.transform.position = randomLocation;
        studioEventEmitter.Play();
        }
        
        RuntimeManager.StudioSystem.setParameterByName("readyToClap", 0);
        //instance.start();
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
