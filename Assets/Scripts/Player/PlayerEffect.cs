using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEffect : MonoBehaviour
{
    [SerializeField] private TrailRenderer dashTrail;
    [SerializeField] private float dashTrailTime;
    [SerializeField] private float dashRadius;

    [Header("UI")]
    [SerializeField] private Slider staminaSlider;

    private Player _player;

    public void Init(Player player)
    {
        _player = player;
        staminaSlider.maxValue = _player.Components.Movement.GetMaxStamina();
    }

    public void UpdateStaminaBar()
    {
        staminaSlider.value = _player.Components.Movement.Stamina;
    }

    public void StartDashTrail()
    {
        dashTrail.numCapVertices = 5;
        dashTrail.time = dashTrailTime;
    }

    public void UpdateTrail(float time)
    {
        dashTrail.time = time;
    }

    public void StopDashTrail()
    {
        Tween tween = dashTrail.DOTime(0, 0.5f);
        tween.OnComplete(() => { dashTrail.numCapVertices = 0; });
        //dashEffectMaterial.SetFloat("_Radius", 20);
    }

    public void LockTail()
    {
        dashTrail.transform.localPosition = Vector3.zero;
    }
}
