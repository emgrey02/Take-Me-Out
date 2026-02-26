using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    
    [SerializeField] InputReader inputReader;
    
    [Header("Player Movement Settings")]
    public float moveSpeed = 5f;
    public float gravity = -9.81f;
    private CharacterController controller;
    private Vector3 _velocity;
    private Vector3 _moveVector;
    
    [Header("Menu Game Objects")]
    public GameObject InventoryMenu;
    public GameObject MainMenu;

    private VisualElement invPanel;
    private VisualElement mmPanel;

    [Header("Mouse Look Settings")]
    [Tooltip("Sensitivity of mouse movement")]
    public float lookSensitivity = 100f;
    // track's camera's x-axis rotation
    private float xRotation = 0f;

    [Tooltip("Camera component attached to player")]
    public Camera cam;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        inputReader.MoveEvent += OnMove;
        inputReader.LookEvent += OnLook;

        invPanel = InventoryMenu.GetComponent<UIDocument>().rootVisualElement;
        mmPanel = MainMenu.GetComponent<UIDocument>().rootVisualElement;
    }

    private void OnMove(Vector2 movement)
    {
        if (invPanel.ClassListContains("hide") && mmPanel.ClassListContains("hide")) {

            _moveVector = movement * moveSpeed; 
        }
    }
    
    private void OnLook(Vector2 playerLook)
    {
        if (invPanel.ClassListContains("hide") && mmPanel.ClassListContains("hide")) {
            #if UNITY_STANDALONE
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
                UnityEngine.Cursor.visible = false;
            #endif
            #if UNITY_EDITOR
                UnityEngine.Cursor.lockState = CursorLockMode.Confined;
                UnityEngine.Cursor.visible = false;
            #endif

            float lookX = playerLook.x * lookSensitivity * Time.deltaTime;
            float lookY = playerLook.y * lookSensitivity * Time.deltaTime;

            xRotation -= lookY;
            xRotation = Math.Clamp(xRotation, -90f, 90f);

            // rotate camera around xAxis (look up & down)
            Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            // rotate player around yAxis (turn left & right)
            transform.Rotate(Vector3.up * lookX);
        }
        else {
            UnityEngine.Cursor.visible = true;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
        }
    }

    void Update()
    {
        Debug.Log("PlayerMovement:" + inputReader.PlayerControlsStatus());  
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
    }
}
