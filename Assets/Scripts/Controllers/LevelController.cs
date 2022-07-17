using System.Collections.Generic;
using UnityEngine;
public class LevelController : BaseController,
    ITargetInfo //Service
{
    #region PrivateFiedls

    #region Serialized

    private List<LevelView> _levels;
    private LevelView _currentLevel;
    private int _tempInt;

    #endregion

    #endregion

    public LevelController(List<LevelView>  levels) : base()
    {
        SetLevels(levels);
    }

    #region PublicMethods

    #region IInitialize

    public override void Initialize()
    {
        base.Initialize();
        SetEvents();
        InitializeService();
    }

    #endregion

    #endregion

    #region PrivateMethods

    #region Initialize

    private void SetEvents()
    {
        GameEvents.Current.OnGameStart += ChangeLevel;

        LevelEvents.Current.OnNextLevel += ChangeLevel;
        LevelEvents.Current.OnLevelRestart += RestartLevel;

    }

    #endregion

    #region LevelsManage

    private void ChangeLevel()
    {
        if (_levels == null
            && _levels.Count == 0)
        {
            return;
        }
        else if (!_currentLevel || CurrentTargetNumber >= _levels.Count)
        {
            _tempInt = 0;
        }
        else
        {
            _tempInt = _levels.IndexOf(_currentLevel) + 1;
            UnLoadLevel(_currentLevel);
        }
        LoadLevel(GetLevelByIndex(_tempInt));
    }

    private void RestartLevel()
    {
        _tempInt = _levels.IndexOf(_currentLevel);
        UnLoadLevel(_currentLevel);
        LoadLevel(GetLevelByIndex(_tempInt));
    }

    private LevelView GetLevelByIndex(int index)
    {
        if (index < _levels.Count && index >= 0)
        {
            return _levels[index];
        }
        else
        {
            return null;
        }
    }

    private void LoadLevel(LevelView level)
    {
        if (level)
        {
            _currentLevel = GameObject.Instantiate(level);
            _currentLevel.Load();
            UpdateConsumersInfo();
        }
    }

    private void UnLoadLevel(LevelView level)
    {
        if (level)
        {
            level.UnLoad();
        }
    }

    private void SetLevels(List<LevelView> levels)
    {
        if (levels != null
            && levels.Count > 0)
        {
            Enable();
            _levels = GetLevelsSorted(levels);
        }
        else
        {
            Disable();
            Debug.Log("Oops! Levels missing");
        }
    }

    private List<LevelView> GetLevelsSorted(List<LevelView> levels)
    {
        levels.Sort();
        return levels;
    }

    private LevelView FindLevelByTargetId(int targetId)
    {
        if (_levels == null)
        {
            return null;
        }
        for (int i = 0; i < _levels.Count; i++)
        {
            if (_levels[i].TargetId.Equals(targetId))
            {
                return _levels[i];
            }
        }
        return null;
    }

    #endregion

    #endregion

    #region IService

    private BaseService<ITargetInfo> _serviceHelper;
    private List<TargetsUIInfo> _tempInfos;

    public int CurrentTargetNumber => _levels.IndexOf(_currentLevel) + 1;

    public TargetsUIInfo GetTargetInfo(int targetNumber)
    {
        return GetLevelByIndex(targetNumber - 1)?.TargetInfo
                   ?? new TargetsUIInfo(0, LevelType.Null);
    }

   public void AddConsumer(IConsumer consumer)
    {
        _serviceHelper?.AddConsumer(consumer);
    }

    private void UpdateConsumersInfo()
    {
        _serviceHelper?.ServeConsumers();
    }

    private void InitializeService()
    {
        _serviceHelper = new BaseService<ITargetInfo>(this);
        _serviceHelper.FindConsumers();
        _tempInfos = new List<TargetsUIInfo>();
    }

    #endregion
}