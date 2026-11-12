using UnityEngine;

public class PlayerFace : MonoBehaviour
{
    [SerializeField] private Transform aim;
    [SerializeField] private Transform face;
    [SerializeField] private float maxDistance;

    private Player _player;

    private bool dashed;

    public void Init(Player player)
    {
        _player = player;
        _player.Components.Movement.OnDashStarted += Movement_OnDashStarted;
        _player.Components.Movement.OnDashFinished += Movement_OnDashFinished;
    }

    private void Movement_OnDashStarted()
    {
        dashed = true;
        face.position = _player.transform.position + new Vector3(_player.Components.Movement.GetHookDirection().x, 
                                                                      _player.Components.Movement.GetHookDirection().y, -2.5f) * maxDistance;
    }

    private void Movement_OnDashFinished()
    {
        dashed = false;
    }

    public void LockRotation()
    {
        Quaternion quaternion = Quaternion.identity;
        face.rotation = quaternion;
        //face.localEulerAngles = Vector3.zero;
    }

    public void LookAtCursor()
    {
        if(!dashed)
            face.position = _player.transform.position + new Vector3(aim.forward.x, aim.forward.y, -2.5f) * maxDistance;
    }
}
