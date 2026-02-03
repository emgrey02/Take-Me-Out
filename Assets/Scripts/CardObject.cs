using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardObject : MonoBehaviour
{
    public int InventoryIndex;

    void OnTriggerEnter(Collider player)
    {
        Debug.Log("Player entered area around card");
        // show UI text
    }
}
