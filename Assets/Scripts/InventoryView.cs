using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class InventoryView : MonoBehaviour
{
    [SerializeField] InputReader inputReader;

    public List<VisualElement> Cards = new List<VisualElement>();
    private InventoryPresenter InventoryManager;

    private GameObject MainMenu;
    private VisualElement mmPanel;

    public VisualElement invPanel;

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

        // hide inventory
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
        // hide all the cards
        foreach (VisualElement btn in Cards)
        {
            btn.AddToClassList("hide");
        }
        
        // only show cards we have
        foreach (Card card in e.Cards)
        {
            if (card != null)
            {
                Debug.Log(card.index);
                Cards[card.index].RemoveFromClassList("hide");
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

    void Update()
    {
        Debug.Log("Inventory:" + inputReader.PlayerControlsStatus());   
    }
}
