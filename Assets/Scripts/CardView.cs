using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class CardView : MonoBehaviour
{
    public UIManager uiManager;
    public Card Card;

    public int CardIndex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Card = new Card(CardIndex);
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
