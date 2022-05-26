using UnityEngine;


public class BaseObjectView : MonoBehaviour
{
    public Vector3 Position => transform.position;
    public Vector3 Forward => transform.forward;
    public Quaternion Rotation => transform.rotation;
}