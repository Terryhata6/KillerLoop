using System.Collections.Generic;
using UnityEngine;

public class EnemyController : BaseController, IExecute
{
    #region PrivateFields

    private List<EnemyView> _enemies;
    private Dictionary<EnemyState, BaseEnemyState> _enemyStates;
    private int _index;
    private EnemyView _tempEnemy;

    #endregion

    public EnemyController (List<EnemyView> enemies)
    {
        SetEnemies(enemies);
    }

    #region PublicMethods

    #region Iinitialize

    public override void Initialize()
    {
        base.Initialize();

        SetEvents();
        CreateStateDictionary();
    }

    #endregion

    #region IExecute

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

    #endregion

    public void SetEnemies(List<EnemyView> enemies)
    {
        if (enemies != null)
        {
            Enable();
            _enemies = enemies;
            for (int _index = 0; _index < _enemies.Count; _index++)
            {
                _enemies[_index].Initialize();
            }
        }
        else
        {
            Disable();
        }
    }

    #endregion

    #region PrivateMethods

    private void CreateStateDictionary()
    {
        _enemyStates = new Dictionary<EnemyState, BaseEnemyState>
        {
            {EnemyState.Idle, new IdleEnemyState()},
            {EnemyState.Jump, new JumpEnemyState()},
            {EnemyState.Move, new MoveEnemyState()},
            {EnemyState.WallRun, new WallRunEnemyState()},
        };
    }
    private void SetEvents()
    {
        GameEvents.Current.OnEnemyDead += EnemyRemove;

        LevelEvents.Current.OnLevelStart += Enable;
        LevelEvents.Current.OnLevelLose += Disable;
        LevelEvents.Current.OnLevelFinish += Disable;
    }
    private void EnemyRemove(EnemyView enemy)
    {
        if (_enemies.Contains(enemy))
        {
            _enemies.Remove(enemy);
        }
    }

    #endregion

}