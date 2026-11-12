using UnityEngine;

public class Borders : MonoBehaviour
{
    [SerializeField] private float addedHeight;

    [Header("Borders")]
    [SerializeField] private Transform top;
    [SerializeField] private Transform bottom;
    [SerializeField] private Transform left;
    [SerializeField] private Transform right;

    [Header("Other")]
    [SerializeField] private Portal portal;


    [ContextMenu("Set Borders in Camera Resolution")]
    private void SetBordersRes()
    {
        //Gets every vertices of the camera screen
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 bottomRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0));
        Vector3 topLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0));
        Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));

        float horizontalDistance = Vector3.Distance(topRight, topLeft);
        float verticalDistance = Vector3.Distance(bottomLeft, topLeft);

        //--------------------------------------------------------------------------------------------------------
        // Top
        top.position = new(top.position.x, topLeft.y, top.position.z);
        top.localScale = new(horizontalDistance, top.localScale.y, top.localScale.z);

        //--------------------------------------------------------------------------------------------------------
        // Bottom
        bottom.position = new(bottom.position.x, bottomLeft.y, bottom.position.z);
        bottom.localScale = new(horizontalDistance, bottom.localScale.y, bottom.localScale.z);

        //--------------------------------------------------------------------------------------------------------
        // Left
        left.position = new(bottomLeft.x, left.position.y, left.position.z);
        left.localScale = new(left.localScale.x, verticalDistance + addedHeight, left.localScale.z);

        //--------------------------------------------------------------------------------------------------------
        // Right
        right.position = new(bottomRight.x, right.position.y, right.position.z);
        right.localScale = new(right.localScale.x, verticalDistance + addedHeight, right.localScale.z);
    }
}
