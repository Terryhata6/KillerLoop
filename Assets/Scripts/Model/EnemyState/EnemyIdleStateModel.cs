using UnityEngine;

public class EnemyIdleStateModel : BaseEnemyStateModel
{
    #region PublicMethods

    public override void Execute(EnemyView enemy)
    {
        base.Execute(enemy);
        enemy.SetMovingBlend(0f);
        enemy.MoveWithSpeed(Vector3.zero, 0);
    }

    #endregion

}