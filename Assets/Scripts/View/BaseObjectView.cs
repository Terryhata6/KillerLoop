using UnityEngine;


public class BaseObjectView : MonoBehaviour
{
    public Vector3 Position
    {
        get => transform.position;
        set => transform.position = value;
    }
    public Vector3 Forward => transform.forward;
    public Quaternion Rotation => transform.rotation;
}