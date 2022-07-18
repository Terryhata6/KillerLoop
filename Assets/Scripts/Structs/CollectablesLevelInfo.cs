using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public struct CollectablesLevelInfo
{
    #region PrivateFields

    [SerializeField] private List<CollectableView> _poolExamples;
    [SerializeField] private CollectablesSpawnPoints _spawnPoints;

    #endregion

    #region AccessFields
    public List<CollectableView> PoolExamples => _poolExamples;
    public CollectablesSpawnPoints Spawns => _spawnPoints;

    #endregion
}
