using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class InGamePanel : BaseMenuPanel,
    IServiceConsumer<IProgressValuesUpdater>, IServiceConsumer<ITargetInfo>
{
    #region PrivateFields

    #region Serialized

    [Header("Panel")]
    [SerializeField] private GameObject _panel;

    [Header("Elements")]
    [SerializeField] private TextMeshProUGUI _targetName;
    [SerializeField] private TextMeshProUGUI _enemyBeatenCounter;
    [SerializeField] private TextMeshProUGUI _moneyCollectedCounter;
    [SerializeField] private Slider _targetProgress;
    [SerializeField] private Image _playerProgress;

    #endregion

//    private Dictionary<ProgressValueType, float> _progressValues;
    private TargetsUIInfo _currentTargetInfo;

    #endregion

    private void Start()
    {
        //CreateValuesDictionary();
    }

    #region PublicMethods

    public override void Hide()
    {
        if (!IsShow) return;
        _panel.gameObject.SetActive(false);
        IsShow = false;
    }

    public override void Show()
    {
        if (IsShow) return;
        _panel.gameObject.SetActive(true);
        IsShow = true;
        ResetProgressValues();
        //UpdateAllProgressValues();
    }

    #region IServiceConsumer

    public void UseService(IProgressValuesUpdater service)
    {
        if (service != null)
        {
            for (int i = 0; i < service.ProgressValues.Length; i++)
            {
                SetProgressValue(service.ProgressValues[i]);
            }
        }
    }

    public void UseService(ITargetInfo service)
    {
        if (service != null)
        {
            _currentTargetInfo = service.GetTargetInfo(service.CurrentTargetNumber);
        }
        if (IsShow)
        {
            UpdateTargetName(_currentTargetInfo);
        }

    }

    #endregion

    #endregion

    #region PrivateMethods

    //private void CreateValuesDictionary()
    //{
    //    _progressValues = new Dictionary<ProgressValueType, float>(4) 
    //    {
    //        {ProgressValueType.EnemyBeaten, 0f},
    //        {ProgressValueType.MoneyCollected, 0f},
    //        {ProgressValueType.PlayerDistance, 0f},
    //        {ProgressValueType.TargetDistance, 0f},

    //    };
    //}

    //private float GetProgressValue(ProgressValueType type)
    //{
    //    if (!_progressValues.ContainsKey(type)
    //        || _progressValues == null)
    //    {
    //        return 0f;
    //    }

    //    return _progressValues[type];
    //}
    private void SetProgressValue(ProgressValue value)
    {
        //if (_progressValues != null 
        //    && _progressValues.ContainsKey(value.ValueType))
        //{
        //    _progressValues[value.ValueType] = value.Value;
        //}
        if (IsShow)
        {
            UpdateProgressValue(value);
        }
    }

    #region UIUpdates

    private void UpdateTargetName(TargetsUIInfo info)
    {
        switch (info.LevelType)
        {
            case LevelType.Common:
                {
                    _targetName.text = $"target {info.LevelNumber}";
                    break;
                }
            case LevelType.Boss:
                {
                    _targetName.text = $"target {info.LevelNumber}";
                    break;
                }
            case LevelType.Bonus:
                {
                    _targetName.text = $"bonus target";
                    break;
                }
            default:
                {
                    _targetName.text = $"unknown target";
                    break;
                }
        }
    }

    private void ResetProgressValues()
    {
        UpdateMoneyCollectedCounter(0f);
        UpdateEnemyBeatenCounter(0f);
        UpdateTargetProgress(0f);
        UpdatePlayerProgress(0f);
    }

    private void UpdateProgressValue(ProgressValue value)
    {
        switch (value.ValueType)
        {
            case ProgressValueType.EnemyBeaten:
                {
                    UpdateEnemyBeatenCounter(value.Value);
                    break;
                }
            case ProgressValueType.MoneyCollected:
                {
                    UpdateMoneyCollectedCounter(value.Value);
                    break;
                }
            case ProgressValueType.PlayerDistance:
                {
                    UpdatePlayerProgress(value.Value);
                    break;
                }
            case ProgressValueType.TargetDistance:
                {
                    UpdateTargetProgress(value.Value);
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    //private void UpdateAllProgressValues()
    //{
    //    UpdateMoneyCollectedCounter(GetProgressValue(ProgressValueType.MoneyCollected));
    //    UpdateEnemyBeatenCounter(GetProgressValue(ProgressValueType.EnemyBeaten));
    //    UpdateTargetProgress(GetProgressValue(ProgressValueType.TargetDistance));
    //    UpdatePlayerProgress(GetProgressValue(ProgressValueType.PlayerDistance));
    //}

    private void UpdateMoneyCollectedCounter(float value)
    {
        if (value % 1 == 0
            && _moneyCollectedCounter != null)
        {
            _moneyCollectedCounter.text = value.ToString();
        }
        else
        {
            Debug.Log("Cant set money value");
        }
    }

    private void UpdateEnemyBeatenCounter(float value)
    {
        if (value % 1 == 0
            && _enemyBeatenCounter != null)
        {
            _enemyBeatenCounter.text = value.ToString();
        }
        else
        {
            Debug.Log("Cant set enemy value");
        }
    }

    private void UpdateTargetProgress(float value)
    {
        if (value >= 0
            && value <= 1
            && _targetProgress != null)
        {
            _targetProgress.value = value;
        }
        else
        {
            Debug.Log("Cant set target progress value");
        }
    }

    private void UpdatePlayerProgress(float value)
    {
        if (value >= 0
            && value <= 1
            && _playerProgress != null)
        {
            _playerProgress.fillAmount = value;
        }
        else
        {
            Debug.Log("Cant set player progress value");
        }
    }

    #endregion

    private void OnDestroy()
    {
    }

    #endregion

}