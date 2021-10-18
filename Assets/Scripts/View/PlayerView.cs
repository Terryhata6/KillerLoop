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

    public PlayerState State => _state;
    public float MovingBlend => _movingBlend;
    public Animator Animator => _animator;
    public Rigidbody Rigidbody => _rigidbody;
    public float MovementSpeed => _movementSpeed;
    public float JumpForce => _jumpForce;

    public void SetState(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.Idle:
                {
                    break;
                }
            case PlayerState.Dead:
                {
                    break;
                }
            case PlayerState.Jumping:
                {
                    break;
                }
            case PlayerState.Move:
                {
                    break;
                }
            case PlayerState.WallMove:
                {
                    break;
                }
            default: break;
        }
        _state = state;
    }

    public void SetMovingBlend(float newValue)
    {
        _movingBlend = newValue;
        Animator.SetFloat("MovingBlend", _movingBlend);
    }


    public void LevelFail()
    {

    }
}