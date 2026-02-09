using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Input Actions")]
    public InputAction moveAction; // Vector2
    
    public float moveSpeed = 5f;
    public float gravity = -9.81f;
    private CharacterController controller;
    public GameObject InventoryMenu;

    private Vector3 velocity;


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
            Vector2 moveVector = moveAction.ReadValue<Vector2>() * moveSpeed;
            Vector3 move = (moveVector.y * transform.forward) + (moveVector.x * transform.right);

            //Combine movement and gravity
            if (controller.isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
            controller.Move(move * Time.deltaTime);
        }
        
    }
}
