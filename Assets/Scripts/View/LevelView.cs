using System;
using UnityEngine;

public class LevelView : BaseObjectView, 
    IEnemiesLevelInfoUpdater, IPlayerLevelInfoUpdater, //Service
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

    #endregion

    #endregion

    #region AccessFields

    public TargetsUIInfo TargetInfo => _targetsUIInfo;

    public int TargetId => _targetsUIInfo.TargetId;

    #endregion

    public void Load()
    {
        InitializeService();
        UpdateConsumersInfo();
        Debug.Log("Level loaded");
        LevelEvents.Current.LevelLoaded();

    }

    public void UnLoad()
    {
        Delete();
    }

    #region IService

    private BaseService<IPlayerLevelInfoUpdater> _PlayerInfoUpdaterHelper;
    private BaseService<IEnemiesLevelInfoUpdater> _EnemiesInfoUpdaterHelper;

    public EnemiesLevelInfo EnemiesInfo => _enemiesInfo;

    public PlayerLevelInfo PlayerInfo => _playerInfo;

    public void AddConsumer(IConsumer consumer)
    {
        _EnemiesInfoUpdaterHelper?.AddConsumer(consumer);
        _PlayerInfoUpdaterHelper?.AddConsumer(consumer);
    }

    private void InitializeService()
    {
        _EnemiesInfoUpdaterHelper = new BaseService<IEnemiesLevelInfoUpdater>(this);
        _PlayerInfoUpdaterHelper = new BaseService<IPlayerLevelInfoUpdater>(this);
        _EnemiesInfoUpdaterHelper.FindConsumers();
        _PlayerInfoUpdaterHelper.FindConsumers();
    }

    private void UpdateConsumersInfo()
    {
        _EnemiesInfoUpdaterHelper?.ServeConsumers();
        _PlayerInfoUpdaterHelper?.ServeConsumers();
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