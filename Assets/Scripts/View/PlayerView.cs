
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : BaseObjectView
{
    #region PrivateFiels
    [SerializeField] private PlayerState _state = PlayerState.Idle;
    [SerializeField] private float _movingBlend = 0;
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private Image _indicatorImage;
    private RaycastHit _hit;
    private Camera _mainCam;
    private EngPassObject _engPassObject;
    private List<Vector3> _movingPoints;
    private bool _readyToKill;
    private EnemyView _enemy;
    private Vector3 _tempVector;
    private float _baseY;
    private float _x;
    private Vector3 _hitNormal; //для некоторых состояний нужен
    #endregion
    
    #region  AccessFields
    public Image IndicatorImage => _indicatorImage;
    public RaycastHit Hit => _hit;
    public Vector3 HitNormal => _hitNormal;
    public PlayerState State => _state;
    public Animator Animator => _animator;
    public Rigidbody Rigidbody => _rigidbody;
    public Camera MainCam => _mainCam;
    public List<Vector3> MovingPoints => _movingPoints;
    #endregion
    
    private void Awake()
    {
        if (_rigidbody == null)
        {
            Debug.Log("rigidbody not set");
        }
        _mainCam = Camera.main;
        //SetRagdoll(false);
    }

    #region Actions

    public void Move(Vector3 dir)
    {
        Move(dir, _movementSpeed);
    }

    public void Move(Vector3 dir, float speed)
    {
        transform.Translate(dir * speed);
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
        _tempVector.y = (-1.8f * ((_x) * (_x)) + _jumpForce) + _baseY;
        _tempVector.x = Position.x;
        _tempVector.z = Position.z;
        transform.position = _tempVector;
        _x += Time.deltaTime * 1.3f;
    }
    
    public void Stand()
    {
        SetAnimatorBool("Run", true);
        SetRigidbodyValues(true,false);
        _state = PlayerState.Idle;
    }
    
    public void Land()
    {
        SetAnimatorBool("Jump", false);
    }

    public void AirKill()
    {
        Kill(Jump);
    }
    
    public void Kill(Action toDo)
    {
        SetRigidbodyValues(false,false);
        _state = PlayerState.Dead;
        if (_hit.collider.gameObject.TryGetComponent(out _enemy))
        {
            StartCoroutine(Kill(_enemy,toDo));
        }
    }

    public void WallRun()
    {
        _hitNormal = _hit.normal;
        SetRigidbodyValues(false,false);
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

    public void Slide()
    {
        SetRigidbodyValues(false,true);
        _state = PlayerState.Slide;
    }

    public void EndSlide()
    {
        _state = PlayerState.Idle;
        SetRigidbodyValues(true,false);
    }
    
    public void Jump()
    {
        SetAnimatorBool("Jump", true);
        SetRigidbodyValues(false,false);
        _baseY = Position.y;
        _x = -1f;
        _state = PlayerState.Jumping;
    }

    #endregion
    
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
        _movingBlend = newValue;
        Animator.SetFloat("MovingBlend", _movingBlend);
    }

    private void SetAnimatorBool(string name, bool value)
    {
        if (_animator)
        {
            _animator.SetBool(name,value);
        }
    }

    private void SetRigidbodyValues(bool useGravity, bool isKinematic)
    {
        if (_rigidbody)
        {
            _rigidbody.useGravity = useGravity;
            _rigidbody.isKinematic = isKinematic;
        }
    }

    public void SetRagdoll(bool value)
    {
        Rigidbody[] bodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody body in bodies)
        {
            body.isKinematic = !value;
            body.transform.GetComponent<Collider>().enabled = value;
        }
        GetComponent<CapsuleCollider>().enabled = !value; 
        SetRigidbodyValues(true,false);
        if (_animator)
        {
            _animator.enabled = !value;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out _engPassObject))
        {
            _movingPoints = _engPassObject.MovePoints;
            if (_movingPoints.Count > 0)
            {
                
                Slide();
            }
        }
    }

    private IEnumerator Kill(EnemyView enemy, Action toDo)
    {
        while (enemy && (Position - enemy.Position).magnitude > 0.5f)
        {
            transform.position = Vector3.MoveTowards(Position, enemy.Position, Time.deltaTime * _movementSpeed);
            yield return null;
        }
        if (enemy)
        {
            enemy.Dead();
        }
        toDo.Invoke();
    }

    public void LevelFail()
    {
        CustomDebug.Log("Level Failed");
    }
}