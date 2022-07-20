using UnityEngine;

public class EnemyWallRunStateModel : BaseEnemyStateModel
{
    #region PrivateField

    private Vector3 _tempVector;

    #endregion

    #region PublicMethods

    public override void Execute(EnemyView enemy)
    {
        base.Execute(enemy);

        enemy.UpdateRoadPoint();
        enemy.MoveToNextPoint(enemy.BaseMovementSpeed * Time.deltaTime);
        enemy.SetMovingBlend(CheckSide(enemy));
    }

    #endregion

    #region PrivateMethods

    private float CheckSide(EnemyView enemy)
    {
        _tempVector.x = enemy.Forward.z;
        _tempVector.z = -enemy.Forward.x;
        _tempVector.y = 0f;
        if (enemy.RayCastCheck(enemy.Position, _tempVector, 2f, 1 << 11))
        {
            return 1f;
        }
        else if (enemy.RayCastCheck(enemy.Position, -_tempVector, 2f, 1 << 11))
        {
            return -1f;
        }
        return 0f;
    }

    #endregion
}