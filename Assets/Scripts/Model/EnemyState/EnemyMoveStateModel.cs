using UnityEngine;

public class EnemyMoveStateModel: BaseEnemyStateModel
{
    #region PublicMethods

    public override void Execute(EnemyView enemy)
    {
        base.Execute(enemy);
        enemy.Move(Vector3.zero);
        enemy.SetMovingBlend(1f);
    }

    #endregion

}