using System;
using UnityEngine;

[Serializable]
public struct TargetsUIInfo
{
    #region PrivateFields

    [SerializeField] private int _targetId;
    [SerializeField] private LevelType _type;

    #endregion

    public TargetsUIInfo(int targetId, LevelType type)
    {
        _targetId = targetId;
        _type = type;
    }

    #region AccessFields
    public int TargetId => _targetId;
    public LevelType LevelType => _type;

    #endregion

}
