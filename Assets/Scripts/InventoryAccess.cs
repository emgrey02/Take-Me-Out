using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class InventoryAccess : MonoBehaviour
{
    [Header("Input Actions")]
    public InputAction inventoryToggle;

    public GameObject inventoryMenu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventoryToggle = InputSystem.actions.FindAction("InventoryToggle");
    }

    void Update()
    {
        if (inventoryToggle.triggered)
        {
            inventoryMenu.SetActive(!inventoryMenu.activeSelf);
        }
    }

}
