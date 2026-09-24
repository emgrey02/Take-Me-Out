using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class FMODAmbRotation : MonoBehaviour
{

   // [Header("FMOD Amb Event")]
    /* public StudioEventEmitter audioCollidedFL;
    public StudioEventEmitter audioCollidedFR;
    public StudioEventEmitter audioCollidedBL;
    public StudioEventEmitter audioCollidedBR; */
   // public StudioEventEmitter thisAudioSource;
    private Quaternion lastParentRotation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastParentRotation = transform.parent.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.Inverse(transform.parent.localRotation) * lastParentRotation * transform.localRotation;
        lastParentRotation = transform.parent.localRotation;
    }
}
