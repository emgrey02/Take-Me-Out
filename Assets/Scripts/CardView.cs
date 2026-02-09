using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class CardView : MonoBehaviour
{
    public InputAction Interact;
    public GameObject pickupText;
    public InventoryPresenter invPresenter;
    public Card Card;
    public bool inCardArea = false;

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
        pickupText.SetActive(true);
        inCardArea = true;
    }

    void Update()
    {
        if (Interact.triggered && inCardArea)
        {
            Debug.Log("player pressed E within card area");
            invPresenter.PickUpCard(gameObject.GetComponent<CardView>());
            gameObject.SetActive(false);
        }
    }

    void OnTriggerExit(Collider player)
    {
        Debug.Log("Player left area around card");
        pickupText.SetActive(false);
        inCardArea = false;
    }
}
