
using UnityEngine;

public class CollectableView : BaseObjectView
{
    #region PrivateFields

    #region Serialized

    [SerializeField] private int _points;
    [SerializeField] private float _movingSpeed;

    #endregion

    private Transform _target;

    #endregion

    #region AccessFields

    public int Points => _points;

    public Transform Target => _target;

    public bool AtTarget => CheckAtTarget();

    #endregion

    #region PublicMethods

    #region IInitialize
    public override void Initialize()
    {
        base.Initialize();
        gameObject.layer = 14;
    }

    #endregion

    #region CollectableManage

    public void MoveToTarget()
    {
        _transform.position =
            Vector3.MoveTowards(Position,
            Target.position,
            Time.deltaTime * _movingSpeed);
    }

    public void Collected()
    {
        gameObject.SetActive(false);
    }

    #endregion

    #endregion

    #region PrivateMethods

    private bool CheckAtTarget()
    {
        if (_target == null)
        {
            return false;
        }
        return (Position - Target.position).magnitude <= 0.5f;
    }

    #endregion

    #region OnTriggerEnter

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(7))
        {
            _target = other.transform; // когда игрок приближается к коину, он становится целью для коина, к которой он летит
        }
    }

    #endregion

    private void OnDisable()
    {
     //   GameEvents.Current.CollectableDisable(this);
    }  
}
