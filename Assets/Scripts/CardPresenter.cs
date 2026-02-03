using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardPresenter
{
    public CardModel Model;

    public CardView View;

    public CardPresenter(int index)
    {
        Model = new CardModel(index);
    }

    public void OnFindCard(int index)
    {
        Model.FindCard(index);
    }
}
