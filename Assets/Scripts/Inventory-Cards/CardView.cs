using UnityEngine;

// class to represent a the physical card to be picked up in the game - a MonoBehaviour 
// connects to the Card class
// handles UI logic as well as communicating with the InventoryPresenter when the player picks up the card
public class CardView : MonoBehaviour
{
    
    [SerializeField] InputReader inputReader;

    public GameObject pickupText;
    private InventoryPresenter invPresenter;
    public Card Card;
    public bool inCardArea = false;

    // we set this in the unity editor for each card prefab to determine which card it is
    public int CardIndex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Card = new Card(CardIndex);
        invPresenter = GameObject.FindWithTag("Inventory").GetComponent<InventoryPresenter>();
        inputReader.InteractEvent += OnInteract;
    }

    void OnTriggerEnter(Collider player)
    {
        Debug.Log("Player entered area around card");
        pickupText.SetActive(true);
        inCardArea = true;
    }


    void OnTriggerExit(Collider player)
    {
        Debug.Log("Player left area around card");
        pickupText.SetActive(false);
        inCardArea = false;
    }
    private void OnInteract(bool Interacted)
    {
        if (Interacted && inCardArea)
        {
            Debug.Log("player pressed E within card area");
            // add card to inventory and hide card in world
            invPresenter.PickUpCard(gameObject.GetComponent<CardView>());
            gameObject.SetActive(false);
            inCardArea = false;
        }
    }

    void OnDisable()
    {
        inputReader.InteractEvent -= OnInteract;
    }
}
