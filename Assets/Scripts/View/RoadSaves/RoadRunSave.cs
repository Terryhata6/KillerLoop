
using System.Collections.Generic;
using UnityEngine;

public class RoadRunSave : BaseObjectView
{
    #region PrivateFields

    #region Serialized

    [Header("SavedList")]
    [SerializeField] private List<MovementWriterPoint> _points;

    #endregion

    #endregion

    #region AccessFields

    public List<MovementWriterPoint> Points => _points;

    #endregion

    public void Reset()
    {
        if (Points != null)
        {
            _points.Clear();
        }
    }

    public void SavePoint(MovementWriterPoint point)
    {
        if (_points != null)
        {
            _points.Add(point);
        }
    }
}