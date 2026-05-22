using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

// handles player movement and mouse look, listens for move and look input events from InputReader
public class PlayerMovement : MonoBehaviour
{
    
    [SerializeField] InputReader inputReader;
    
    [Header("Player Movement Settings")]
    [SerializeField]
    private int _moveSpeed;
    public int MoveSpeed
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
    private Vector3 _playerLook;

    [Header("Mouse Look Settings")]
    [Tooltip("Sensitivity of mouse movement")]
    [SerializeField]
    private int _lookSensitivity;
    public int LookSensitivity 
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
        _playerLook = playerLook;

        // up down
        float lookX = _playerLook.x * (_lookSensitivity * .001f);

        // left right
        float lookY = _playerLook.y * (_lookSensitivity * .001f);

        // up down
        xRotation -= lookY;
        xRotation = Math.Clamp(xRotation, -90f, 90f);

        // rotate camera around xAxis (look up & down)
        // we're not rotating the player on the x axis, just the camera, so we can look up and down without affecting player movement direction
        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // rotate player around yAxis (turn left & right)
        // camera follows player rotation on y axis, so we only need to rotate the player and the camera will follow
        transform.Rotate(0, lookX, 0);

        
    }

    void Update()
    {
        //Debug.Log("PlayerMovement:" + inputReader.PlayerControlsStatus());  
        if (inputReader.PlayerControlsStatus())
        {
             #if UNITY_STANDALONE
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
                UnityEngine.Cursor.visible = false;
            #endif
            #if UNITY_EDITOR
                UnityEngine.Cursor.lockState = CursorLockMode.Confined;
                UnityEngine.Cursor.visible = false;
            #endif

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
