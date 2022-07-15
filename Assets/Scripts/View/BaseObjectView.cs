using UnityEngine;

public class BaseObjectView : MonoBehaviour, IInitialize
{
    #region AccessFields

    public Vector3 Position => transform.position;
    public Vector3 Forward => transform.forward;
    public Quaternion Rotation => transform.rotation;

    #endregion

    #region ProtectedFields

    protected Transform _transform;

    #endregion

    public virtual void Initialize()
    {
        _transform = transform;
    }
}