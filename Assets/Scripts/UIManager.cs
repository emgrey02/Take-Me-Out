using UnityEngine;
using System;

public class UIManager : MonoBehaviour
{
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
}
