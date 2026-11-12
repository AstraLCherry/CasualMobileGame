using UnityEngine;
using static PlayerMovement;

public class PlayerGrappling : PlayerBaseMovement
{
    [SerializeField] private LayerMask grappableMask;

    [Header("Aiming")]
    [SerializeField] private Transform aim;
    [SerializeField] private LineRenderer aimLine;
    [SerializeField] private float maxAimDistance;

    private float aimDistance;

    [Header("Grappling")]
    [SerializeField] private Transform hook;
    [SerializeField] private LineRenderer hookLine;
    [SerializeField] private float maxPullDistance;

    private SpringJoint springJoint;

    private Transform hookedTarget;

    private bool gotTarget;

    public override void StartAim()
    {
        Time.timeScale = 0.1f;
    }

    public override void UpdateAim()
    {
        //VisualizeAimLine();
        UpdateLines();
    }

    public override void ReleaseAim()
    {
        if (Physics.Raycast(aim.position, aim.forward, out RaycastHit hitInfo, maxAimDistance, grappableMask))
        {
            //Hook grapable = hitInfo.transform.GetComponent<Hook>();
            //grapable.Connect();
            //grapable.Unselect();

            Debug.Log(hitInfo.transform);
            hookedTarget = hitInfo.transform;
            Hook(hitInfo.transform.position);
        }
    }

    private void Hook(Vector3 targetPos)
    {
        springJoint.connectedAnchor = targetPos;

        //springJoint.spring = defaultSpring;
        //springJoint.damper = defaultDamper;

        hookLine.SetPosition(1, targetPos);
    }

    public void VisualizeAimLine()
    {
        aimLine.SetPosition(0, transform.position);
        aimLine.SetPosition(1, transform.position + GetAimDirection(aim) * aimDistance);
    }

    public void UpdateLines()
    {
        hookLine.SetPosition(0, transform.position);

        if (hookedTarget == null)
        {
            hookLine.SetPosition(1, transform.position);
            Debug.Log("b");
        }
        else
        {
            hookLine.SetPosition(1, hookedTarget.position);
            Debug.Log("a");
        }

        
    }
}
