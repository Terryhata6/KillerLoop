
using System.Collections;
using Dreamteck.Splines;
using UnityEngine;

public class EnemyView : BaseObjectView
{
    #region PrivateFields

    #region Serialized

    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _baseMovementSpeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] protected SplineTracer _splineTracer;
    [SerializeField] private float _flyAwayPower;
    [SerializeField] private EnemyState _state = EnemyState.Idle;
    [SerializeField] private RoadRunSave _roadRunSave;
    [SerializeField] private int _savePointCounter;

    #endregion

    private float _movingBlend = 0;
    private bool _canMove;
    private float _baseY;
    private float _x;
    private Vector3 _tempVector;
    private float _tempMagnitude;
    private RaycastHit _hit;
    private float _movementSpeed;
    private float _timer;
    private float _timeToDead;
    private Collider _tempCollider;
    private Vector3 _nextPosition;

    #endregion

    #region AccessFields

    public RaycastHit Hit => _hit;
    public EnemyState State => _state;
    public float BaseMovementSpeed => _baseMovementSpeed;

    #endregion

    private void Awake()
    {
        _state = EnemyState.Inactive;
    }

    #region EnemyManage

    public void Rotate(Quaternion rotation)
    {
        _transform.rotation = rotation;
    }

    public virtual void MoveToNextPoint(float speed)
    {
        if (!_canMove)
        {
            return;
        }
        _transform.position = Vector3.MoveTowards(Position, _nextPosition, speed);
    }

    public virtual void Dead()
    {
        Debug.Log("EnemyDead");
        GameEvents.Current.EnemyDead(this);
        ChangeActionState(EnemyState.Inactive);
        gameObject.layer = 9;
    }

    public void DeadWithRagdoll()
    {
        Dead();
        StartCoroutine(DeadAnimation());
    }

    public virtual void Flying()
    {
        _tempVector.y = (-(_jumpForce - 0.2f) * ((_x) * (_x)) + _jumpForce) + _baseY;
        _tempVector.x = Position.x;
        _tempVector.z = Position.z;
        _transform.position = _tempVector;
        _x += Time.deltaTime * 1.3f;
    }

    public void PushAway(Vector3 flyAwayDirection)
    {
        //FlyAway(flyAwayDirection);
        StartCoroutine(DeadAnimation());
    }

    private void FlyAway(Vector3 dir)
    {
        if (_rigidbody)
        {
            _rigidbody.AddForce(dir * _flyAwayPower, ForceMode.Impulse);
        }
    }

    public void CheckForAWall()
    {
        _tempVector.x = Forward.z;
        _tempVector.z = -Forward.x;
        _tempVector.y = 0f;
        RayCastCheck(Position + (_tempVector + Vector3.up) * 0.5f,
            Forward.normalized + Vector3.up,
            1f,
            1 << 11);
        RayCastCheck(Position - (_tempVector - Vector3.up) * 0.5f,
            Forward.normalized + Vector3.up,
            1f,
            1 << 11);
    }

    #endregion

    #region Actions

    public void ChangeActionState(EnemyState state)
    {
        StopCurrentAction();
        _state = state;
    }

    public void StartStateAction(EnemyState state)
    {
        switch (state)
        {
            case EnemyState.Idle:
                {
                    Stand();
                    break;
                }
            case EnemyState.Move:
                {
                    Run();
                    break;
                }
            case EnemyState.Jump:
                {
                    Jump();
                    break;
                }
            case EnemyState.Slide:
                {
                    Slide();
                    break;
                }
            case EnemyState.WallRun:
                {
                    CheckForAWall();
                    WallRun();
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    private void StopCurrentAction()
    {
        switch (_state)
        {
            case EnemyState.Jump:
                {
                    Land();
                    break;
                }
            case EnemyState.Move:
                {
                    StopRun();
                    break;
                }
            case EnemyState.Idle:
                {
                    StopRun();
                    break;
                }
            case EnemyState.Slide:
                {
                    EndSlide();
                    break;
                }
            case EnemyState.WallRun:
                {
                    StopWallRun();
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    #region StartAction

    public void WallRun()
    {
        SetRigidbodyValues(false, false);
        SetAnimatorBool("WallRun", true);
        ChangeActionState(EnemyState.WallRun);
    }

    public void Run()
    {
        SetAnimatorBool("Run", true);
        SetRigidbodyValues(false, false);
        ChangeActionState(EnemyState.Move);
    }

    public void Slide()
    {
        SetRigidbodyValues(false, true);
        ChangeActionState(EnemyState.Slide);
    }

    public void Stand()
    {
        SetAnimatorBool("Run", true);
        SetRigidbodyValues(false, false);
        ChangeActionState(EnemyState.Idle);
    }

    public void Jump()
    {
        SetAnimatorBool("Jump", true);
        SetRigidbodyValues(false, false);
        _baseY = Position.y;
        _x = -1f;
        ChangeActionState(EnemyState.Jump);
    }

    public void EndSlide()
    {
        ChangeActionState(EnemyState.Idle);
        SetRigidbodyValues(false, false);
    }

    #endregion

    #region StopAction

    private void Land()
    {
        SetAnimatorBool("Jump", false);
    }

    private void StopWallRun()
    {
        SetAnimatorBool("WallRun", false);
    }

    private void StopRun()
    {
        SetAnimatorBool("Run", false);
    }

    #endregion

    #endregion

    #region RoadPointsUpdate

    public void UpdateRoadPoint()
    {
        if (_savePointCounter >= _roadRunSave.Points.Count)
        {
            _canMove = false;
            return;
        }
        if ((Position - _nextPosition).magnitude <= 0.07f)
        {
            UpdateState();
            UpdateRotation();
            UpdateNextPosition();
            _savePointCounter++;
        }

        _nextPosition.y = Position.y;
    }

    private void UpdateRotation()
    {
        if (_savePointCounter > 0)
        {
            _transform.rotation = _roadRunSave.Points[_savePointCounter].Rotation;
        }
    }

    private void UpdateNextPosition()
    {
        if (_savePointCounter + 1 < _roadRunSave.Points.Count)
        {
            _nextPosition = _roadRunSave.Points[_savePointCounter + 1].Position;
        }
    }

    private void UpdateState()
    {
        if (_roadRunSave.Points[_savePointCounter].State == EnemyState.Inactive)
        {
            return;
        }
        if (State == EnemyState.Jump
                && _roadRunSave.Points[_savePointCounter].State == EnemyState.Move)
        {
            return;
        }
        StartStateAction(_roadRunSave.Points[_savePointCounter].State);

    }

    private void FindNearestRoadPoint()
    {
        if (!_canMove)
        {
            return;
        }
        for (int i = 0; i < _roadRunSave.Points.Count; i++)
        {
            if ((_roadRunSave.Points[i].Position - Position).magnitude <= _tempMagnitude)
            {
                _tempMagnitude = (_roadRunSave.Points[i].Position - Position).magnitude;
                _savePointCounter = i;
            }
        }
        _nextPosition = _roadRunSave.Points[_savePointCounter].Position;
    }

    #endregion

    #region PublicOptionsMethods

    public void Initialize(RoadRunSave roadRunSave)
    {
        SetRoadRunSave(roadRunSave);
        Initialize();
        FindNearestRoadPoint();
    }

    public override void Initialize()
    {
        base.Initialize();
        InitializeFields();
        SetRagdoll(false);
        StartStateAction(EnemyState.Idle);
    }

    public void SetRoadRunSave(RoadRunSave roadRunSave)
    {
        if (!roadRunSave)
        {
            _canMove = false;
            return;
        }
        _canMove = true;
        _roadRunSave = roadRunSave;
    }

    #region OptionalMethods

    public void SetSpeed(float speed)
    {
        _movementSpeed = speed;
    }

    public void DefaultSpeed()
    {
        _movementSpeed = _baseMovementSpeed;
    }

    public void SetMovingBlend(float newValue)
    {
        if (_animator)
        {
            _movingBlend = newValue;
            _animator.SetFloat("MovingBlend", _movingBlend);
        }
    }

    public bool RayCastCheck(Vector3 origin, Vector3 dir, float length, LayerMask layerToCheck)
    {
        Debug.DrawRay(origin, dir.normalized * length, Color.blue);
        if (Physics.Raycast(origin, dir, out _hit, length, layerToCheck))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion

    #region Coroutine

    public virtual IEnumerator DeadAnimation()
    {
        SetRagdoll(true);
        _timer = _timeToDead;
        while (_timer > 0)
        {
            _timer -= Time.deltaTime;
            yield return null;
        }
    }

    #endregion

    #endregion

    #region PrivateOptionsMethods

    private void InitializeFields()
    {
        _savePointCounter = 0;
        _nextPosition = Position;
        _tempMagnitude = 100.0f;
        _timeToDead = 0.5f;
        DefaultSpeed();
    }

    private void SetAnimatorBool(string name, bool value)
    {
        if (_animator)
        {
            _animator.SetBool(name, value);
        }
    }

    private void SetRigidbodyValues(bool useGravity, bool isKinematic)
    {
        if (_rigidbody)
        {
            _rigidbody.isKinematic = true;
        }
    }

    private void SetRagdoll(bool value)
    {
        Rigidbody[] bodies = GetComponentsInChildren<Rigidbody>();
        for (int i = 0; i < bodies.Length; i++)
        {
            bodies[i].isKinematic = !value;
            bodies[i].transform.TryGetComponent(out _tempCollider);
            _tempCollider.enabled = value;
        }

        gameObject.TryGetComponent(out _tempCollider);
        _tempCollider.enabled = !value;
        if (_animator)
        {
            _animator.enabled = !value;
        }
    }

    #endregion
}