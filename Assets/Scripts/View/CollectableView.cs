
using UnityEngine;

public class CollectableView : BaseObjectView
{
    #region PrivateFields

    #region Serialized

    [SerializeField] private int _points;
    [SerializeField] private float _movingSpeed;
    [SerializeField] private SphereCollider _trigger;
    [SerializeField] private Animation _spinningAnimation;

    #endregion

    private Transform _target;
    private bool _isReadyForMove;
    private string _tag;
    private float _speedBoostValue;

    #endregion

    #region AccessFields

    public int Points => _points;

    public Transform Target => _target;

    public bool AtTarget => CheckAtTarget();

    #endregion

    private void Awake()
    {
        ActiveTrigger(true);
    }

    #region PublicMethods

    #region IInitialize

    public override void Initialize()
    {
        base.Initialize();
        InitializeFields();
        PlayAnimation(true);
    }

    #endregion

    #region CollectableManage

    public void MoveToTarget()
    {
        if (!_isReadyForMove)
        {
            return;
        }
        _transform.position =
            Vector3.MoveTowards(Position,
            Target.position,
            Time.deltaTime * _movingSpeed);
        IncreaseSpeed();
    }

    public void Collected()
    {
        gameObject.SetActive(false);
    }

    #endregion

    #endregion

    #region PrivateMethods

    private void InitializeFields()
    {
        gameObject.layer = 14;
        _isReadyForMove = true;
        _tag = "Player";
        _speedBoostValue = Time.deltaTime * 10f;
    }

    private void PlayAnimation(bool value)
    {
        if (!_spinningAnimation)
        {
            return;
        }
        if (value)
        {
            _spinningAnimation.Play();
            return;
        }
        _spinningAnimation.Stop();
    }

    private void IncreaseSpeed()
    {
        _movingSpeed += Time.deltaTime;
    }

    private void ChangeTriggerRadius(float radius)
    {
        if (!_trigger)
        {
            return;
        }
        _trigger.radius = radius;
    }

    private bool CheckAtTarget()
    {
        if (_target == null)
        {
            return false;
        }
        return (Position - Target.position).magnitude <= 0.5f;
    }

    private void ActiveTrigger(bool value)
    {
        if (!_trigger)
        {
            return;
        }
        _trigger.enabled = value;
    }

    private void Triggered(Transform other)
    {
        ActiveTrigger(false);
        GameEvents.Current.CollectableTriggered(this);
        _target = other;
    }

    #endregion

    #region OnTriggerEnter

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Untagged")
        {
            return;
        }
        if (other.CompareTag(_tag))
        {
            Triggered(other.transform); // когда игрок приближается к коину, он становится целью для коина, к которой он летит
        }
    }

    #endregion

    private void OnDisable()
    {
     //   GameEvents.Current.CollectableDisable(this);
    }  
}
