using System.Collections.Generic;
using UnityEngine;

public class EnemyController : BaseController, IExecute
{
    private List<EnemyView> _enemies;
    private Dictionary<EnemyState, BaseEnemyState> _enemyStates;
    private int _index;
    private EnemyView _tempEnemy;
    
    public override void Initialize()
    {
        base.Initialize();

        GameEvents.Current.OnEnemyDead += EnemyRemove;
        
        LevelEvents.Current.OnLevelStart += Enable;
        LevelEvents.Current.OnLevelLose += Disable;
        LevelEvents.Current.OnLevelFinish += Disable;
        
        UIEvents.Current.OnButtonPause += Disable;
        UIEvents.Current.OnButtonResume += Enable;
        
        _enemyStates = new Dictionary<EnemyState, BaseEnemyState>
        {
            {EnemyState.Idle, new IdleEnemyState()},
            {EnemyState.Jump, new JumpEnemyState()},
            {EnemyState.Move, new MoveEnemyState()},
            {EnemyState.WallRun, new WallRunEnemyState()},
        };
    }

    public void Execute()
    {
        if (!_isActive)
        {
            return;
        }
        
        for (int _index = 0; _index < _enemies.Count; _index++)
        {
            _tempEnemy = _enemies[_index];
            if (_tempEnemy != null)
            {
                _enemyStates[_tempEnemy.State].Execute(_tempEnemy);
            }
        }
    }

    public void SetEnemies(List<EnemyView> enemies)
    {
        _enemies = new List<EnemyView>();
        _enemies = enemies;
        for (int _index = 0; _index < _enemies.Count; _index++)
        {
            EnemyInit(_enemies[_index]);
        }
    }

    private void EnemyInit(EnemyView enemy)
    {
        enemy.Run(); //State - move
    }

    private void EnemyRemove(EnemyView enemy)
    {
        if (_enemies.Contains(enemy))
        {
            _enemies.Remove(enemy);
        }
    }
}