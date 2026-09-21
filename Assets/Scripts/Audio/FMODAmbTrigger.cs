using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class FMODAmbTrigger : MonoBehaviour
{

    [Header("FMOD Amb Event")]
    /* public StudioEventEmitter audioCollidedFL;
    public StudioEventEmitter audioCollidedFR;
    public StudioEventEmitter audioCollidedBL;
    public StudioEventEmitter audioCollidedBR; */
    public StudioEventEmitter thisAudioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(this.gameObject.name + "collided with: " + other.gameObject.name);
        if (other.gameObject.name == "Amb Zone Trigger")
        {
            switchParam(1f);
        }
        /* switch (other.gameObject.name) {
            case "QuadAmbBL":
                Debug.Log("got bl");
                switchParam(audioCollidedBL);
                break;
            case "QuadAmbFL":
                Debug.Log("got fl");
                switchParam(audioCollidedFL);
                break;
            case "QuadAmbBR":
                Debug.Log("got br");
                switchParam(audioCollidedBR);
                break;
            case "QuadAmbFR":
                Debug.Log("got fr");
                switchParam(audioCollidedBR);
                break;
            case null:
                Debug.Log("nada");
                break;
        } */
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log(this.gameObject.name + "is leaving: " + other.gameObject.name);
        if (other.gameObject.name == "Amb Zone Trigger")
        {
            switchParam(0f);
        }
        /* switch (other.gameObject.name) {
            case "QuadAmbBL":
                Debug.Log("got bl");
                switchParam(audioCollidedBL);
                break;
            case "QuadAmbFL":
                Debug.Log("got fl");
                switchParam(audioCollidedFL);
                break;
            case "QuadAmbBR":
                Debug.Log("got br");
                switchParam(audioCollidedBR);
                break;
            case "QuadAmbFR":
                Debug.Log("got fr");
                switchParam(audioCollidedBR);
                break;
            case null:
                Debug.Log("nada");
                break;
        } */
    }

    void switchParam(float num) // (StudioEventEmitter ambEmitter)
    {
        thisAudioSource.SetParameter("theaterLocation", num, false);
        /* ambEmitter.gameObject.GetComponent<FMODTheaterAssignTrigger>().changeState();
        if (ambEmitter.gameObject.GetComponent<FMODTheaterAssignTrigger>().isEntering)
        {
            ambEmitter.SetParameter("theaterLocation", 1f, false);
        } else
        {
            ambEmitter.SetParameter("theaterLocation", 0f, false);
        } */
    }
}
