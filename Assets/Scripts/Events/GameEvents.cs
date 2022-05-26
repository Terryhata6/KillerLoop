using System;

public class GameEvents
{
        public static GameEvents Current = new GameEvents();
        
        public Action<EnemyView> OnEnemyDead;

        public void EnemyDead(EnemyView enemy)
        { 
                OnEnemyDead?.Invoke(enemy);
        }
}