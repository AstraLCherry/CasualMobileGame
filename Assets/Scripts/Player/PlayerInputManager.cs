using System;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerInputManager
{
    private readonly InputSystem_Actions actions;

    private readonly TouchScreenInput screenInput;
    public event Action<Vector2> OnScreenTouch;

    public PlayerInputManager()
    {
        actions = new InputSystem_Actions();
        screenInput = new(actions);
        screenInput.OnTouchDown += ScreenInput_OnTouchDown;
    }

    private void ScreenInput_OnTouchDown(Vector2 obj)
    {
        OnScreenTouch.Invoke(obj);
    }

    public void EnableInput()
    {
        actions.Enable();
    }
    public void DisableInput()
    {
        actions.Disable();
    }

    public void AddJumpListener(Action<CallbackContext> action)
    {
        actions.Player.Jump.started += action;
    }

    public bool AttackPressed()
    {
        return actions.Player.Attack.WasPressedThisFrame();
    }
    
    public void AddAttackStartListener(Action<CallbackContext> action)
    {
        actions.Player.Attack.started += action;
    }

    public void AddAttackReleaseListener(Action<CallbackContext> action)
    {
        actions.Player.Attack.canceled += action;
    }

    public void AddTouchStartListener(Action<CallbackContext> action)
    {
        actions.Player.TouchPress.started += action;
    }

    public void AddTouchReleaseListener(Action<CallbackContext> action)
    {
        actions.Player.TouchPress.canceled += action;
    }

    public Vector2 GetMousePos()
    {
        return actions.Player.MousePos.ReadValue<Vector2>();
    }

    public Vector2 GetTouchPos()
    {
        return actions.Player.TouchPos.ReadValue<Vector2>();
    }


    public Vector2 GetMouseDelta()
    {
        return actions.Player.Look.ReadValue<Vector2>();
    }

    public bool GetAttackInput()
    {
        return actions.Player.Attack.IsPressed();
    }

    public bool GetTouchInput()
    {
        return actions.Player.TouchPress.IsPressed();
    }

    public void AddAttackPressListener(Action<CallbackContext> action)
    {
        actions.Player.Attack.performed += action;
    }
}
