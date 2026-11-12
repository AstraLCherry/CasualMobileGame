using DG.Tweening;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private GameObject body;

    private new Collider collider;
    private Tween tween;

    public void Start()
    {
        collider = GetComponent<Collider>();
        LevelSystem.Instance.OnWin += Instance_OnWin;
        Player.Instance.Components.Collision.OnEnterPortal += Collision_OnEnterPortal;
    }

    private void Collision_OnEnterPortal()
    {
        tween?.Kill();
    }

    private void Instance_OnWin()
    {
        collider.enabled = true;
        body.SetActive(true);
        Invoke(nameof(WinAnimation), 1);
    }

    private void WinAnimation()
    {
        tween = Player.Instance.Movement.GetRigidbody().DOMove(transform.position, 1);
        tween.SetEase(Ease.InOutCubic);
    }
}
