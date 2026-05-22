using UnityEngine;

public class RingFound : MonoBehaviour
{
    // SO dialogue asset
    public DialogueAsset dialogue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DialogueBoxController.instance.StartDialogue(dialogue);
        DialogueBoxController.OnDialogueEnded += LeaveConversation;
    }

    void OnDisable()
    {
        DialogueBoxController.OnDialogueEnded -= LeaveConversation;
    }

    private void LeaveConversation()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
