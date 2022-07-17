using Dreamteck.Splines;
using System;
using UnityEngine;

[Serializable]
public struct PlayerLevelInfo
{
    #region PrivateFields
    [SerializeField] private Transform _spawn;
    [SerializeField] private SplineComputer _roadSpline;
    #endregion

    #region AccessFields
    public Transform Spawn => _spawn;
    public SplineComputer RoadSpline => _roadSpline;
    #endregion
}
