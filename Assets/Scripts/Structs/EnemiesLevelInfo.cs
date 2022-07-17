using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public struct EnemiesLevelInfo
{
    #region PrivateFields

    [SerializeField] private List<EnemyView> _enemies;

    #endregion

    #region AccessFields
    public List<EnemyView> Enemies => _enemies;

    #endregion
}
