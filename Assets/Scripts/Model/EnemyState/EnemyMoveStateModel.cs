using UnityEngine;

public class EnemyMoveStateModel: BaseEnemyStateModel
{
    #region PublicMethods

    public override void Execute(EnemyView enemy)
    {
        base.Execute(enemy);
        enemy.UpdateRoadPoint();
        enemy.MoveToNextPoint(enemy.BaseMovementSpeed * Time.deltaTime);
        enemy.SetMovingBlend(1f);
    }

    #endregion

}