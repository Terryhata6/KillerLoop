using System.Collections.Generic;
using UnityEngine;

public class EnemyController : BaseController, IExecute, IContainServices, IBeatenEnemyCounter
{
    #region PrivateFields

    private List<EnemyView> _enemies;
    private Dictionary<EnemyState, BaseEnemyStateModel> _enemyStates;
    private int _index;
    private EnemyView _tempEnemy;
    private List<IServiceConsumer<IBeatenEnemyCounter>> _beatenEnemyConsumers;
    private List<IService> _services;
    private int _enemyBeaten;

    public int EnemyBeaten => _enemyBeaten;

    #endregion

    public EnemyController (List<EnemyView> enemies)
    {
        SetNewEnemies(enemies);    
    }

    #region PublicMethods

    #region Iinitialize

    public override void Initialize()
    {
        base.Initialize();

        SetEvents();
        CreateStateDictionary();
        InitializeFields();
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

    public List<IService> GetServices()
    {
        return _services;
    }

    public void AddConsumer(IConsumer consumer)
    {
        if (consumer != null 
            && consumer is IServiceConsumer<IBeatenEnemyCounter>)
        {
            SetBeatenEnemyConsumer(consumer as IServiceConsumer<IBeatenEnemyCounter>);
        }
    }
    public void SetNewEnemies(List<EnemyView> enemies)
    {
        if (enemies != null)
        {
            Enable();
            _enemies = enemies;
            ResetFields();
            InitializeEnemies(_enemies);
        }
        else
        {
            Disable();
        }
    }

    #endregion

    #region PrivateMethods

    #region Initialize

    private void CreateStateDictionary()
    {
        _enemyStates = new Dictionary<EnemyState, BaseEnemyStateModel>
        {
            {EnemyState.Idle, new EnemyIdleStateModel()},
            {EnemyState.Jump, new EnemyJumpStateModel()},
            {EnemyState.Move, new EnemyMoveStateModel()},
            {EnemyState.WallRun, new EnemyWallRunStateModel()},
            {EnemyState.Inactive, new EnemyInactiveStateModel()}
        };
    }

    private void SetEvents()
    {
        GameEvents.Current.OnEnemyDead += EnemyRemove;

        LevelEvents.Current.OnLevelStart += Enable;
        LevelEvents.Current.OnLevelLose += Disable;
        LevelEvents.Current.OnLevelFinish += Disable;

        UIEvents.Current.OnToMainMenu += Disable;
    }

    private void InitializeFields()
    {
        _services = new List<IService>();
        _beatenEnemyConsumers = new List<IServiceConsumer<IBeatenEnemyCounter>>();
        if (_enemies == null)
        {
            _enemies = new List<EnemyView>();
        }
        _services.Add(this);
    }

    #endregion

    private void InitializeEnemies(List<EnemyView> enemies)
    {
        for (int _index = 0; _index < enemies.Count; _index++)
        {
            if (enemies[_index] == null)
            {
                _enemies.Remove(enemies[_index]);
                continue;
            }
            enemies[_index].Initialize();
        }
    }

    private void ResetFields()
    {
        _enemyBeaten = 0;
    }

    private void EnemyRemove(EnemyView enemy)
    {
        if (_enemies.Contains(enemy))
        {
            _enemies.Remove(enemy);
        }
        CountBeatenEnemy();
    }

    private void CountBeatenEnemy()
    {
        _enemyBeaten++;
        ServeEnemyBeatenConsumers();
    }

    private void ServeEnemyBeatenConsumers()
    {
        for (_index = 0; _index < _beatenEnemyConsumers.Count; _index++)
        {
            if (_beatenEnemyConsumers[_index] != null)
            {
                _beatenEnemyConsumers[_index].UseService(this);
            }
        }
    }

    private void SetBeatenEnemyConsumer(IServiceConsumer<IBeatenEnemyCounter> consumer)
    {
        if (!_beatenEnemyConsumers.Contains(consumer))
        {
            _beatenEnemyConsumers.Add(consumer);
        }
    }

    #endregion

}