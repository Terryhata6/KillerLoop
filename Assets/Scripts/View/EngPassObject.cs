
using UnityEngine;
using System.Collections.Generic;

public class EngPassObject : BaseObjectView
{
    [SerializeField] private List<Transform> _moveTransforms;
    private List<Vector3> _movePoints;

    private void Awake()
    {
        _movePoints = new List<Vector3>();
        for (int i = 0; i < _moveTransforms.Count; i++)
        {
            _movePoints.Add(_moveTransforms[i].position);
        }
    }

    public List<Vector3> MovePoints => _movePoints;
}