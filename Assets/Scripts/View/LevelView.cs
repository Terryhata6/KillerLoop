using System;
using UnityEngine;

public class LevelView : BaseObjectView, 
    IEnemiesLevelInfoUpdater, IPlayerLevelInfoUpdater, ICollectablesLevelInfoUpdater,//Service
    IEquatable<LevelView>, IComparable<LevelView>
{
    #region PrivateFields

    #region Serialized

    [Header("Target UI Information")]
    [SerializeField] private TargetsUIInfo _targetsUIInfo;
    [Header("Player Information")]
    [SerializeField] private PlayerLevelInfo _playerInfo;
    [Header("Enemies Information")]
    [SerializeField] private EnemiesLevelInfo _enemiesInfo;
    [SerializeField] private RoadRunSave _roadRunWay;
    [Header("Collectables Information")]
    [SerializeField] private CollectablesLevelInfo _collectablesInfo;
    [Header("Winning scene")]
    [SerializeField] private FinalSceneView _winScene;
    [Header("Main scene")]
    [SerializeField] private GameObject _mainLevel;
    [Header("Writer Information")]
    [SerializeField] private WriterLevelInfo _writerLevelInfo;

    #endregion

    #endregion

    #region AccessFields

    public TargetsUIInfo TargetInfo => _targetsUIInfo;

    public int TargetId => _targetsUIInfo.TargetId;

    #endregion

    public void Load()
    {
        InitializeService();
        if (_winScene)
        {
            _winScene.Initialize();
        }
        UpdateConsumersInfo();
        Debug.Log("Level loaded");
        LevelEvents.Current.LevelLoaded();

    }

    public void ActiveWinScene()
    {
        if (!_winScene || !_mainLevel)
        {
            return;
        }
        _mainLevel.SetActive(false);
        _winScene.Enable();
        Debug.Log("Win Scene loaded");
        LevelEvents.Current.WinSceneLoaded();
    }

    public void UnLoad()
    {
        Delete();
    }

    #region IService

    private BaseService<IPlayerLevelInfoUpdater> _PlayerInfoUpdaterHelper;
    private BaseService<IEnemiesLevelInfoUpdater> _EnemiesInfoUpdaterHelper;
    private BaseService<ICollectablesLevelInfoUpdater> _CollectablesInfoUpdaterHelper;

    public EnemiesLevelInfo EnemiesInfo => _enemiesInfo;
    public RoadRunSave RoadRunWay => _roadRunWay;
    public PlayerLevelInfo PlayerInfo => _playerInfo;
    public WriterLevelInfo WriterInfo => _writerLevelInfo;
    public CollectablesLevelInfo CollectablesInfo => _collectablesInfo;

    public void AddConsumer(IConsumer consumer)
    {
        _EnemiesInfoUpdaterHelper?.AddConsumer(consumer);
        _PlayerInfoUpdaterHelper?.AddConsumer(consumer);
        _CollectablesInfoUpdaterHelper?.AddConsumer(consumer);
    }

    private void InitializeService()
    {
        _EnemiesInfoUpdaterHelper = new BaseService<IEnemiesLevelInfoUpdater>(this);
        _PlayerInfoUpdaterHelper = new BaseService<IPlayerLevelInfoUpdater>(this);
        _CollectablesInfoUpdaterHelper = new BaseService<ICollectablesLevelInfoUpdater>(this);
        _EnemiesInfoUpdaterHelper.FindConsumers();
        _PlayerInfoUpdaterHelper.FindConsumers();
        _CollectablesInfoUpdaterHelper.FindConsumers();
    }

    private void UpdateConsumersInfo()
    {
        _EnemiesInfoUpdaterHelper?.ServeConsumers();
        _PlayerInfoUpdaterHelper?.ServeConsumers();
        _CollectablesInfoUpdaterHelper?.ServeConsumers();
    }

    #endregion

    #region IComparable

    public int CompareTo(LevelView other)
    {
        return TargetId.CompareTo(other.TargetId);
    }

    #endregion

    #region IEquitable

    public bool Equals(LevelView other)
    {
        return other.TargetId.Equals(TargetId);
    }

    #endregion

    private void OnDestroy()
    {
        ServiceDistributor.Instance.RemoveService(this);
    }
}