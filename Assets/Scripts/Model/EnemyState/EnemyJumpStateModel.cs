
using UnityEngine;

public class EnemyJumpStateModel : BaseEnemyStateModel
{

    #region PublicMethods

    public override void Execute(EnemyView enemy)
    {
        base.Execute(enemy);
        enemy.Flying();
        enemy.LookRotation(enemy.SplineDirection);
        enemy.Move(Vector3.forward * Time.deltaTime);

        CheckToLand(enemy);
        enemy.CheckForAWall();
    }

    #endregion

    #region PrivateMethods

    private void CheckToLand(EnemyView enemy)
    {
        if (enemy.RayCastCheck(enemy.Position + Vector3.up, Vector3.down + Vector3.forward * 0.4f, 1.2f, 1 << 11 | (1 << 12)))
        {
            enemy.Run();
        }
    }

    #endregion

}