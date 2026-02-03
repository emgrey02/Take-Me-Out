using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardModel
{
    public int InventoryIndex;
    public bool Found;
    public CardPresenter Presenter;

    public CardModel(int index)
    {
        InventoryIndex = index;
        Found = false;
    }

    public void FindCard(int index)
    {
        Found = true;
    }
}