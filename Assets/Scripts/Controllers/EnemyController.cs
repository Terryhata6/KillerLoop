using System.Collections.Generic;
using UnityEngine;

public class EnemyController : BaseController, IExecute, 
    IBeatenEnemyCounter, //Service
    IServiceConsumer<IEnemiesLevelInfoUpdater> //Consumer
{
    #region PrivateFields

    private List<EnemyView> _enemies;
    private Dictionary<EnemyState, BaseEnemyStateModel> _enemyStates;
    private int _index;
    private EnemyView _tempEnemy;
    private int _enemyBeaten;

    #endregion

    public EnemyController () : base()
    {
        Disable();    
    }

    #region PublicMethods

    #region Iinitialize

    public override void Initialize()
    {
        base.Initialize();

        SetEvents();
        CreateStateDictionary();
        InitializeFields();
        InitializeService();
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
        LevelEvents.Current.OnLevelContinue += Enable;
        LevelEvents.Current.OnLevelLose += Disable;
        LevelEvents.Current.OnLevelWin += Disable;

        UIEvents.Current.OnToMainMenu += Disable;
        UIEvents.Current.OnReviveButton += DisableEnemiesAnimation;
    }

    private void InitializeFields()
    {
        if (_enemies == null)
        {
            _enemies = new List<EnemyView>();
        }
    }

    #endregion


    private void LoadNewEnemies(List<EnemyView> enemies)
    {
        if (enemies != null)
        {
            SetEnemies(enemies);
            ResetFields();
            InitializeEnemies(_enemies);
            Debug.Log("Enemies loaded");
        }
        else
        {
            Debug.Log("Oops! Enemies missing");
        }
    }

    private void SetEnemies(List<EnemyView> enemies)
    {
        _enemies = enemies;
    }

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

    private void DisableEnemiesAnimation()
    {
        for (int i = 0; i < _enemies.Count; i++)
        {
            _enemies[i].SetMovingBlend(0f);
        }
    }

    #endregion

    #region IService

    private BaseService<IBeatenEnemyCounter> _serviceHelper;

    public int EnemyBeaten => _enemyBeaten;

    public void AddConsumer(IConsumer consumer)
    {
        _serviceHelper?.AddConsumer(consumer);
    }

    private void InitializeService()
    {
        _serviceHelper = new BaseService<IBeatenEnemyCounter>(this);
        _serviceHelper.FindConsumers();
    }

    private void ServeEnemyBeatenConsumers()
    {
        _serviceHelper?.ServeConsumers();
    }

    #endregion

    #region IServiceConsumer

    public void UseService(IEnemiesLevelInfoUpdater service)
    {
        if (service != null)
        {
            LoadNewEnemies(service.EnemiesInfo.Enemies);
        }
    }

    #endregion

}