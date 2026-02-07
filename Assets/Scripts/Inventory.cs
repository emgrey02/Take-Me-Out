using UnityEngine;
using System;

public class Inventory
{
    public Card[] Cards;

    public Inventory()
    {
        Cards = new Card[7];
    }

    public void AddCard(Card newCard)
    {
        Cards[newCard.index] = newCard;
    }
}