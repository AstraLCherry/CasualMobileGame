using DG.Tweening;
using System;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public enum PlayerState
    {
        Free,
        Hooked,
        Dashing
    }

    public event Action OnDashStarted;
    public event Action<float> OnDash;
    public event Action OnDashFinished;

    [SerializeField] private PlayerState state;
    [SerializeField] private LayerMask grappableMask;

    [Header("Aiming")]
    [SerializeField] private Transform aim;
    [SerializeField] private LineRenderer aimLine;
    [SerializeField] private float maxAimDistance;

    [Header("Grappling")]
    [SerializeField] private Transform hook;
    [SerializeField] private LineRenderer hookLine;
    [SerializeField] private float maxPullDistance;

    [Header("Movement")]
    [SerializeField] private float moveForce;
    [SerializeField] private float moveAirResist;

    [Header("Dash")]
    [SerializeField] private float maxDashTime;
    [SerializeField] private float dashForce;

    [Header("Stamina")]
    [SerializeField] private float maxStamina;
    [SerializeField] private float staminaDrainSpeed;

    private Player _player;

    private Rigidbody rb;

    private SpringJoint springJoint;

    private Tween dashTween;
    private Transform hookedTarget;

    private float aimDistance;
    private float acceleratedDashForce;
    private float dashTime;
    private float airResistance;

    private float defaultSpring;
    private float defaultDamper;
    private float defAirResist;

    public float Stamina { get; private set; }

    private bool aiming;

    public void Init(Player player)
    {
        _player = player;

        rb = GetComponent<Rigidbody>();
        springJoint = GetComponent<SpringJoint>();

        SetParamsToDefault();

        ResetSpring();
    }

    private void SetParamsToDefault()
    {
        defaultSpring = springJoint.spring;
        defaultDamper = springJoint.damper;

        defAirResist = rb.linearDamping;
        rb.linearDamping = defAirResist;

        Stamina = maxStamina;
    }

    #region Aim Methods
    public void StartAim()
    {
        if(state != PlayerState.Hooked)
        {
            Time.timeScale = 0.1f;
            aimDistance = maxAimDistance;
        }

        if (state == PlayerState.Hooked)
            airResistance = moveAirResist;

        if (state == PlayerState.Dashing)
            StopDash();

        dashTime = maxDashTime;
        aiming = true;
    }

    public void UpdateAim()
    {
        if(state == PlayerState.Hooked && hookedTarget != null)
            Hook(hookedTarget.position);

        if (aiming)
        {
            if (state != PlayerState.Hooked)
            {
                Stamina = Mathf.Clamp(Stamina -= staminaDrainSpeed * Time.deltaTime, 0, maxStamina);

                if (Physics.Raycast(aim.position, aim.forward, out RaycastHit hitInfo, maxAimDistance, grappableMask))
                {
                    Hook grapable = hitInfo.transform.GetComponent<Hook>();
                    grapable.Select();

                    hookedTarget = grapable.transform;
                }
                else
                {
                    if (hookedTarget != null)
                    {
                        Hook grapable = hookedTarget.transform.GetComponent<Hook>();
                        grapable.Unselect();
                    }
                }
            }
        }
        else
        {
            if (state != PlayerState.Hooked)
            {
                Stamina = Mathf.Clamp(Stamina += Time.deltaTime, 0, maxStamina);
            }
        }

        if(Stamina == 0)
        {
            StopAim(); 
        }
    }

    public void StopAim()
    {
        aimDistance = 0;
        Time.timeScale = 1f;
        aiming = false; 

        if (state != PlayerState.Hooked)
        {
            if (Physics.Raycast(aim.position, aim.forward, out RaycastHit hitInfo, maxAimDistance, grappableMask))
            {
                Hook grapable = hitInfo.transform.GetComponent<Hook>();
                grapable.Connect();
                grapable.Unselect();

                hookedTarget = hitInfo.transform;

                state = PlayerState.Hooked;
            }
        }
        else if(state == PlayerState.Hooked)
        {
            Hook grapable = hookedTarget.transform.GetComponent<Hook>();
            grapable.Disconnect();

            hookedTarget = null;
            Dash();
        }

        rb.linearDamping = airResistance;
    }

    #endregion

    private void Hook(Vector3 targetPos)
    {
        springJoint.connectedAnchor = targetPos;

        springJoint.spring = defaultSpring;
        springJoint.damper = defaultDamper;

        hookLine.SetPosition(1, targetPos);
    }

    public void Move(Vector2 delta)
    {
        if (state != PlayerState.Hooked) return;

        Debug.Log(delta);
        if (GameManager.Instance.GetPlatform() == GameManager.Platform.PC)
            rb.AddForce(delta * moveForce, ForceMode.Acceleration);
        else
            rb.AddForce(moveForce * GetAimDirection(), ForceMode.Force);
    }

    public void UpdateLines()
    {
        aimLine.SetPosition(0, transform.position);
        aimLine.SetPosition(1, transform.position + GetAimDirection() * aimDistance);

        hookLine.SetPosition(0, transform.position);

        if (state == PlayerState.Free || state == PlayerState.Dashing)
            hookLine.SetPosition(1, transform.position);
    }

    private void Dash()
    {
        state = PlayerState.Dashing;
        OnDashStarted?.Invoke();

        rb.angularVelocity = Vector3.zero;
        rb.linearVelocity = Vector3.zero;
        rb.freezeRotation = true;
        airResistance = defAirResist;

        rb.AddForce(acceleratedDashForce * dashForce * GetHookDirection(), ForceMode.Impulse);
        ResetSpring();

        //Timer
        dashTween = DOTween.To(() => dashTime, (x) => 
        {
            dashTime = x;
            OnDash?.Invoke(x);
        }, 0, maxDashTime);

        dashTween.OnComplete(StopDash);

        if(dashTween.IsComplete())
            dashTween.Restart();
    }

    private void StopDash()
    {
        state = PlayerState.Free;
        dashTween.Pause();
        OnDashFinished?.Invoke();
    }

    private void ResetSpring()
    {
        springJoint.connectedAnchor = Vector3.zero;
        springJoint.spring = 0;
        springJoint.damper = 0;

        airResistance = defAirResist;
        rb.angularDamping = 0;
        springJoint.damper = defaultDamper;
    }

    public void LimitPulling()
    {
        if(state != PlayerState.Hooked) return;

        float distance = Vector3.Distance(transform.position, springJoint.connectedAnchor);

        if (distance > maxPullDistance)
        {
            rb.linearDamping = distance;
            rb.angularDamping = distance;
            springJoint.damper = distance;
            rb.AddForce(distance * moveForce * GetHookDirection(), ForceMode.Force);
        }
        else
        {
            rb.linearDamping = airResistance;
            rb.angularDamping = 0;
            springJoint.damper = defaultDamper;
        }

        acceleratedDashForce = Mathf.Clamp(distance, 1, 5);
    }

    public Vector3 GetMousePos()
    {
        Vector3 mousePos;
        if (GameManager.Instance.GetPlatform() == GameManager.Platform.PC)
            mousePos = _player.Components.InputManager.GetMousePos();
        else
            mousePos = _player.Components.InputManager.GetTouchPos();

        mousePos.z = Camera.main.farClipPlane;

        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);

        return worldMousePos;
    }

    public Vector3 GetAimDirection()
    {
        aim.LookAt(new Vector3(GetMousePos().x, GetMousePos().y, 0));

        return new Vector3(aim.forward.x, aim.forward.y, 0);
    }

    public Vector3 GetHookDirection()
    {
        hook.LookAt(springJoint.connectedAnchor);

        return new Vector3(hook.forward.x, hook.forward.y, 0);
    }

    public Rigidbody GetRigidbody()
    {
        return rb;
    }

    public PlayerState GetState()
    {
        return state;
    }

    public float GetMaxStamina() 
    {
        return maxStamina;
    }

    public void KillTween()
    {
        dashTween?.Pause();
        dashTween?.Kill();
    }
}
