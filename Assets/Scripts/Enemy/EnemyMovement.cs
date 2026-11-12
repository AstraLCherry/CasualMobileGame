using UnityEngine;

public class EnemyMovement : MonoBehaviour, IGrapable
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform orientation;
    [SerializeField] private float speed;

    [SerializeField] private LayerMask playerMask;
    [SerializeField] private float distance;

    private Rigidbody rb;

    private bool gotTarget;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        LookAtTarget();

        bool foundTarget = Physics.CheckSphere(transform.position, distance, playerMask);

        if(foundTarget)
        {
            gotTarget = true;
            rb.linearVelocity = Vector3.zero;
        }
        else
        {
            gotTarget = false;
        }
    }

    private void FixedUpdate()
    {
        if(!gotTarget) 
            rb.AddForce(orientation.forward * speed + (Vector3.up * -Physics.gravity.y), ForceMode.Force);
    }

    private void LookAtTarget()
    {
        orientation.LookAt(target);
    }

    public void OnHooked()
    {
        gotTarget = true;
        rb.linearVelocity = Vector3.zero;
    }

    public void OnUnhooked()
    {
        gotTarget = false;
    }
}
