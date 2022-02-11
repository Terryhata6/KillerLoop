using System;
using Dreamteck.Splines;
using UnityEngine;

public class EnemyView : BaseObjectView
{
      [SerializeField] private EnemyState _state = EnemyState.Idle;
      [SerializeField] private float _movingBlend = 0;
      [SerializeField] private Animator _animator;
      [SerializeField] private Rigidbody _rigidbody;
      [SerializeField] private float _movementSpeed;
      [SerializeField] private float _jumpForce;
      [SerializeField] private SplineTracer _splineTracer;
      private bool _canMove;
      private float _baseY;
      private float _x;
      private Vector3 _tempVector;
      private Vector3 _hitNormal;
      private RaycastHit _hit;

      public Vector3 SplineDirection => _splineTracer.result.forward;
      public RaycastHit Hit => _hit;
      public Vector3 HitNormal => _hitNormal;
      public EnemyState State => _state;

      private void Start()
      {
            if (TryGetComponent(out _splineTracer))
            {
                  _canMove = true;
            }
      }
      
      #region Actions

      public void Trigger()
      {
          Debug.Log(_splineTracer.result.forward);
      }
      public void Dead()
      {
          Debug.Log("EnemyDead" );
          Destroy(gameObject);
      }
      
      public void LookRotation(Vector3 lookVector)
      {

            transform.rotation = Quaternion.LookRotation(lookVector, Vector3.up);
      }
      
      public void Move(Vector3 dir)
      {
          if (_canMove)
          {
              transform.Translate(dir * _movementSpeed);
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
              SetRigidbodyValues(true,false);
              _state = EnemyState.Idle;
          }
          
          public void Land()
          {
              SetAnimatorBool("Jump", false);
          }

          public void WallRun()
          {
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
              _state = EnemyState.Move;
          }
      
          public void Slide()
          {
              SetRigidbodyValues(false,true);
              _state = EnemyState.Slide;
          }
      
          public void EndSlide()
          {
              _state = EnemyState.Idle;
              SetRigidbodyValues(true,false);
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

      public void SetMovingBlend(float newValue)
      {
          _movingBlend = newValue;
          _animator.SetFloat("MovingBlend", _movingBlend);
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
}