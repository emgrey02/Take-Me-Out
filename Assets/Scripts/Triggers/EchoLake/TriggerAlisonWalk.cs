using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.Playables;
using System.Collections;

public class TriggerAlisonWalk:MonoBehaviour
{
    // SO dialogue asset
    public DialogueAsset dialogue;

    CharacterController controller;

    private bool walking;

    void Start() 
    {
        // start camera movement
        Camera.main.GetComponent<CinemachineBrain>().enabled = true;

        // move myself so alison follows
        GameObject player = GameObject.FindWithTag("Player");
        controller = player.GetComponent<CharacterController>();
        player.transform.rotation = Quaternion.Euler(0f, -40f, 0f);
        
        //Take control of player
        goForWalk();
    }


    void OnDisable() {
        DialogueBoxController.OnDialogueEnded -= LeaveConversation;
    }
 

    private void goForWalk()
    {
        walking = true;
        Debug.Log("starting coroutine");
        StartCoroutine("WaitForDialogue");
    }

    IEnumerator WaitForDialogue()
    {
         yield return new WaitForSeconds(1);
         DialogueBoxController.instance.StartDialogue(dialogue);
         DialogueBoxController.OnDialogueEnded += LeaveConversation;
    }

    void Update() 
    {
        if (walking)
        {
            controller.Move(new Vector3(-.5f, 0f, 0f) * Time.deltaTime);
        }
    }

    public void LeaveConversation()
    {
        Debug.Log("leaving conversation");
        Camera.main.GetComponent<CinemachineBrain>().enabled = false;
        walking = false;


        // so player cant go through this dialogue again
        GetComponent<Collider>().enabled = false;

        // turn off this script
        this.enabled = false;

    }

}

