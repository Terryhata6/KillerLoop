using UnityEngine;
using System;

[Serializable]
public struct MovementWriterPoint
{
    #region PrivateFields

    [SerializeField] private Quaternion _rotation;
    [SerializeField] private Vector3 _position;
    [SerializeField] private EnemyState _state;

    #endregion

    public MovementWriterPoint(Quaternion rot, Vector3 pos, PlayerState state)
    {
        _rotation = rot;
        _position = pos;
        _state = EnemyState.Inactive;
        _state = ConvertState(state);
    }

    #region AccessFields
    public Quaternion Rotation => _rotation;
    public Vector3 Position => _position;
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
