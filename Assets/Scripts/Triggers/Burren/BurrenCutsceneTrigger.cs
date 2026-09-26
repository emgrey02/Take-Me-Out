using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.Playables;
using FMODUnity;
using FMOD.Studio;
using FMODUnityResonance;
using System.Collections;

public class BurrenCutsceneTrigger : MonoBehaviour
{
    public InputReader inputReader;

    public GameObject EnterCutscenePrompt;
    public bool inTableArea = false;

    // objects with timelines
    public GameObject cameraMove;
    public GameObject sipAndFinishAnimation;

    public GameObject fadeOut;

    // director that controls cameraMove timeline
    public PlayableDirector firstDirector;

    // director that controls sipandFinishAnimation timeline
    public PlayableDirector secondDirector;

    // SO dialogue asset
    public DialogueAsset dialogue;

    // FMOD Events
    [SerializeField] EventReference dialogueTalk;
    public EventInstance dateTime;
    public Bus MasterBus;

    void OnEnable()
    {
        firstDirector.paused += OnFirstDirectorPaused;
        secondDirector.stopped += OnSecondDirectorStopped;
        DialogueBoxController.OnDialogueEnded += LeaveConversation;
        dateTime = RuntimeManager.CreateInstance("snapshot:/FirstDate");
        MasterBus = RuntimeManager.GetBus("bus:/");
    }

    void OnFirstDirectorPaused(PlayableDirector aDirector)
    {
        if (firstDirector == aDirector)
        {
            DialogueBoxController.instance.StartDialogue(dialogue);
        }
    }

    void OnSecondDirectorStopped(PlayableDirector aDirector)
    {
        if (secondDirector == aDirector)
        {
            // go back to baseball field
            fadeOut.SetActive(true);
            StartCoroutine(FadeOut());
        }
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(7f);
        GameManager.Instance.MoveToScene(1);
    }

    void Start()
    {
        inputReader.InteractEvent += OnInteract;
    }

    void OnInteract(bool Interacted)
    {
        if (Interacted & inTableArea)
        {
            // FMOD
            // Play dialogue talk sfx
            RuntimeManager.PlayOneShot(dialogueTalk);
            dateTime.start();
            // trigger cutscene
            EnterCutscenePrompt.SetActive(false);
            cameraMove.SetActive(true);
            Camera.main.GetComponent<CinemachineBrain>().enabled = true;
        }
    }

    void OnTriggerEnter(Collider player)
    {
        Debug.Log("Player entered area around table");
        EnterCutscenePrompt.SetActive(true);
        inTableArea = true;
    }

    void OnTriggerExit(Collider player)
    {
        Debug.Log("Player left area around table");
        EnterCutscenePrompt.SetActive(false);
        inTableArea = false;
    }

    void LeaveConversation()
    {
        sipAndFinishAnimation.SetActive(true);
        dateTime.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        dateTime.release();
    }

    void OnDisable()
    {
        inputReader.InteractEvent -= OnInteract;
        firstDirector.paused -= OnFirstDirectorPaused;
        secondDirector.stopped -= OnSecondDirectorStopped;
        DialogueBoxController.OnDialogueEnded -= LeaveConversation;
    }
}
