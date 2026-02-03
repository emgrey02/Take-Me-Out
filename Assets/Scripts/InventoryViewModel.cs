using UnityEngine;
using System.ComponentModel;

public class InventoryViewModel
{
    private readonly InventoryModel _model;

    public InventoryViewModel(InventoryModel model)
    {
        _model = model;
    }

    public CardModel[] Inventory => _model.Inventory;

    public void OnAddToInventory(CardModel card)
    {
        _model.AddToInventory(card);
    }
}

