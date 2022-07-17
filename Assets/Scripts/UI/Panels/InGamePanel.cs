using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGamePanel : BaseMenuPanel,
    IServiceConsumer<IBeatenEnemyCounter>, IServiceConsumer<ITargetInfo>,
    IServiceConsumer<ICollectedMoneyCounter>, IServiceConsumer<IPlayerDistanceUpdater>,
    IServiceConsumer<ITargetDistanceUpdater>
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
    [SerializeField] private Slider _playerProgress;

    #endregion

    private TargetsUIInfo _currentTargetInfo;

    #endregion

    private void Awake()
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
        ResetUIValues();
        UpdateTargetName(_currentTargetInfo);
    }

    #region IServiceConsumer

    public void UseService(IBeatenEnemyCounter service)
    {
        if (service == null || !IsShow)
        {
            return;
        }
        UpdateEnemyBeatenCounter(service.EnemyBeaten);
    }

    public void UseService(ICollectedMoneyCounter service)
    {
        if (service == null || !IsShow)
        {
            return;
        }
        UpdateMoneyCollectedCounter(service.MoneyCollected);
    }

    public void UseService(ITargetDistanceUpdater service)
    {
        if (service == null || !IsShow)
        {
            return;
        }
        UpdateTargetDistance(service.Distance);
    }

    public void UseService(IPlayerDistanceUpdater service)
    {
        if (service == null || !IsShow)
        {
            return;
        }
        UpdatePlayerDistance(service.Distance);
    }

    public void UseService(ITargetInfo service)
    {
        if (service != null)
        {
            _currentTargetInfo = service.GetTargetInfo(service.CurrentTargetNumber);
        }
    }

    #endregion

    #endregion

    #region PrivateMethods

    #region UIUpdates

    private void UpdateTargetName(TargetsUIInfo info)
    {
        if (!_targetName)
        {
            return;
        }
        switch (info.LevelType)
        {
            case LevelType.Common:
                {
                    _targetName.text = $"target {info.TargetId}";
                    break;
                }
            case LevelType.Boss:
                {
                    _targetName.text = $"target {info.TargetId}";
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

    private void ResetUIValues()
    {
        UpdateMoneyCollectedCounter(0);
        UpdateEnemyBeatenCounter(0);
        UpdateTargetDistance(0f);
        UpdatePlayerDistance(0f);
    }

    private void UpdateMoneyCollectedCounter(int value)
    {
        if (value % 1 == 0
            && _moneyCollectedCounter)
        {
            _moneyCollectedCounter.text = value.ToString();
        }
        else
        {
            Debug.Log("Cant set money value");
        }
    }

    private void UpdateEnemyBeatenCounter(int value)
    {
        if (value % 1 == 0
            && _enemyBeatenCounter)
        {
            _enemyBeatenCounter.text = value.ToString();
        }
        else
        {
            Debug.Log("Cant set enemy value");
        }
    }

    private void UpdateTargetDistance(float value)
    {
        if (value >= 0
            && value <= 1
            && _targetProgress)
        {
            _targetProgress.value = value;
        }
        else
        {
            Debug.Log("Cant set target progress value");
        }
    }

    private void UpdatePlayerDistance(float value)
    {
        if (value >= 0
            && value <= 1
            && _playerProgress)
        {
            _playerProgress.value = value;
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