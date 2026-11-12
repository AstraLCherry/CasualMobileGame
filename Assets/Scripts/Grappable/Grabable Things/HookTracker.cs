using UnityEngine;

public class HookTracker : MonoBehaviour
{
    private GrabableSphere[] hooks;

    private void Start()
    {
        hooks = FindObjectsByType<GrabableSphere>(FindObjectsInactive.Exclude, FindObjectsSortMode.InstanceID);
    }
}
