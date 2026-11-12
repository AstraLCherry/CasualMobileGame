using DG.Tweening;
using System.Collections;
using UnityEngine;

public class GrabableSphere : Hook
{
    [Header("Explosion")]
    [SerializeField] private float explosionForce;
    [SerializeField] private float explosionRange;

    [Header("Aim")]
    [SerializeField] private SpriteRenderer aim;
    [SerializeField] private float rotSpeed;

    [Header("Debris")]
    [SerializeField] private Rigidbody[] cellRbs;

    private Vector3[] debrisPos;
    private Quaternion[] debrisRot;
    
    private Collider objCollider;
    private Tween tween;

    private float restoreTime;

    public bool Destroyed { get; private set; }

    private bool isPlayerWon;

    private void Awake()
    {
        objCollider = GetComponent<Collider>();

        debrisPos = new Vector3[cellRbs.Length];
        debrisRot = new Quaternion[cellRbs.Length];

        // Save each debri's default position and rotation 
        for (int i = 0; i < cellRbs.Length; i++)
        {
            debrisPos[i] = cellRbs[i].transform.localPosition;
            debrisRot[i] = cellRbs[i].transform.localRotation;
        }
    }

    private void Start()
    {
        restoreTime = LevelSystem.Instance.GetLevel().hookRecoveryTime;

        // Animation thingy
        tween = aim.transform.DOScale(2, 0.1f);
        tween.SetLoops(-1, LoopType.Yoyo);

        LevelSystem.Instance.OnWin += Instance_OnWin;
    }

    private void Instance_OnWin()
    {
        isPlayerWon = true;
        tween.Kill();
    }

    private void Update()
    {
        if (Destroyed)
        {
            if (!isPlayerWon)
            {
                objCollider.enabled = false;

                // After the object is destroyed (not with Destroy() method), the restore timer starts, when it hits 0, the debris restore
                restoreTime -= Time.deltaTime;

                if (restoreTime < 0)
                    DebrisRestore();
            }
        }
        else
        {
            // If the player is dashing AND the object is not destroyed, make the object's collider a trigger
            if (Player.Instance.Components.Movement.GetState() == PlayerMovement.PlayerState.Dashing)
            {
                objCollider.isTrigger = true;
            }
            else
            {
                objCollider.isTrigger = false;
            }
        }

        aim.transform.Rotate(new Vector3(0, 0, rotSpeed));
    }

    public void DebrisExplostion()
    {
        LevelSystem.Instance.IncreaseDestroyedHookCount();

        objCollider.enabled = false;

        SoundManager.Instance.PlaySound(0, true);
        Destroyed = true;
        aim.color = new(aim.color.r, aim.color.g, aim.color.b, 0);

        for (int i = 0; i < cellRbs.Length; i++)
        {
            cellRbs[i].isKinematic = false;
            cellRbs[i].AddExplosionForce(explosionForce * Player.Instance.Components.Movement.GetRigidbody().linearVelocity.magnitude * Random.Range(0.5f, 1.5f),
                                 transform.position + -Player.Instance.transform.Find("Hook").forward, explosionRange);
        }
    }

    private void DebrisRestore()
    {
        LevelSystem.Instance.DecreaseDestroyedHookCount();

        objCollider.enabled = true;
        restoreTime = LevelSystem.Instance.GetLevel().hookRecoveryTime;

        StartCoroutine(RestoreOnePerDebri());
        Destroyed = false;
    }

    private IEnumerator RestoreOnePerDebri()
    {
        for (int i = 0; i < cellRbs.Length; i++)
        {
            cellRbs[i].isKinematic = true;

            // Smooth restoring animation
            Tween posTween = cellRbs[i].transform.DOLocalMove(debrisPos[i], 0.5f);
            Tween rotTween = cellRbs[i].transform.DOLocalRotateQuaternion(debrisRot[i], 0.5f);

            posTween.SetEase(Ease.InOutCirc);
            rotTween.SetEase(Ease.InOutCirc);

            yield return new WaitForSeconds(0.025f);
        }
    }

    //public bool IsRendererVisible(MeshRenderer meshRenderer)
    //{
    //    // This checks if the renderer's bounds are within the camera's view frustum
    //    Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
    //    return GeometryUtility.TestPlanesAABB(frustumPlanes, meshRenderer.bounds);
    //}

    public override void Select()
    {
        // Makes aim visible
        aim.color = new(aim.color.r, aim.color.g, aim.color.b, 1);
        tween.Play();
    }

    public override void Unselect()
    {
        // Makes aim invisible
        aim.color = new(aim.color.r, aim.color.g, aim.color.b, 0);
        tween.Pause();
    }

    public override void Connect()
    {
        Debug.Log($"{gameObject.name} got connected");
    }

    public override void Disconnect()
    {
        Debug.Log($"{gameObject.name} got disconnected");
    }
}
