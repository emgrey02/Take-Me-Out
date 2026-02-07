using UnityEngine;
using System;

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
        inventory = new Inventory();
        DontDestroyOnLoad(gameObject);
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
