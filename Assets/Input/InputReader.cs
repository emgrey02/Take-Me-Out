using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;

[CreateAssetMenu(fileName = "New Input", menuName = "Input/InputReader")]
public class InputReader : ScriptableObject, IPlayerActions, IUIActions
{
    private Controls controls;

    public event Action<Vector2> MoveEvent;
    public event Action<Vector2> LookEvent;
    public event Action<bool> LeaveEvent;
    public event Action<bool> InteractEvent;
    public event Action<bool> InventoryToggleEvent;
    public event Action<bool> MainMenuToggleEvent;

    public void EnablePlayerControls()
    {
        controls.Player.Enable();
    }

    public void DisablePlayerControls()
    {
        controls.Player.Disable();
    }

    public bool PlayerControlsStatus()
    {
        return controls.Player.enabled;
    }

    public bool UIControlsStatus()
    {
        return controls.UI.enabled;
    }
    
    void OnEnable()
    {
        if (controls == null)
        {
            controls = new Controls();
            controls.Player.SetCallbacks(this);
            controls.UI.SetCallbacks(this);
        }

        controls.Player.Enable();
        controls.UI.Enable();
    }

    void OnDisable()
    {
        controls.Player.Disable();
        controls.UI.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        LookEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnLeave(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            LeaveEvent?.Invoke(true);
        }
        else if (context.canceled)
        {
            LeaveEvent?.Invoke(false);
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            InteractEvent?.Invoke(true);
        }
        else if (context.canceled)
        {
            InteractEvent?.Invoke(false);
        }
    }

    public void OnInventoryToggle(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            InventoryToggleEvent?.Invoke(true);
        }
        else if (context.canceled)
        {
            InventoryToggleEvent?.Invoke(false);
        }
    }

    public void OnMainMenuToggle(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            MainMenuToggleEvent?.Invoke(true);
        }
        else if (context.canceled)
        {
            MainMenuToggleEvent?.Invoke(false);
        }
    }

}
