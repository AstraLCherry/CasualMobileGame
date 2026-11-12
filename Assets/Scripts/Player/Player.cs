using DG.Tweening;
using UnityEditor.Build.Profile;
using UnityEngine;

[RequireComponent (typeof(PlayerMovement))]
public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public PlayerComponents Components { get; private set; }

    public PlayerMovement Movement => Components.Movement;
    public PlayerInputManager InputManager => Components.InputManager;

    private void Awake()
    {
        Instance = this;
        Components = new(this);
        //Components.Grappling.Init(this);
        Movement.Init(this);
        Components.Face.Init(this);
        Components.Effect.Init(this);
    }

    private void Start()
    {
        InitGrappling();

        LevelSystem.Instance.OnWin += Instance_OnWin;
    }

    private void Instance_OnWin()
    {
        //Movement.KillTween();
        Components.Effect.DOKill();
    }

    private void Update()
    {
        HandleTouchMovement();

        Movement.UpdateAim();
        Movement.LimitPulling();
        Movement.UpdateLines();

        // Face
        Components.Face.LookAtCursor();
        Components.Face.LockRotation();

        // Effect
        Components.Effect.UpdateStaminaBar();
        //Components.Grappling.UpdateLines();
    }

    private void HandleTouchMovement()
    {
        // Movement
        if (GameManager.Instance.GetPlatform() == GameManager.Platform.PC)
        {
            if (InputManager.GetAttackInput())
            {
                Movement.Move(InputManager.GetMouseDelta());
                EventBus<OnPlayerUpdateAim>.Raise(new(this));
            }
        }
        else
        {
            if (InputManager.GetTouchInput())
            {
                //Movement.Move(InputManager.GetMouseDelta());
                EventBus<OnPlayerUpdateAim>.Raise(new(this));
            }
        }
    }

    private void LateUpdate()
    {
        Components.Effect.LockTail();

        Components.Face.LockRotation();
    }

    private void OnEnable()
    {
        InputManager.EnableInput();
    }

    private void OnDisable()
    {
        InputManager.DisableInput();
        //Components.Grappling.ClearAllListeners();
    }

    private void InitGrappling()
    {
        if (GameManager.Instance.GetPlatform() == GameManager.Platform.PC)
        {
            InputManager.AddAttackStartListener((context) =>
            {
                Movement.StartAim();
            });

            InputManager.AddAttackReleaseListener((context) =>
            {
                Movement.StopAim();
                //Components.Grappling.ReleaseAim();
            });
        }
        else
        {
            InputManager.AddTouchStartListener((context) =>
            {
                //Movement.StartAim();
                EventBus<OnPlayerStartAim>.Raise(new(this));
            });

            InputManager.AddTouchReleaseListener((context) =>
            {
                //Movement.StopAim();
                EventBus<OnPlayerReleaseAim>.Raise(new(this));
            });
        }

        Movement.OnDashStarted += Movement_OnDashStarted;
        Movement.OnDash += Movement_OnDash;
        Movement.OnDashFinished += Movement_OnDashFinished;
    }

    private void Movement_OnDashStarted()
    {
        Components.Effect.StartDashTrail();
    }

    private void Movement_OnDash(float x)
    {
        Components.Effect.UpdateTrail(x);
    }

    private void Movement_OnDashFinished()
    {
        Components.Effect.StopDashTrail();
    }
}

public class PlayerComponents
{
    public PlayerGrappling Grappling { get; private set; }
    public PlayerMovement Movement { get; private set; }
    public PlayerEffect Effect { get; private set; }
    public PlayerFace Face { get; private set; }

    public PlayerInputManager InputManager { get; private set; }

    public PlayerCollision Collision { get; private set; }

    public PlayerComponents(Player player)
    {
        Movement = player.GetComponent<PlayerMovement>();
        Grappling = player.GetComponent<PlayerGrappling>();

        Effect = player.GetComponent<PlayerEffect>();
        Face = player.GetComponent<PlayerFace>();

        Collision = player.GetComponent<PlayerCollision>();

        InputManager = new();
    }
}
