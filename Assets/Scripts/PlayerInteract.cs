using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    public InputAction Interact;
    public InventoryPresenter invPresenter;
    public UIManager uiManager;
    public GameObject[] PickupCards;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Interact = InputSystem.actions.FindAction("Interact");
    }

    // Update is called once per frame
    void Update()
    {
        if (uiManager.getActiveState("Pickup Prompt") && Interact.triggered)
        {
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 3f))
            {
                CardView card = hit.collider.GetComponent<CardView>();
                invPresenter.PickUpCard(card);
                uiManager.HidePrompt("Pickup");
                PickupCards[card.CardIndex].SetActive(false);
            }
        }
    }
}
