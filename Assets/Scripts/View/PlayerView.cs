using System;
using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerView : BaseObjectView, 
    IPlayerDistanceUpdater //Service
{
    #region PrivateFields

    #region Serialized

    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _baseMovementSpeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private Image _indicatorImage;
    [SerializeField] protected SplineProjector _roadSpline;
    [SerializeField] private SplineProjector _engPassSplineProjector;

    #endregion

    private PlayerState _state = PlayerState.Idle;
    private float _movingBlend = 0;
    private RaycastHit _hit;
    private Camera _mainCam;
    private EngPassObject _engPassObject;
    private EnemyView _enemy;
    private Vector3 _tempVector;
    private float _baseY;
    private float _x;
    private float _timer;
    private float _movementSpeed;
    private int _tempInt;
    private float _killingAnimationTime = 0.03f;
    private Collider _tempCollider;
    private Vector3 _hitNormal; //для некоторых состояний нужен

    #endregion

    #region AccessFields

    public Image IndicatorImage => _indicatorImage;
    public RaycastHit Hit => _hit;
    public Vector3 HitNormal => _hitNormal;
    public PlayerState State => _state;
    public Animator Animator => _animator;
    public Camera MainCam => _mainCam;
    public Vector3 SplineDirection => _engPassSplineProjector.result.forward;
    public double SplinePercent => _engPassSplineProjector.result.percent;

    #endregion

    private void Awake()
    {
        ChangeActionState(PlayerState.Inactive);
    }

    #region Actions

    #region StartAction

    public void Run()
    {
        ChangeActionState(PlayerState.Move);
    }

    public void Slide(SplineComputer spline)
    {
        _engPassSplineProjector.enabled = true;
        _engPassSplineProjector.spline = spline;
        if (_engPassSplineProjector.result.percent > 0.5f)
        {
            _engPassSplineProjector.direction = Spline.Direction.Backward;
        }
        else
        {
            _engPassSplineProjector.direction = Spline.Direction.Forward;
        }
        SetRigidbodyValues(true);
        SetAnimatorBool("EngPass", true);
        ChangeActionState(PlayerState.Slide);
    }

    public void Jump()
    {
        SetAnimatorBool("Jump", true);
        SetRigidbodyValues(false);
        _baseY = Position.y;
        _x = -1f;
        ChangeActionState(PlayerState.Jumping);
    }

    public void Stand()
    {
        SetAnimatorBool("Run", true);
        SetRigidbodyValues(true);
        ChangeActionState(PlayerState.Idle);
    }

    public void WallRun()
    {
        _hitNormal = _hit.normal;
        SetRigidbodyValues(true);
        SetAnimatorBool("WallRun", true);
        ChangeActionState(PlayerState.WallRun);
    }

    #endregion

    #region EndAction

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

    private void EndSlide()
    {
        Debug.Log("endSlide");
        _engPassSplineProjector.enabled = false;
        _engPassSplineProjector.spline = null;
        SetAnimatorBool("EngPass", false);
        DefaultSpeed();
        Stand();
    }

    #endregion

    #region Attack

    public void KillEnemy(EnemyView enemy)
    {
        if (enemy)
        {
            SetRigidbodyValues(false);
            if (State == PlayerState.Jumping)
            {
                AirKill(enemy);
            }
            else
            {
                GroundKill(enemy);
            }
            enemy.Dead();
        }
    }

    public void AirKill(EnemyView enemy)
    {
        SetAnimatorBool("AirKill", true);
        StartCoroutine(KillAnimation(enemy, EndAirKill));
    }

    private void EndAirKill()
    {
        SetAnimatorBool("AirKill", false);
        Jump();
    }

    public void GroundKill(EnemyView enemy)
    {
        _tempInt = Random.Range(1, 3);
        SetAnimatorBool("GroundKill" + _tempInt.ToString(), true);
        StartCoroutine(KillAnimation(enemy, EndGroundKill));
    }

    private void EndGroundKill()
    {
        SetAnimatorBool("GroundKill" + _tempInt.ToString(), false);
    }

    #endregion

    public virtual void Move(Vector3 dir)
    {
        MoveWithSpeed(dir, _movementSpeed);
        UpdateConsumersInfo();
    }

    public void MoveWithSpeed(Vector3 dir, float speed)
    {
        _transform.Translate(dir * speed);
    }

    public void LookRotation(Vector3 lookVector)
    {
        lookVector.y = 0f;
        if (lookVector.Equals(Vector3.zero))
        {
            return;
        }
        Rotate(Quaternion.LookRotation(lookVector));
    }

    public virtual void Rotate(Quaternion dir)
    {
        _transform.rotation = dir;
    }

    public void MovePlayerToWall(Vector3 hitPoint, Vector3 hitNormal)
    {
        _tempVector = Vector3.Project(Position, hitPoint) + hitNormal * 0.3f;
        _tempVector.y = Position.y;
        _transform.position = _tempVector;
    }

    public void Flying()
    {
        _tempVector.y = (-(_jumpForce - 0.2f) * ((_x) * (_x)) + _jumpForce) + _baseY;
        _tempVector.x = Position.x;
        _tempVector.z = Position.z;
        _transform.position = _tempVector;
        _x += Time.deltaTime * 1.3f;
    }

    public void Dead()
    {
        Debug.Log("PlayerDead");
        SetRagdoll(true);
        ChangeActionState(PlayerState.Inactive);
        LevelEvents.Current.LevelLose();
    }

    public Vector3 SaveRevivePosition()
    {
        return _roadSpline?.result.position ?? Vector3.zero;
    }

    public void LoadRevivePosition(Vector3 revivePos)
    {
        _tempVector = revivePos;
        _tempVector.y = 2f;
        _transform.position = _tempVector;
        _transform.rotation = Quaternion.identity;
    }

    #endregion

    #region PublicMethods

    public void Initialize(PlayerLevelInfo info)
    {
        base.Initialize();

        InitializeService();
        CheckRigidbody();
        InitializeFields();
        SetRagdoll(false);
        SetRoadSpline(info.RoadSpline);
        Stand();
        Debug.Log("Player Loaded");
    }

    public override void Initialize()
    {
        Initialize(new PlayerLevelInfo());
    }

    #region Optional

    public void SetSpeed(float speed)
    {
        _movementSpeed = speed;
    }

    public void DefaultSpeed()
    {
        _movementSpeed = _baseMovementSpeed;
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

    public void SetMovingBlend(float newValue)
    {
        if (_animator)
        {
            _movingBlend = newValue;
            Animator.SetFloat("MovingBlend", _movingBlend);
        }
    }

    #endregion

    #endregion

    #region PrivateMethods

    #region Initialize

    private void CheckRigidbody()
    {
        if (_rigidbody == null)
        {
            Debug.Log("rigidbody not set");
        }
    }

    private void InitializeFields()
    {
        if (_engPassSplineProjector)
        {
            _engPassSplineProjector.enabled = false;
        }
        else
        {
            Debug.Log("EngPassProjector missing");
        }
        _mainCam = Camera.main;
        DefaultSpeed();
    }

    #endregion

    public virtual void ChangeActionState(PlayerState state)
    {
        StopCurrentAction();
        _state = state;
    }

    private void StopCurrentAction()
    {
        switch (_state)
        {
            case PlayerState.Jumping:
                {
                    Land();
                    break;
                }
            case PlayerState.Move:
                {
                    StopRun();
                    break;
                }
            case PlayerState.Idle:
                {
                    StopRun();
                    break;
                }
            case PlayerState.Slide:
                {
                    EndSlide();
                    break;
                }
            case PlayerState.WallRun:
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

    #region Optional

    private void SetRoadSpline(SplineComputer roadSpline)
    {
        if (roadSpline)
        {
            _roadSpline.spline = roadSpline;
        }
    }

    private void SetAnimatorBool(string name, bool value)
    {
        if (_animator)
        {
            _animator.SetBool(name, value);
        }
    }

    private void SetRigidbodyValues(bool isKinematic)
    {
        SetRigidbodyValues(false, isKinematic);
    }

    private void SetRigidbodyValues(bool useGravity, bool isKinematic)
    {
        if (_rigidbody)
        {
            _rigidbody.useGravity = useGravity;
            _rigidbody.isKinematic = isKinematic;
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

    private void PushAwayEnemy(EnemyView enemy)
    {
        if (enemy)
        {
            enemy.PushAway(enemy.Position - Position);
        }
    }

    #endregion

    #region Coroutine

    private IEnumerator KillAnimation(EnemyView enemy, Action toDo)
    {
        _timer = _killingAnimationTime;
        while (_timer >= 0)
        {
            _timer -= Time.deltaTime;
            yield return null;
        }
        PushAwayEnemy(enemy);
        toDo.Invoke();
    }

    #endregion

    #region OnTriggerEnter

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out _engPassObject)
            && _engPassObject.Spline
            && _engPassSplineProjector)
        {
            Slide(_engPassObject.Spline);
        }
    }

    #endregion

    private void OnDestroy()
    {
        ServiceDistributor.Instance.RemoveService(this);
    }

    #endregion

    #region IService

    private BaseService<IPlayerDistanceUpdater> _serviceHelper;

    public float Distance => (float)_roadSpline?.result.percent;

    public void AddConsumer(IConsumer consumer)
    {
        _serviceHelper?.AddConsumer(consumer);
    }

    private void InitializeService()
    {
        _serviceHelper = new BaseService<IPlayerDistanceUpdater>(this);
        _serviceHelper.FindConsumers();
    }

    private void UpdateConsumersInfo()
    {
        _serviceHelper?.ServeConsumers();
    }

    #endregion
}