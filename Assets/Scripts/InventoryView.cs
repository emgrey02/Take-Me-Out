using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class InventoryView : MonoBehaviour
{
    public List<VisualElement> Cards = new List<VisualElement>();
    public GameObject InventoryManager;

    public InputAction inventoryToggle;
    public VisualElement invPanel;

    private InventoryPresenter invPresenter;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        invPanel = GetComponent<UIDocument>().rootVisualElement;

        Cards = invPanel.Query(className: "unity-button").ToList();
        invPanel.AddToClassList("hide");
    }

    void Start()
    {
        inventoryToggle = InputSystem.actions.FindAction("InventoryToggle");  
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

    void Update()
    {
        if (inventoryToggle.triggered)
        {
            Debug.Log("inventory toggle triggered");
            invPanel.ToggleInClassList("hide");
        }
    }
}
