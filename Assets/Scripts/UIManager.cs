using UnityEngine;
using System;

public class UIManager : MonoBehaviour
{
    public GameObject PickupPrompt;
    public GameObject FirstScenePrompt;

    public GameObject[] CardImages;
    public GameObject InventoryMenu;

    private InventoryPresenter invPresenter;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        invPresenter = InventoryMenu.GetComponent<InventoryPresenter>();
        invPresenter.InventoryUpdated += UpdateInventoryUI;
    }

    void OnDisable()
    {
        invPresenter.InventoryUpdated -= UpdateInventoryUI;
    }

    private void UpdateInventoryUI(object sender, InvUpdatedEventArgs e)
    {
        Debug.Log("updating ui from ui manager");
        foreach (GameObject img in CardImages)
        {
            img.SetActive(false);
        }
        
        foreach (Card card in e.Cards)
        {
            if (card != null)
            {
                Debug.Log(card.index);
                CardImages[card.index].SetActive(true);
            }
        }
        
    }

    public void ShowPrompt(string prompt)
    {
        switch (prompt)
        {
            case "Pickup":
                PickupPrompt.SetActive(true);
                break;
            case "FirstScene":
                FirstScenePrompt.SetActive(true);
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
            case "FirstScene":
                FirstScenePrompt.SetActive(false);
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
            case "First Scene Prompt":
                return FirstScenePrompt.activeSelf;
        }
        return false;
    }
}
