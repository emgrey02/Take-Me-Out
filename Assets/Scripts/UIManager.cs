using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject PickupPrompt;
    public GameObject[] CardImages;
    public GameObject InventoryMenu;

    public void ShowPrompt(string prompt)
    {
        switch (prompt)
        {
            case "Pickup":
                PickupPrompt.SetActive(true);
                break;
        }  
    }

    public void HidePrompt(string prompt)
    {
        switch (prompt)
        {
            case "Pickup":
                PickupPrompt.SetActive(false);
                break;
        }
        
    }

    public bool getActiveState(string name)
    {
        switch (name)
        {
            case "Inventory Menu":
                return InventoryMenu.activeSelf;
            case "Pickup Prompt":
                return PickupPrompt.activeSelf;
        }
        return false;
    }

    public void UpdateInventoryUI(Card[] cards)
    {
        Debug.Log("updating ui from ui manager");
        foreach (GameObject img in CardImages)
        {
            img.SetActive(false);
        }

        foreach (Card card in cards)
        {
            if (card != null)
            {
                CardImages[card.index].SetActive(true);
            }
        }
    }
}
