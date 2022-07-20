
using UnityEngine;

public class EnemyJumpStateModel : BaseEnemyStateModel
{

    #region PublicMethods

    public override void Execute(EnemyView enemy)
    {
        base.Execute(enemy);
        enemy.Flying();
        enemy.UpdateRoadPoint();
        enemy.MoveToNextPoint(enemy.BaseMovementSpeed  * Time.deltaTime);
        CheckToLand(enemy);
    }

    #endregion

    #region PrivateMethods

    private void CheckToLand(EnemyView enemy)
    {
        if (enemy.RayCastCheck(enemy.Position + Vector3.up, Vector3.down, 1.1f, 1 << 11 | (1 << 12)))
        {
            enemy.Run();
        }
    }

    #endregion

}