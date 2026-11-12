using UnityEngine;

public abstract class PlayerBaseMovement : MonoBehaviour
{
    protected Player _player;

    public virtual void Init(Player player)
    {
        _player = player;

        EventBus<OnPlayerStartAim>.OnAction += EventBus_OnStartAim;
        EventBus<OnPlayerUpdateAim>.OnAction += EventBus_OnUpdateAim;
        EventBus<OnPlayerReleaseAim>.OnAction += EventBus_OnReleaseAim;
    }

    public virtual void ClearAllListeners()
    {
        EventBus<OnPlayerStartAim>.OnAction -= EventBus_OnStartAim;
        EventBus<OnPlayerUpdateAim>.OnAction -= EventBus_OnUpdateAim;
        EventBus<OnPlayerReleaseAim>.OnAction -= EventBus_OnReleaseAim;
    }

    private void EventBus_OnStartAim(OnPlayerStartAim obj)
    {
        StartAim();
    }

    private void EventBus_OnUpdateAim(OnPlayerUpdateAim obj)
    {
        UpdateAim();
    }

    private void EventBus_OnReleaseAim(OnPlayerReleaseAim obj)
    {
        ReleaseAim();
    }

    public abstract void StartAim();

    public abstract void UpdateAim();

    public abstract void ReleaseAim();

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

    public Vector3 GetAimDirection(Transform aim)
    {
        aim.LookAt(new Vector3(GetMousePos().x, GetMousePos().y, 0));

        return new Vector3(aim.forward.x, aim.forward.y, 0);
    }
}
