using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryModel
{
    public event System.Action OnInventoryUpdated;

    private CardModel[] _inventory;

    public CardModel[] Inventory { 
        get => _inventory;
        set
        {
            _inventory = value;
            OnInventoryUpdated?.Invoke();
        }
    }

    public InventoryModel()
    {
        Inventory = new CardModel[7];
    }

    public void AddToInventory(CardModel card)
    {
        Inventory[card.InventoryIndex] = card;
        OnInventoryUpdated?.Invoke();
    }

    public CardModel GetCardFromIndex(int index)
    {
        CardModel card = Inventory[index];
        return card;
    }

}
