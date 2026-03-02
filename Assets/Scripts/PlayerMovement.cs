using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    
    [SerializeField] InputReader inputReader;
    
    [Header("Player Movement Settings")]
    [SerializeField]
    private int _moveSpeed;
    public int moveSpeed
    {
        get
        {
            return _moveSpeed;
        }
        set
        {
            Debug.Log("setting move speed: " + value);
            _moveSpeed = value;
        }
    }
    public float gravity = -9.81f;
    private CharacterController controller;
    private Vector3 _velocity;
    private Vector3 _moveVector;

    private VisualElement invPanel;
    private VisualElement mmPanel;

    [Header("Mouse Look Settings")]
    [Tooltip("Sensitivity of mouse movement")]
    [SerializeField]
    private int _lookSensitivity;
    public int lookSensitivity 
    {
        get
        {
            return _lookSensitivity;
        }
        set
        {
            Debug.Log("setting look sensitivity: " + value);
            _lookSensitivity = value;
        }
    }
    // track's camera's x-axis rotation
    private float xRotation = 0f;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        // Subscribe to input events
        inputReader.MoveEvent += OnMove;
        inputReader.LookEvent += OnLook;
    }

    void Start()
    {
        // get ui gameobjects & ui elements
        GameObject InventoryMenu = GameObject.FindWithTag("Inventory");
        GameObject MainMenu = GameObject.FindWithTag("MainMenu");

        invPanel = InventoryMenu.GetComponent<UIDocument>().rootVisualElement;
        mmPanel = MainMenu.GetComponent<UIDocument>().rootVisualElement;

    }

    private void OnMove(Vector2 movement)
    {
        //Debug.Log("player moving");
        _moveVector = movement * _moveSpeed; 
        
    }

    void OnDisable()
    {
        // unsubscribe on destroy
        inputReader.MoveEvent -= OnMove;
        inputReader.LookEvent -= OnLook;
    }
    
    private void OnLook(Vector2 playerLook)
    {
        //Debug.Log("player looking");
        #if UNITY_STANDALONE
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
        #endif
        #if UNITY_EDITOR
            UnityEngine.Cursor.lockState = CursorLockMode.Confined;
            UnityEngine.Cursor.visible = false;
        #endif

        // up down
        float lookX = playerLook.x * _lookSensitivity * Time.deltaTime;

        // left right
        float lookY = playerLook.y * _lookSensitivity * Time.deltaTime;

        // up down
        xRotation -= lookY;
        xRotation = Math.Clamp(xRotation, -90f, 90f);

        // rotate camera around xAxis (look up & down)
        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // rotate player around yAxis (turn left & right)
        transform.Rotate(Vector3.up * lookX);
        
        
    }

    void Update()
    {
        //Debug.Log("PlayerMovement:" + inputReader.PlayerControlsStatus());  
        if (inputReader.PlayerControlsStatus())
        {
            Vector3 move = (_moveVector.y * transform.forward) + (_moveVector.x * transform.right);
            //Combine movement and gravity
            if (controller.isGrounded && _velocity.y < 0)
            {
                _velocity.y = -2f;
            }
            _velocity.y += gravity * Time.deltaTime;
            controller.Move(_velocity * Time.deltaTime);
            controller.Move(move * Time.deltaTime);
        }
        else
        {
            UnityEngine.Cursor.visible = true;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
        }
    }
}
