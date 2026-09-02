using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using System.Linq;
using FMODUnity;

// handles inventory UI, listens for inventory updates from InventoryPresenter
// also listens for inventory toggle input
public class InventoryView : MonoBehaviour
{
    [SerializeField] InputReader inputReader;

    // ui inventory & card slots
    public VisualElement invPanel;
    public List<VisualElement> Cards = new();

    // card gameobject prefab to instantiate
    public GameObject cardObjPrefab;

    // card object scriptable objects
    public CardObjectSO[] cardSpawnValues;

    [Header("FMOD Events")]
    [SerializeField] EventReference cardSelect;
    [SerializeField] EventReference inventoryOpen;
    [SerializeField] EventReference inventoryClose;

    // main menu ui
    private VisualElement mmPanel;
    
    // inventory presenter - to talk to Inventory class
    private InventoryPresenter invPresenter;

    private bool playerControlsStatus;

    void OnEnable()
    {
        // get inv presenter component for event subscription
        invPresenter = GetComponent<InventoryPresenter>();

        // listen for any inventory updates from inv presenter
        invPresenter.InventoryUpdated += UpdateInventoryUI;
    }

    void Start()
    {
        // get ui doc for inventory
        invPanel = GetComponent<UIDocument>().rootVisualElement;

        // fill list with ui cards
        Cards = invPanel.Query(className: "unity-button").ToList();

        // listen for click events on each card in inventory
        for (int i = 0; i < Cards.Count; i++)
        {
            Cards[i].RegisterCallback<ClickEvent>(OnCardClicked);
        }

        // hide inventory on start
        invPanel.AddToClassList("hide");
        
        // get main menu ui 
        GameObject MainMenu = GameObject.FindWithTag("MainMenu");
        mmPanel = MainMenu.GetComponent<UIDocument>().rootVisualElement;

        // subscribe to inventory toggle event
        inputReader.InventoryToggleEvent += OnInventoryToggle; 
    }

    void OnDisable()
    {   
        // unsubscribe to inventory toggle event
        invPresenter.InventoryUpdated -= UpdateInventoryUI;

        for (int i = 0; i < Cards.Count; i++)
        {
            Cards[i].UnregisterCallback<ClickEvent>(OnCardClicked);
        }
    }

    private void OnCardClicked(ClickEvent evt)
    {
        Debug.Log("card clicked");

        // FMOD
        // Play card select sfx
        RuntimeManager.PlayOneShot(cardSelect);

        VisualElement clickedCard = evt.target as VisualElement;
        Texture2D cardImage = clickedCard.style.backgroundImage.value.texture;
        VisualElement cardDisplay = invPanel.Q<VisualElement>("Big-Card");
        cardDisplay.style.backgroundImage = cardImage;
    }

    // when we get an inventory update event, update the inventory ui to match the current inventory state
    private void UpdateInventoryUI(object sender, InvUpdatedEventArgs e)
    {
        Debug.Log("updating inventory from InventoryView");
        
        // hide all the cards in inventory
        foreach (VisualElement btn in Cards)
        {
            btn.AddToClassList("hide");
        }
        
        // only show ui cards we have in inventory
        // only instantiate card prefabs we can still pickup
        for (int i = 0; i < 6; i++)
        {
            // if card is in our inventory, show it in the inventory ui
            Debug.Log(i + ": " + cardSpawnValues[i].cardName + " is in inventory: " + e.Cards[i]);
            if (e.Cards[i])
            {
                Cards[i].RemoveFromClassList("hide");
                if (cardSpawnValues[i].cardImage != null)
                {
                    Cards[i].style.backgroundImage = Resources.Load<Texture2D>(cardSpawnValues[i].cardImage);
                }
            }
            // since card isnt in our inventory, check if we are in baseball field scene and if card already exists in scene
            else
            {
                // instantiate card prefab if we are in baseball field scene and card doesnt already exist in scene
                if (GameObject.Find(cardSpawnValues[i].cardName) == null && GameManager.Instance.GetSceneId() == 1)
                {
                    Debug.Log("instantiating card prefab for " + cardSpawnValues[i].cardName);
                    Debug.Log(GameObject.Find(cardSpawnValues[i].cardName));

                    // use corresponding SO spawn point, name, card index, and image for the card prefab
                    GameObject c = Instantiate(cardObjPrefab, cardSpawnValues[i].spawnPoint, Quaternion.identity);
                    c.name = cardSpawnValues[i].cardName;
                    c.GetComponent<CardView>().CardIndex = cardSpawnValues[i].cardIndex;
                    Material cardMat = Resources.Load<Material>("pickup-cards/" + cardSpawnValues[i].materialName);
                    Debug.Log(cardMat);
                    GameObject cardChild = c.transform.GetChild(1).gameObject;
                    Debug.Log(cardChild.name);
                    cardChild.GetComponent<MeshRenderer>().material = cardMat;
                }
            }
        }
    }

    void OnInventoryToggle(bool InventoryToggled)
    {
        // if we toggled inventory with TAB and main menu isnt open
        if (InventoryToggled && mmPanel.ClassListContains("hide"))
        {
            Debug.Log("inventory toggle triggered");

            // FMOD
            // Play inventory open sfx
            RuntimeManager.PlayOneShot(inventoryOpen);

            // disable/enable player controls if inventory on screen or not
            if (invPanel.ClassListContains("hide"))
            {
                // get current playercontrols status
                playerControlsStatus = inputReader.PlayerControlsStatus();

                //open inventory 
                invPanel.RemoveFromClassList("hide");
                inputReader.DisablePlayerControls();

            }
            else {
                //close inventory
                
                // FMOD
                // Play inventory close sfx
                RuntimeManager.PlayOneShot(inventoryClose);

                invPanel.AddToClassList("hide");
                // if inventory is hidden, go back to what controls were before toggling inventory
                if (playerControlsStatus)
                {
                    // if they were on before, turn them back on, if they were off, leave them off
                    inputReader.EnablePlayerControls();
                }
            }
        }
    }
}
