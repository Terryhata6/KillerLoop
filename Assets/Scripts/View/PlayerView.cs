using System;
using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerView : BaseObjectView
{
    #region PrivateFiels
    [SerializeField] private PlayerState _state = PlayerState.Idle;
    [SerializeField] private float _movingBlend = 0;
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _baseMovementSpeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private Image _indicatorImage;
    [SerializeField] private SplineProjector _splineProjector;
    private RaycastHit _hit;
    private Camera _mainCam;
    private EngPassObject _engPassObject;
    private List<Vector3> _movingPoints;
    private bool _readyToKill;
    private EnemyView _enemy;
    private Vector3 _tempVector;
    private float _baseY;
    private float _x;
    private float _timer;
    private float _timeToKill;
    private float _movementSpeed;
    private int _tempInt;
    private Collider _tempCollider;
    private Vector3 _hitNormal; //для некоторых состояний нужен
    #endregion
    
    #region  AccessFields
    public Image IndicatorImage => _indicatorImage;
    public RaycastHit Hit => _hit;
    public Vector3 HitNormal => _hitNormal;
    public PlayerState State => _state;
    public Animator Animator => _animator;
    public Camera MainCam => _mainCam;
    public Vector3 SplineDirection => _splineProjector.result.forward;
    public double SplinePercent => _splineProjector.result.percent;
    #endregion
    
    private void Awake()
    {
        if (_rigidbody == null)
        {
            Debug.Log("rigidbody not set");
        }
        _mainCam = Camera.main;
        DefaultSpeed();
        if (_splineProjector)
        {
            _splineProjector.enabled = false;
        }
        SetRagdoll(false);
    }

    #region Actions

    #region Moving

    public void Move(Vector3 dir)
    {
        Move(dir, _movementSpeed);
    }

    public void Move(Vector3 dir, float speed)
    {
        transform.Translate(dir * speed);
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
    
    public void Rotate(Quaternion dir)
    {
        transform.rotation = dir;
    }

    public void MovePlayerToWall(Vector3 hitPoint, Vector3 hitNormal)
    {
        _tempVector = Vector3.Project(Position, hitPoint) + hitNormal * 0.3f;
        _tempVector.y = Position.y;
        transform.position = _tempVector;
    }
    
    public void Jumping()
    {
        _tempVector.y = (-(_jumpForce-0.2f) * ((_x) * (_x)) + _jumpForce) + _baseY;
        _tempVector.x = Position.x;
        _tempVector.z = Position.z;
        transform.position = _tempVector;
        _x += Time.deltaTime * 1.3f;
    }
    
    public void Stand()
    {
        SetAnimatorBool("Run", true);
        SetRigidbodyValues(true);
        _state = PlayerState.Idle;
    }
    
    public void Land()
    {
        SetAnimatorBool("Jump", false);
    }

    public void WallRun()
    {
        _hitNormal = _hit.normal;
        SetRigidbodyValues(false);
        SetAnimatorBool("WallRun",true);
        _state = PlayerState.WallRun;
    }

    public void StopWallRun()
    {
        SetAnimatorBool("WallRun",false);
    }
    
    public void StopRun()
    {
        SetAnimatorBool("Run", false);
    }
    public void Run()
    {
        _state = PlayerState.Move;
    }

    public void Slide(SplineComputer spline)
    {
        _splineProjector.enabled = true;
        _splineProjector.spline = spline;
        if (_splineProjector.result.percent > 0.5f)
        {
            _splineProjector.direction = Spline.Direction.Backward;
        }
        else
        {
            _splineProjector.direction = Spline.Direction.Forward;
        }
        SetRigidbodyValues(true);
        SetAnimatorBool("EngPass",true);
        _state = PlayerState.Slide;
    }

    public void EndSlide()
    {
        Debug.Log("endSlide");
        _splineProjector.enabled = false;
        _splineProjector.spline = null;
        SetAnimatorBool("EngPass",false);
        DefaultSpeed();
        Stand();
    }
    
    public void Jump()
    {
        SetAnimatorBool("Jump", true);
        SetRigidbodyValues(false);
        _baseY = Position.y;
        _x = -1f;
        _state = PlayerState.Jumping;
    }

    #endregion

    #region Attack

    public void AirKill()
    {
        SetAnimatorBool("AirKill",true);
        Kill(EndAirKill);
    }

    public void EndAirKill()
    {
        SetAnimatorBool("AirKill",false);
        Jump();
    }

    public void GroundKill()
    {
        _tempInt = Random.Range(1, 3);
        SetAnimatorBool("GroundKill" + _tempInt.ToString(),true);
        Kill(EndGroundKill);
    }

    public void EndGroundKill()
    {
        SetAnimatorBool("GroundKill" + _tempInt.ToString(), false);
    }
    
    public void Kill(Action CurrentAction)
    {
        if (_hit.collider.gameObject.TryGetComponent(out _enemy))
        {
            SetRigidbodyValues(false);
            StartCoroutine(Kill(_enemy,CurrentAction));
        }
    }

    #endregion

    #endregion

    #region Options

    public void SetSpeed(float speed)
    {
        _movementSpeed = speed;
    }
    public void DefaultSpeed()
    {
        _movementSpeed = _baseMovementSpeed;
    }
    
    public bool RayCastCheck(Vector3 origin,Vector3 dir, float length, LayerMask layerToCheck)
    {
        Debug.DrawRay(origin , dir.normalized * length, Color.blue);
        if (Physics.Raycast(origin , dir, out _hit, length, layerToCheck))
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

    private void SetAnimatorBool(string name, bool value)
    {
        if (_animator)
        {
            Debug.Log(name);
            _animator.SetBool(name,value);
        }
    }

    private void SetRigidbodyValues(bool isKinematic)
    {
        SetRigidbodyValues(false,isKinematic);
    }
    private void SetRigidbodyValues(bool useGravity,bool isKinematic)
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

    #endregion
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out _engPassObject) && _engPassObject.Spline && _splineProjector)
        {
            Slide(_engPassObject.Spline);
        }
    }

    private IEnumerator Kill(EnemyView enemy, Action toDo)
    {
        if (enemy)
        {
            enemy.Dead();
        }
        _timer = 0.3f;
        while (_timer >= 0)
        {
            _timer -= Time.deltaTime;
            yield return null;
        }
        if (enemy)
        {
            enemy.DeadAnimation(enemy.Position - Position);
        }
        toDo.Invoke();



    }

    public void LevelFail()
    {
        CustomDebug.Log("Level Failed");
    }
}