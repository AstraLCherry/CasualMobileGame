using UnityEngine;

public abstract class Hook : MonoBehaviour
{
    public abstract void Select();

    public abstract void Unselect();

    public abstract void Connect();

    public abstract void Disconnect();
}
