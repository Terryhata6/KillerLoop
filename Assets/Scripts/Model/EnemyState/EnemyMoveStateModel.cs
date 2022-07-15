using UnityEngine;

public class EnemyMoveStateModel: BaseEnemyStateModel
{
    #region PublicMethods

    public override void Execute(EnemyView enemy)
    {
        base.Execute(enemy);
        enemy.LookRotation(enemy.SplineDirection);
        enemy.Move(Vector3.forward * Time.deltaTime);
        enemy.SetMovingBlend(1f);
        GravityEffect(enemy);
    }

    #endregion

}