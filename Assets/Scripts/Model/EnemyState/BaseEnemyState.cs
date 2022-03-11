
using UnityEngine;

public abstract class BaseEnemyState : IEnemyState
{
    private float _gravity = 8f;
    public virtual void Execute(EnemyView enemy)
    {
        
    }
    protected void Gravity(EnemyView enemy)
    {
        if (enemy.RayCastCheck(enemy.Position + Vector3.up, Vector3.down, 1.2f, 1<<11))
        {
            enemy.Move(Vector3.down * (enemy.Hit.distance - 1f) * Time.deltaTime, _gravity);
        }
        else
        {
            enemy.Move(Vector3.down * Time.deltaTime, _gravity);
        }
    }
}