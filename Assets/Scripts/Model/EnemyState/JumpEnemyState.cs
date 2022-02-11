
using UnityEngine;

public class JumpEnemyState : BaseEnemyState
{
    private Vector3 _tempVector;
    
    public override void Execute(EnemyView enemy)
    {
        base.Execute(enemy);
        enemy.Jumping();
        enemy.LookRotation(enemy.SplineDirection);
        enemy.Move(Vector3.forward * Time.deltaTime); 
        
        CheckToLand(enemy);
    }
    
    private void CheckToLand(EnemyView enemy)
    {
        if (enemy.RayCastCheck(enemy.Position + Vector3.up,  Vector3.down, 1.1f, 1 << 11|(1 << 12)))
        {
            enemy.Land();
            enemy.Stand();
        }
    }
}