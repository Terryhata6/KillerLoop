using System;

public class GameEvents
{
    public static GameEvents Current = new GameEvents();

    #region EnemyEvents

    public Action<EnemyView> OnEnemyDead;
    public void EnemyDead(EnemyView enemy)
    {
        OnEnemyDead?.Invoke(enemy);
    }

    #endregion
}