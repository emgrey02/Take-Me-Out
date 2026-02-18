using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class FirstPersonCamera : MonoBehaviour
{
    public InputAction lookAction; //Vector2
    
    public GameObject InventoryMenu;
    public GameObject MainMenu;
    
    [Header("Mouse Look Settings")]
    [Tooltip("Sensitivity of mouse movement")]
    public float lookSensitivity = 100f;

    [Tooltip("The player's body transform for rotation'")]
    public Transform playerTransform;

    // track's camera's x-axis rotation
    private float xRotation = 0f;

    [Tooltip("Camera component attached to player")]
    public Camera cam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lookAction = InputSystem.actions.FindAction("Look");
        
        #if UNITY_STANDALONE
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        #endif
        #if UNITY_EDITOR
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
        #endif
        

    }

    // Update is called once per frame
    void Update()
    {
            #if UNITY_STANDALONE
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            #endif
            #if UNITY_EDITOR
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = false;
            #endif

            Vector2 playerLook = lookAction.ReadValue<Vector2>();
            float lookX = playerLook.x * lookSensitivity * Time.deltaTime;
            float lookY = playerLook.y * lookSensitivity * Time.deltaTime;

            xRotation -= lookY;
            xRotation = Math.Clamp(xRotation, -90f, 90f);

            // rotate camera around xAxis (look up & down)
            cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            // rotate player around yAxis (turn left & right)
            playerTransform.Rotate(Vector3.up * lookX);
        
        //Cursor.visible = true;
        //Cursor.lockState = CursorLockMode.None;
        
    }
}
