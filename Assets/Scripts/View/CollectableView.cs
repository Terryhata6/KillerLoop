
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
    private float _speedBoostValue;
    private bool _triggered;
    private int _playerLayer;
    private float _defaultTriggerRadius;

    #endregion

    #region AccessFields

    public int Points => _points;

    public Transform Target => _target;

    public bool AtTarget => CheckAtTarget();

    #endregion

    private void Awake()
    {
        ActiveTrigger(true);
        _defaultTriggerRadius = _trigger?.radius ?? 2f;
    }

    #region PublicMethods

    #region IInitialize

    public override void Initialize()
    {
        base.Initialize();
        InitializeFields();
        PlayAnimation(true);
        ChangeTriggerRadius(_defaultTriggerRadius);
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

    public void ChangeTriggerRadius(float radius)
    {
        if (!_trigger)
        {
            return;
        }
        _trigger.radius = radius;
    }

    #endregion

    #region PrivateMethods

    private void InitializeFields()
    {
        gameObject.layer = 14;
        _isReadyForMove = true;
        _speedBoostValue = Time.deltaTime * 5f;
        _playerLayer = 7;
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
        _movingSpeed += _speedBoostValue;
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
        _triggered = true;
    }

    #endregion

    #region OnTriggerEnter

    private void OnTriggerStay(Collider other)
    {
        if (_triggered)
        {
            return;
        }
        if (other.gameObject.layer == _playerLayer)
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
