using UnityEngine;

public class Test : MonoBehaviour
{
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        // Perform the check every frame
        if (IsRendererVisible())
        {
            Debug.Log("visible");
        }
        else
        {
            Debug.Log("NOT visible");
        }
    }

    private bool IsRendererVisible()
    {
        // This checks if the renderer's bounds are within the camera's view frustum
        Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        return GeometryUtility.TestPlanesAABB(frustumPlanes, meshRenderer.bounds);
    }
}
