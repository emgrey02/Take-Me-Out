using UnityEngine;
using System;

public class Inventory
{
    public event EventHandler InventoryUpdated;

    public Card[] Cards;

    public Inventory()
    {
        Cards = new Card[7];
    }

    public void AddCard(Card newCard)
    {
        Cards[newCard.index] = newCard;
        OnInventoryUpdated();
    }

    protected virtual void OnInventoryUpdated()
    {
        InventoryUpdated?.Invoke(this, EventArgs.Empty);
    }

}
