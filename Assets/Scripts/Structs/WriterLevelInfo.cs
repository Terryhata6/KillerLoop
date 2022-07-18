using System;
using UnityEngine;

[Serializable]
public struct WriterLevelInfo
{
    #region PrivateFields
    [SerializeField] private RoadRunSave _savePrefab;
    [SerializeField] private CollectablesSpawnPoints _spawnPointsStorage;
    [SerializeField] private RoadRunWriter _writer;
    #endregion

    #region AccessFields

    public RoadRunSave SavePrefab => _savePrefab;
    public RoadRunWriter Writer => _writer;
    public CollectablesSpawnPoints SpawnPointsStorage => _spawnPointsStorage;
    public bool isActive => _writer != null;

    #endregion
}
