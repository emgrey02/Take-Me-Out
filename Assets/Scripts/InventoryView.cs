using UnityEngine;
using System;

public class InventoryView : MonoBehaviour
{
    public GameObject[] CardImages;
    public GameObject InventoryCanvas;

    private InventoryPresenter invPresenter;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        invPresenter = InventoryCanvas.GetComponent<InventoryPresenter>();
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
