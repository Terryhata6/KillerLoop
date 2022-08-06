using System.Collections.Generic;
using UnityEngine;

public class LevelController : BaseController,
    ITargetInfo, //Service
    ISaveable
{
    #region PrivateFields

    private List<LevelView> _levels;
    private LevelView _currentLevel;
    private int _tempInt;
    private int _currentLevelIndex;
    private int _levelIndexProprety
    {
        get { return _currentLevelIndex; }
        set
        {
            _currentLevelIndex = value;
            LevelIndexSave = value;
        }
    }

    #endregion

    #region AccessFields

    public SaveDataType Type => DataType; //ISaveable

    public readonly SaveDataType DataType;

    #endregion

    #region FieldsToSave

    public int LevelIndexSave;

    #endregion

    public LevelController(List<LevelView>  levels) : base()
    {
        SetLevels(levels);
        DataType = SaveDataType.LevelData;
    }

    #region PublicMethods

    #region IInitialize

    public override void Initialize()
    {
        base.Initialize();
        SetData(SaveProgressManager.Instance.LoadData<LevelController>(DataType));
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
        else if (_currentLevel && _currentLevelIndex < _levels.Count)
        {
            _levelIndexProprety = _levels.IndexOf(_currentLevel) + 1;
        }
        else if (_currentLevelIndex >= _levels.Count) 
        {
            _levelIndexProprety = 0;
        }
        UnLoadLevel(_currentLevel);
        LoadLevel(GetLevelByIndex(_currentLevelIndex));
        SaveProgressManager.Instance.SaveData(this);
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

    #endregion

    #endregion

    #region IService

    private BaseService<ITargetInfo> _serviceHelper;

    public int CurrentTargetNumber => LevelIndexSave + 1;

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
    }

    #endregion

    #region LoadProgress

    private void SetData(LevelController data)
    {
        if (data == null)
        {
            return;
        }
        _levelIndexProprety = data.LevelIndexSave;
    }

    #endregion

}