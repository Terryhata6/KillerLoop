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

    #region PlayerEvents

    public event Action OnRevive;
    public void Revive()
    {
        OnRevive?.Invoke();
    }

    #endregion

    #region Global

    public Action OnGameStart;

    public void GameStart()
    {
        OnGameStart?.Invoke();
    }

    #endregion
}