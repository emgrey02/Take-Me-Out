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

    void Awake()
    {
        if (inventory == null)
        {
            inventory = new Inventory();
        }
    }

    void Start()
    {
        // update inventory at start
        OnInventoryUpdated(new InvUpdatedEventArgs(inventory.Cards));
    }

    public void PickUpCard(CardView cardView)
    {
        Debug.Log("Picking up card");
        // tell inventory to add ths card
        inventory.AddCard(cardView.Card);

        // emit event to listeners (InventoryView)
        OnInventoryUpdated(new InvUpdatedEventArgs(inventory.Cards));
    }

    // event emitter
    protected virtual void OnInventoryUpdated(InvUpdatedEventArgs e)
    {
        InventoryUpdated?.Invoke(this, e);
    }
  
}
