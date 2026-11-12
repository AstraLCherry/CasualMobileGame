using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour
{
    public event Action OnEnterPortal;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out GrabableSphere hook))
            hook.DebrisExplostion();

        if (other.CompareTag("Portal"))
        {
            // WIN YEY YAY YAY AYA
            OnEnterPortal?.Invoke();
            SceneManager.LoadScene(0);
        }
    }
}
