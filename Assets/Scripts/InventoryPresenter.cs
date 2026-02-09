using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class InvUpdatedEventArgs : EventArgs {
    public Card[] Cards { get; }
    public InvUpdatedEventArgs(Card[] cards)
    {
        Cards = cards;
    }
}

public class InventoryPresenter : MonoBehaviour
{
    public event EventHandler<InvUpdatedEventArgs> InventoryUpdated;

    private Inventory inventory;

    public InputAction inventoryToggle;

    public GameObject inventoryMenu;

    void Awake()
    {
        inventory = new Inventory();
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        inventoryToggle = InputSystem.actions.FindAction("InventoryToggle");  
    }

    void Update()
    {
        if (inventoryToggle.triggered)
        {
            inventoryMenu.SetActive(!inventoryMenu.activeSelf);
        }
    }

    public void PickUpCard(CardView cardView)
    {
        inventory.AddCard(cardView.Card);
        OnInventoryUpdated(new InvUpdatedEventArgs(inventory.Cards));
    }

    protected virtual void OnInventoryUpdated(InvUpdatedEventArgs e)
    {
        InventoryUpdated?.Invoke(this, e);
    }
  
}
