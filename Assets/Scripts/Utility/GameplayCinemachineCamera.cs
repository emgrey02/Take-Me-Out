using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UIElements;

public class GameplayCinemachineCamera : MonoBehaviour
{
    [SerializeField] InputReader inputReader;
    private Vector3 _playerLook;
    private PlayerMovement _player;
    private float xRotation;

    void Awake()
    {
        inputReader.LookEvent += OnLook;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CinemachineCamera gameplayCamera = GetComponent<CinemachineCamera>();
        _player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();

        gameplayCamera.Follow = _player.transform;
        //gameplayCamera.LookAt = _player.transform;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnLook(Vector2 playerLook)
    {
        _playerLook = playerLook;

        // left right
        float lookY = _playerLook.y * (_player.LookSensitivity * .001f);

        // up down
        xRotation -= lookY;
        xRotation = Math.Clamp(xRotation, -90f, 90f);

        // rotate camera around xAxis (look up & down)
        // we're not rotating the player on the x axis, just the camera, so we can look up and down without affecting player movement direction
        // match player's rotation in the y axis (look left & right)
        transform.localRotation = Quaternion.Euler(xRotation, _player.transform.eulerAngles.y, 0f);
    }

    void OnDisable()
    {
        inputReader.LookEvent -= OnLook;
    }
}
