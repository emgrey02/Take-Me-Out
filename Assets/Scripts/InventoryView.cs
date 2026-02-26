using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class InventoryView : MonoBehaviour
{
    [SerializeField] InputReader inputReader;

    public List<VisualElement> Cards = new List<VisualElement>();
    public GameObject InventoryManager;

    public GameObject MainMenu;
    private VisualElement mmPanel;

    public VisualElement invPanel;

    private InventoryPresenter invPresenter;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        invPanel = GetComponent<UIDocument>().rootVisualElement;
        mmPanel = MainMenu.GetComponent<UIDocument>().rootVisualElement;

        Cards = invPanel.Query(className: "unity-button").ToList();
        invPanel.AddToClassList("hide");
    }

    void Start()
    {
        inputReader.InventoryToggleEvent += OnInventoryToggle;
    }

    void OnEnable()
    {
        invPresenter = InventoryManager.GetComponent<InventoryPresenter>();
        invPresenter.InventoryUpdated += UpdateInventoryUI;
    }

    void OnDisable()
    {
        invPresenter.InventoryUpdated -= UpdateInventoryUI;
    }

    private void UpdateInventoryUI(object sender, InvUpdatedEventArgs e)
    {
        Debug.Log("updating inventory from InventoryView");
        foreach (VisualElement btn in Cards)
        {
            btn.AddToClassList("hide");
        }
        
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
        if (InventoryToggled && mmPanel.ClassListContains("hide"))
        {
            Debug.Log("inventory toggle triggered");
            invPanel.ToggleInClassList("hide");

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
