
using UnityEngine;

public abstract class BaseEnemyStateModel : IEnemyState
{
    #region PrivateFields

    private float _gravity = 8f;

    #endregion

    #region PublicMethods

    public virtual void Execute(EnemyView enemy)
    {

    }

    #endregion

    #region ProtectedMethods

    protected void GravityEffect(EnemyView enemy)
    {
        if (enemy.RayCastCheck(enemy.Position + Vector3.up, Vector3.down, 1.2f, 1 << 11))
        {
            enemy.MoveWithSpeed(Vector3.down * (enemy.Hit.distance - 1f) * Time.deltaTime, _gravity);
        }
        else
        {
            enemy.MoveWithSpeed(Vector3.down * Time.deltaTime, _gravity);
        }
    }

    #endregion
}