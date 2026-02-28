using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class InvUpdatedEventArgs : EventArgs {
    public bool[] Cards { get; }
    public InvUpdatedEventArgs(bool[] cards)
    {
        Cards = cards;
    }
}

public class InventoryPresenter : MonoBehaviour
{
    public event EventHandler<InvUpdatedEventArgs> InventoryUpdated;
    private Inventory inventory;
    private SaveManager SaveManager;

    void Awake()
    {
        // get save manager
        SaveManager = GameObject.FindWithTag("SaveManager").GetComponent<SaveManager>();
    }

    void Start()
    {
        inventory = SaveManager.LoadInventory();
        if (inventory == null)
        {
            Debug.Log("Creating new inventory");
            inventory = new Inventory();
        }
        else 
        {
            Debug.Log("Populated inventory from save");
        }

        // update inventory at start
        OnInventoryUpdated(new InvUpdatedEventArgs(inventory.Cards));
    }

    public void PickUpCard(CardView cardView)
    {
        Debug.Log("Picking up card");
        // tell inventory to add ths card
        inventory.AddCard(cardView.Card);

        // save inventory on card PickUpCard
        SaveManager.SaveInventory(inventory);

        // emit event to listeners (InventoryView)
        OnInventoryUpdated(new InvUpdatedEventArgs(inventory.Cards));
    }

    // event emitter
    protected virtual void OnInventoryUpdated(InvUpdatedEventArgs e)
    {
        InventoryUpdated?.Invoke(this, e);
    }
  
}
