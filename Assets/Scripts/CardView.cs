using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class CardView : MonoBehaviour
{
    [Header("Input Actions")]
    public InputAction Interact;

    public GameObject UIPrompt;

    private CardPresenter Presenter;
    
    public GameObject CardObject;
    public Sprite CardImage;
    public int CardIndex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Interact = InputSystem.actions.FindAction("Interact");
        Presenter = new CardPresenter(CardIndex);
    }

    // Update is called once per frame
    void Update()
    {
        if (UIPrompt.activeSelf && Interact.triggered)
        {
            Debug.Log("Card is Picked Up!");
            Presenter.OnFindCard(CardIndex);
            CardObject.transform.parent.gameObject.SetActive(false);
            UIPrompt.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider player)
    {
        Debug.Log("Player entered area around card");
        UIPrompt.SetActive(true);
    }

    void OnTriggerExit(Collider player)
    {
        Debug.Log("Player left area around card");
        UIPrompt.SetActive(false);
    }
}
