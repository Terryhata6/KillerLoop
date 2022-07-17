
using System;
using UnityEngine;

[Serializable]
public struct WriterLevelInfo

{
    #region PrivateFields
    [SerializeField] private RoadRunSave _savePrefab;
    [SerializeField] private RoadRunWriter _writer;
    #endregion

    #region AccessFields
    public RoadRunSave SavePrefab => _savePrefab;
    public RoadRunWriter Writer => _writer;
    public bool isActive => _writer != null;
    #endregion
}
