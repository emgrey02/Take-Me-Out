using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Input Actions")]
    public InputAction moveAction; // Vector2
    
    public float moveSpeed = 5f;
    private CharacterController controller;
    public GameObject InventoryMenu;


    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        DontDestroyOnLoad(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
    }

    // Update is called once per frame
    void Update()
    {
        if (!InventoryMenu.activeSelf) {
            Vector2 moveVector = moveAction.ReadValue<Vector2>();
            Vector3 move = (moveVector.y * transform.forward) + (moveVector.x * transform.right);

            //Combine h & v mvmt
            Vector3 finalMove = (move * moveSpeed);
            controller.Move(finalMove * Time.deltaTime);
        }
        
    }
}
