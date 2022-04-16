
using System.Collections;
using Dreamteck.Splines;
using UnityEngine;

public class EnemyView : BaseObjectView
{
      [SerializeField] private EnemyState _state = EnemyState.Idle;
      [SerializeField] private float _movingBlend = 0;
      [SerializeField] private Animator _animator;
      [SerializeField] private Rigidbody _rigidbody;
      [SerializeField] private float _baseMovementSpeed;
      [SerializeField] private float _jumpForce;
      [SerializeField] private SplineTracer _splineTracer;
      [SerializeField] private float _flyAwayPower;
      private bool _canMove;
      private float _baseY;
      private float _x;
      private Vector3 _tempVector;
      private Vector3 _hitNormal;
      private RaycastHit _hit;
      private float _movementSpeed;
      private float _timer;
      private float _timeToDead;
      private Collider _tempCollider;

      public Vector3 SplineDirection
      {
          get
          {
              if (_splineTracer.result.percent >= 1f)
              {
                  _canMove = false;
              }
              return _splineTracer.result.forward;
          }
      }
      public RaycastHit Hit => _hit;
      public Vector3 HitNormal => _hitNormal;
      public EnemyState State => _state;

      private void Awake()
      {
          DefaultSpeed();
          _timeToDead = 4f;
          SetRagdoll(false);
            if (_splineTracer && _splineTracer.spline)
            {
                  _canMove = true;
            }
      }
      
      #region Actions
    
      public void FlyAway(Vector3 dir)
      {
          if (_rigidbody)
          {
              _rigidbody.AddForce(dir * _flyAwayPower ,ForceMode.Impulse);
          }
      }
      
      public void Dead()
      {
          Debug.Log("EnemyDead" );
          GameEvents.Current.EnemyDead(this);
          gameObject.layer = 9;
      }

      public void DeadAnimation(Vector3 flyAwayDirection)
      {
          SetRagdoll(true);
          FlyAway(flyAwayDirection);
          StartCoroutine(DeadAnimation());
      }
      
      public void LookRotation(Vector3 lookVector)
      {
          if (_canMove)
          {
              lookVector.y = 0f;
              if (lookVector == Vector3.zero)
              {
                  return;
              }
              transform.rotation = Quaternion.LookRotation(lookVector, Vector3.up);
          }
      }
      
      public void Move(Vector3 dir)
      {
          Move(dir,_movementSpeed);
      }
      public void Move(Vector3 dir, float speed)
      {
          if (_canMove)
          {
              transform.Translate(dir * speed);
          }
      }

      public void MoveEnemyToWall(Vector3 hitPoint, Vector3 hitNormal)
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
              SetRigidbodyValues(false,false);
              _state = EnemyState.Idle;
          }
          
          public void Land()
          {
              SetAnimatorBool("Jump", false);
          }

          public void WallRun()
          {
              Debug.Log("wallrun");
              _hitNormal = _hit.normal;
              SetRigidbodyValues(false,false);
              SetAnimatorBool("WallRun",true);
              _state = EnemyState.WallRun;
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
              Debug.Log("run");
              if (_canMove)
              {
                  SetAnimatorBool("Run", true);
                  SetRigidbodyValues(false,false);
                  _state = EnemyState.Move;
              }
          }
      
          public void Slide()
          {
              SetRigidbodyValues(false,true);
              _state = EnemyState.Slide;
          }
      
          public void EndSlide()
          {
              _state = EnemyState.Idle;
              SetRigidbodyValues(false,false);
          }
          
          public void Jump()
          {
              SetAnimatorBool("Jump", true);
              SetRigidbodyValues(false,false);
              _baseY = Position.y;
              _x = -1f;
              _state = EnemyState.Jump;
          }
          
          public void CheckForAWall()
          {
              _tempVector.x = Forward.z;
              _tempVector.z = -Forward.x;
              _tempVector.y = 0f;
              if (RayCastCheck(Position + (_tempVector + Vector3.up) * 0.5f, Forward.normalized + Vector3.up, 1f, 1 << 11)
                  || RayCastCheck(Position - (_tempVector - Vector3.up) * 0.5f, Forward.normalized  + Vector3.up, 1f, 1 << 11))
              {
                  Land();
                  MoveEnemyToWall(Hit.point, Hit.normal);
                  WallRun();
              }
          }
      #endregion

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
      
      public IEnumerator DeadAnimation()
      {
          _timer = _timeToDead;
          while (_timer > 0)
          {
              _timer -= Time.deltaTime;
              yield return null;
          }
      }
}