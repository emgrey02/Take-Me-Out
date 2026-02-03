using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryModel
{
    public event System.Action OnInventoryUpdated;

    private bool[] _inventory;

    public bool[] Inventory { 
        get => _inventory;
        set
        {
            _inventory = value;
            OnInventoryUpdated?.Invoke();
        }
    }

    public InventoryModel()
    {
        Inventory = new bool[7];
    }

    public void AddToInventory(int index)
    {
        Inventory[index] = true;
        OnInventoryUpdated?.Invoke();
    }

}
