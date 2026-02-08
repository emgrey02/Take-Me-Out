using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    public InputAction Interact;
    public InventoryPresenter invPresenter;
    public UIManager uiManager;
    public GameObject[] PickupCards;
    public SceneController sceneController;
    public Transform player;

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
                Debug.Log(card);
                invPresenter.PickUpCard(card);
                uiManager.HidePrompt("Pickup");
                PickupCards[card.CardIndex].SetActive(false);
            }
        }

        if (uiManager.getActiveState("First Scene Prompt") && Interact.triggered)
        {

            Debug.Log("Send to first scene experience");
            player.transform.rotation = new Quaternion(0, 130f, 0, 0);
            player.position = new Vector3 (0, 1f, 0);
            Physics.SyncTransforms();
            sceneController.MoveToScene(2);
            // scene transition
            // change scene
        }
    }
}
