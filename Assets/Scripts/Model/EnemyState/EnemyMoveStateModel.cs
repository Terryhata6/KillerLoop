using UnityEngine;

public class EnemyMoveStateModel: BaseEnemyStateModel
{
    #region PublicMethods

    public override void Execute(EnemyView enemy)
    {
        base.Execute(enemy);
        enemy.MoveWithSpeed(Vector3.zero, 0);
        enemy.SetMovingBlend(1f);
    }

    #endregion

}