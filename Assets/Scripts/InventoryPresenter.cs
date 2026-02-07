using UnityEngine;
using System;

public class InventoryPresenter : MonoBehaviour
{
    private Inventory inventory;
    public UIManager uiManager;

 
    void OnDisable()
    {
        //inventory.InventoryUpdated -= UpdateInventoryUI;
    }

    void Start()
    {
        inventory = new Inventory();
        //inventory.InventoryUpdated += UpdateInventoryUI;
    }

    public void PickUpCard(CardView cardView)
    {
        inventory.AddCard(cardView.Card);
        uiManager.UpdateInventoryUI(inventory.Cards);
    }

    /**
    private void UpdateInventoryUI(object sender, EventArgs e)
    {
        
    }
   **/
  
}
