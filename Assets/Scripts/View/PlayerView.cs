using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : BaseObjectView
{
    [SerializeField] private PlayerState _state = PlayerState.Idle;
    [SerializeField] private float _movingBlend = 0;
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _jumpForce;

    private PlayerStateTriggerView _trigger;

    public PlayerState State => _state;
    public float MovingBlend => _movingBlend;
    public Animator Animator => _animator;
    public Rigidbody Rigidbody => _rigidbody;
    public float MovementSpeed => _movementSpeed;
    public float JumpForce => _jumpForce;
    public PlayerStateTriggerView Trigger => _trigger;


    private void Awake()
    {
        if (_rigidbody == null)
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        SetRagdoll(false);
    }


    public void SetState(PlayerState state)
    {
        //switch (state)
        //{
        //    case PlayerState.Idle:
        //        {
        //            break;
        //        }
        //    case PlayerState.Dead:
        //        {
        //            break;
        //        }
        //    case PlayerState.Jumping:
        //        {
        //            break;
        //        }
        //    case PlayerState.Move:
        //        {
        //            break;
        //        }
        //    case PlayerState.WallMove:
        //        {
        //            break;
        //        }
        //    default: break;
        //}
        _state = state;
    }

    public void SetMovingBlend(float newValue)
    {
        _movingBlend = newValue;
        Animator.SetFloat("MovingBlend", _movingBlend);
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
        _rigidbody.isKinematic = true;
        _animator.enabled = !value;
    }

    public void SetTrigger(PlayerStateTriggerView trigger)
    {
        _trigger = trigger;
    }

    public void LevelFail()
    {
        CustomDebug.Log("Level Failed");
    }
}