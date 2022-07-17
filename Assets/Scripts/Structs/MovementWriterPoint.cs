using UnityEngine;
using System;

[Serializable]
public struct MovementWriterPoint
{
    #region PrivateFields

    [SerializeField] private Vector3 _position;
    [SerializeField] private Quaternion _rotation;
    [SerializeField] private EnemyState _state;

    #endregion

    public MovementWriterPoint(Vector3 pos, Quaternion rot, PlayerState state)
    {
        _position = pos;
        _rotation = rot;
        _state = EnemyState.Inactive;
        _state = ConvertState(state);
    }

    #region AccessFields
    public Vector3 Position => _position;
    public Quaternion Rotation => _rotation;
    public EnemyState State => _state;

    #endregion

    private EnemyState ConvertState(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.Idle:
                {
                    return EnemyState.Idle;
                }
            case PlayerState.WallRun:
                {
                    return EnemyState.WallRun;
                }
            case PlayerState.Move:
                {
                    return EnemyState.Move;
                }
            case PlayerState.Jumping:
                {
                    return EnemyState.Jump;
                }
            case PlayerState.Slide:
                {
                    return EnemyState.Slide;
                }
            case PlayerState.Inactive:
                {
                    return EnemyState.Inactive;
                }
            default:
                {
                    return EnemyState.Idle;
                }
        }
    }
}
