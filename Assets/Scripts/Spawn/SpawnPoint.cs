using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private void Start()
    {
        Player.Instance.transform.position = transform.position;
    }
}
