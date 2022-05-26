
using UnityEngine;
using System.Collections.Generic;
using Dreamteck.Splines;

public class EngPassObject : BaseObjectView
{
    [SerializeField] private SplineComputer _spline;
    public SplineComputer Spline => _spline;
}