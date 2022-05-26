using UnityEngine;

public class WallRunEnemyState : BaseEnemyState
{
    private Vector3 _movingVector = Vector3.zero;
    private Vector3 _tempVector;
    private float _directionTreshold = 0.8f;

    public override void Execute(EnemyView enemy)
    {
        base.Execute(enemy);
        CalculateMovingVector3d(enemy.SplineDirection,enemy.HitNormal, out _movingVector);
        enemy.LookRotation(_movingVector);
        enemy.Move(Vector3.forward * Time.deltaTime);
        enemy.SetMovingBlend(CalculateMovingBlend(_movingVector,enemy.HitNormal));
    }

    private void CalculateMovingVector3d(Vector3 direct, Vector3 normal,out Vector3 movingVector3d)
    {
        _tempVector.x = normal.z;
        _tempVector.z = -normal.x;
        _tempVector.y = 0f;
        direct.y = 0f;
        if (Vector3.Dot(direct, -_tempVector.normalized) >= _directionTreshold)
        {
            movingVector3d = -_tempVector;
        }
        else if (Vector3.Dot(direct, _tempVector.normalized) >= _directionTreshold)
        {
            movingVector3d = _tempVector;
        }
        else
        {
            movingVector3d = Vector3.zero;
        }
    }

    private float CalculateMovingBlend( Vector3 movingVector,Vector3 normal)
    {
        _tempVector.z = -movingVector.x;
        _tempVector.x = movingVector.z;
        _tempVector.y = 0f;
        if (Vector3.Dot(  normal,_tempVector.normalized) >= _directionTreshold)
        {
            return -1f;
        }
        if (Vector3.Dot(normal,-_tempVector.normalized) >= _directionTreshold)
        {
            return 1f;
        }

        return 0f;
    }
}