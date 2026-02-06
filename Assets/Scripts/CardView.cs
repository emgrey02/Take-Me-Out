using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class CardView : MonoBehaviour
{
    public InputAction Interact;

    public UIManager uiManager;
    public Card Card;

    public int CardIndex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Card = new Card(CardIndex);
        Interact = InputSystem.actions.FindAction("Interact");
    }

    void OnTriggerEnter(Collider player)
    {
        Debug.Log("Player entered area around card");
        uiManager.ShowPrompt("Pickup");
    }

    void OnTriggerExit(Collider player)
    {
        Debug.Log("Player left area around card");
        uiManager.HidePrompt("Pickup");
    }
}
