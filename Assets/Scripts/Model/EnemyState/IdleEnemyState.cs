public class IdleEnemyState : BaseEnemyState
{
    public override void Execute(EnemyView enemy)
    {
        base.Execute(enemy);
        enemy.SetMovingBlend(0f);
        Gravity(enemy);
    }    
}