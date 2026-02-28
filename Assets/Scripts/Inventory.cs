using UnityEngine;
using System;

[System.Serializable]
public class Inventory
{
    public bool[] Cards;

    public Inventory()
    {
        Cards = new bool[7];
    }

    public void AddCard(Card newCard)
    {
        Cards[newCard.index] = true;
    }
}