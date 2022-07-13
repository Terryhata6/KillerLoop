using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinGamePanel : BaseMenuPanel, 
    IServiceConsumer<IProgressValuesUpdater>, IServiceConsumer<IMultiplierCounter>
{
    #region PrivateFields

    #region Serialized

    [Header("Panel")]
    [SerializeField] private GameObject _panel;

    [Header("Elements")]
    [SerializeField] private Button _collectX2Button;
    [SerializeField] private Button _collectButton;
    [SerializeField] private List<Animation> _animations;
    [SerializeField] private TextMeshProUGUI _enemyBeatenValue;
    [SerializeField] private TextMeshProUGUI _multiplierValue;
    [SerializeField] private TextMeshProUGUI _moneyCollected;

    #endregion

    private float _timer;
    private float _countingPeriod;
    private bool _waitingToCloseMenu;
    private int _currentMoney;
    private int _moneyAdded;
    private int _enemyBeaten;
    private int _multiplier;
    private int _incrementValue;
    private float _incrementMultiplier;

    #endregion

    #region PublicMethods

    public override void Initialize()
    {
        _countingPeriod = 0.05f;
        _incrementMultiplier = 0.005f;
        ResetValues();
        SetButtonEvents();
    }

    public override void Hide()
    {
        if (!IsShow) return;
        _panel.gameObject.SetActive(false);
        IsShow = false;
        StopAnimations();
        ResetValues();
    }

    public override void Show()
    {
        if (IsShow) return;
        _panel.gameObject.SetActive(true);
        IsShow = true;
        StartAnimations();
        UpdateAllValues();
        EnableButtons();
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

    public void UseService(IMultiplierCounter service)
    {
        if (service != null)
        {
            SetMultiplier(service.Multiplier);
        }
    }

    #endregion

    #endregion

    #region PrivateMethods
    private void SetProgressValue(ProgressValue value)
    {
        switch (value.ValueType)
        {
            case ProgressValueType.EnemyBeaten:
                {
                    _enemyBeaten = (int)value.Value;
                    break;
                }
            case ProgressValueType.MoneyCollected:
                {
                    _moneyAdded = (int)value.Value;
                    if (IsShow)
                    {
                        UpdateMoneyCounter();
                    }
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    private void SetMultiplier(float value)
    {
        _multiplier = (int)value;
    }

    private void ResetValues()
    {
        _waitingToCloseMenu = false;
        _currentMoney = 0;
    }

    private void SetButtonEvents()
    {
        BindListenerToButton(_collectButton, UIEvents.Current.CollectButton);
        BindListenerToButton(_collectX2Button, UIEvents.Current.CollectX2Button);
        BindListenerToButton(_collectButton, DisableButtons);
        BindListenerToButton(_collectX2Button, DisableButtons);
    }

    private void EnableButtons()
    {
        if (_collectButton)
        {
            _collectButton.enabled = true;
        }
        if (_collectX2Button)
        {
            _collectX2Button.enabled = true;
        }
    }

    private void DisableButtons()
    {
        if (_collectButton)
        {
            _collectButton.enabled = false;
        }
        if (_collectX2Button)
        {
            _collectX2Button.enabled = false;
        }
    }

    #region UIUpdates

    private void UpdateAllValues()
    {
        UpdateMoneyCounter();
        UpdateMultiplierCounter(_multiplier);
        UpdateEnemyBeatenValue(_enemyBeaten);
    }
    private void UpdateMoneyCounter()
    {
        _moneyAdded = 10000;
        if (_moneyAdded > _currentMoney)
        {
            StartCoroutine(CountingMoney());
        }
    }
    private void UpdateMultiplierCounter(int value)
    {
        if (_multiplierValue)
        {
            _multiplierValue.text = $"x{value}";
        }
    }

    private void UpdateEnemyBeatenValue(int value)
    {
        if (_enemyBeatenValue)
        {
            _enemyBeatenValue.text = (value).ToString();
        }
    }

    private void UpdateMoneyCounterValue(int value)
    {
        if (_moneyCollected)
        {
            _moneyCollected.text = $"+{value}";
        }
    }

    private void MoneyIncrement(int value)
    {
        if (_currentMoney + value < _moneyAdded)
        {
            _currentMoney += value;
        }
        else
        {
            _currentMoney = _moneyAdded;
        }
        UpdateMoneyCounterValue(_currentMoney);
    }

    #endregion

    private void StartAnimations()
    {
        if (_animations != null)
        {
            for (int i = 0; i < _animations.Count; i++)
            {
                _animations[i]?.Play();
            }
        }
    }

    private void StopAnimations()
    {
        if (_animations != null)
        {
            for (int i = 0; i < _animations.Count; i++)
            {
                _animations[i]?.Stop();
            }
        }
    }

    private void OnDestroy()
    {
        RemoveListenersFromButton(_collectButton);
        RemoveListenersFromButton(_collectX2Button);
    }

    #region Coroutine

    private IEnumerator CountingMoney()
    {
        _incrementValue = (int)((_moneyAdded - _currentMoney) * _incrementMultiplier);
        while (_currentMoney < _moneyAdded)
        {
            _timer += Time.deltaTime;
            if (_timer >= _countingPeriod)
            {
                MoneyIncrement(_incrementValue);
            }
            yield return null;
        }
        if (_waitingToCloseMenu)
        {
            UIEvents.Current.ToMainMenu();
        }
    }

    #endregion

    #endregion
}
