using System;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class TouchScreenInput
{
    public event Action<Vector2> OnTouchDown;

    private Finger lookFinger;

    private readonly InputSystem_Actions actions;

    private Vector2 previousPos;
    private Vector2 delta;

    public TouchScreenInput(InputSystem_Actions actions)
    {
        this.actions = actions;

        this.actions.Player.TouchPress.started += TouchPress_started;
        this.actions.Player.TouchPress.canceled += TouchPress_canceled;
    }

    private void TouchPress_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        EnhancedTouchSupport.Enable();

        Touch.onFingerMove += Touch_onFingerMove;
        Touch.onFingerUp += Touch_onFingerUp;
    }

    private void Touch_onFingerMove(Finger obj)
    {
        if (lookFinger == null) return;

        if (lookFinger.index == obj.index)
        {
            delta = GetDelta(lookFinger);

            OnTouchDown?.Invoke(delta);
        }
    }

    private void Touch_onFingerUp(Finger obj)
    {
        if (lookFinger == obj)
        {
            lookFinger = null;
        }
    }

    private void TouchPress_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Touch.onFingerMove -= Touch_onFingerMove;
        Touch.onFingerUp -= Touch_onFingerUp;
        lookFinger = null;

        EnhancedTouchSupport.Disable();
    }

    /// <summary>
    /// Manually creates delta. because Unity still won't fix bugs >:(
    /// </summary>
    /// <param name="finger"></param>
    /// <returns></returns>
    private Vector2 GetDelta(Finger finger)
    {
        Vector2 currentPos = finger.screenPosition;
        Vector2 prevPos = previousPos;

        Vector2 delta = currentPos - prevPos;

        previousPos = currentPos;

        return delta;
    }
}
