using UnityEngine;

public class MoveEnemyState : BaseEnemyState
{
    private Vector2 _movingVector2D;
    private float _magnitude;

    public override void Execute(EnemyView enemy)
    {
        base.Execute(enemy);
        enemy.LookRotation(enemy.SplineDirection);
        enemy.Move(Vector3.forward * Time.deltaTime);
        enemy.SetMovingBlend(1f);
    }
}