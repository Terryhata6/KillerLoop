
using System.Collections.Generic;
using UnityEngine;

public class CollectablesSpawnPoints : BaseObjectView
{
    #region PrivateFields

    #region Serialized

    [Header("SavedList")]
    [SerializeField] private List<Vector3> _spawnPoints;

    #endregion

    #endregion

    #region AccessFields

    public List<Vector3> Points => _spawnPoints;

    #endregion

    #region PublicMethods

    public void Reset()
    {
        if (_spawnPoints != null)
        {
            _spawnPoints.Clear();
        }
    }

    public void SavePoint(Vector3 spawnPoint)
    {
        if (_spawnPoints != null)
        {
            _spawnPoints.Add(spawnPoint);
        }
    }

    #endregion

}