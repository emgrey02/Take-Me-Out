using UnityEngine;

// class to represent a card to be picked up in the game
// all it needs is an index to represent which card it is
// this index connects it to its corresponding Scriptable Object
public class Card
{
    public int index;

    // constructor
    public Card(int i)
    {
        index = i;
    }
    
}
