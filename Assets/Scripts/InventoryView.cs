using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using System.Linq;

public class InventoryView : MonoBehaviour
{
    [SerializeField] InputReader inputReader;

    // ui inventory & card slots
    public VisualElement invPanel;
    public List<VisualElement> Cards = new List<VisualElement>();

    // card gameobject prefab to instantiate
    public GameObject cardObjPrefab;

    // card object scriptable objects
    public CardObjectSO[] cardSpawnValues;

    // main menu ui
    private GameObject MainMenu;
    private VisualElement mmPanel;
    
    // inventory presenter - to talk to Inventory class
    private InventoryPresenter invPresenter;
    
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
    }

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
            if (e.Cards[i])
            {
                Cards[i].RemoveFromClassList("hide");
            }
            else 
            {
                // if we are in baseball field scene and card doesnt already exist in scene
                if (GameObject.Find(cardSpawnValues[i].cardName) == null && GameManager.Instance.GetSceneId() == 1)
                {
                    // use corresponding SO spawn point, name, and card index
                    GameObject c = Instantiate(cardObjPrefab, cardSpawnValues[i].spawnPoint, Quaternion.identity);
                    c.name = cardSpawnValues[i].cardName;
                    c.GetComponent<CardView>().CardIndex = cardSpawnValues[i].cardIndex;
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
            // toggle hide class
            invPanel.ToggleInClassList("hide");

            // disable/enable player controls if inventory on screen or not
            if (invPanel.ClassListContains("hide"))
            {
                inputReader.EnablePlayerControls();
            }
            else {
                inputReader.DisablePlayerControls();
            }
        }
    }
}
