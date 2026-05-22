[System.Serializable]

// class to represent the player's inventory of cards
// only InventoryPresenter interacts with this class
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