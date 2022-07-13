using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopMenuPanel : BaseMenuPanel,
    IServiceConsumer<IMoneyStorage>
{
    #region PrivateFields

    #region Serialized

    [Header("Panel")]
    [SerializeField] private GameObject _panel;

    [Header("Elements")]
    [SerializeField] private TextMeshProUGUI _moneyStorageValue;
    [SerializeField] private TextMeshProUGUI _moneyGetValue;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _buySkinButton;
    [SerializeField] private Button _getMoneyButton;
    [SerializeField] private List<Button> Goods;

    #endregion

    private const float _decrementMultiplier = 0.005f;
    private const float _incrementMultiplier = 0.005f;

    private int _incrementValue;
    private int _decrementValue;
    private int _currentMoneyValue;
    private int _newMoneyValue;

    #endregion

    #region PublicMethods

    public override void Initialize()
    {
        SetButtonEvents();
    }

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

    }

    #region IserviceConsumer

    public void UseService(IMoneyStorage service)
    {
        if (service != null)
        {
            _newMoneyValue = service.MoneyValue;
        }
        if (IsShow)
        {
            UpdateMoneyStorage();
        }
        else
        {
            _currentMoneyValue = _newMoneyValue;
        }
    }

    #endregion

    #endregion

    #region PrivateMethods

    #region EnableAndDisableButtons

    private void EnableBuySkinButton()
    {
        if (_buySkinButton)
        {
            _buySkinButton.enabled = true;
        }
    }
    private void EnableGetMoneyButton()
    {
        if (_getMoneyButton)
        {
            _getMoneyButton.enabled = true;
        }
    }
    private void DisableGetMoneyButton()
    {
        if (_getMoneyButton)
        {
            _getMoneyButton.enabled = false;
        }
    }
    private void DIsableBuySkinButton()
    {
        if (_buySkinButton)
        {
            _buySkinButton.enabled = false;
        }
    }

    #endregion

    private void UpdateMoneyStorage()
    {
        if (_currentMoneyValue < _newMoneyValue)
        {
            StartCoroutine(IncreaseMoneyWithAnimation());
        }
        else if (_currentMoneyValue > _newMoneyValue)
        {
            StartCoroutine(DecreaseMoneyWithAnimation());
        }
    }
    private void SetButtonEvents()
    {
        BindListenerToButton(_exitButton, UIEvents.Current.ExitOptionsButton);
        BindListenerToButton(_getMoneyButton, UIEvents.Current.GetMoneyButton);
    }

    private void MoneyIncrement(int value)
    {
        if (_currentMoneyValue + value < _newMoneyValue)
        {
            _currentMoneyValue += value;
        }
        else
        {
            _currentMoneyValue = _newMoneyValue;
        }
        UpdateMoneyStorageValue(_currentMoneyValue);
    }
    private void MoneyDecrement(int value)
    {
        if (_currentMoneyValue - value > _newMoneyValue)
        {
            _currentMoneyValue += value;
        }
        else
        {
            _currentMoneyValue = _newMoneyValue;
        }
        UpdateMoneyStorageValue(_currentMoneyValue);
    }

    #region UIUpdates

    private void UpdateMoneyStorageValue(int value)
    {
        if (_moneyStorageValue)
        {
            _moneyStorageValue.text = $"{value}";
        }
    }

    #endregion

    private void OnDestroy()
    {
        RemoveListenersFromButton(_buySkinButton);
        RemoveListenersFromButton(_exitButton);
        RemoveListenersFromButton(_getMoneyButton);
    }

    #region Coroutines

    private IEnumerator IncreaseMoneyWithAnimation()
    {
        _incrementValue = (int)((_newMoneyValue - _currentMoneyValue) * _incrementMultiplier);
        while (_currentMoneyValue < _newMoneyValue)
        {
            MoneyIncrement(_incrementValue);
            yield return null;
        }
    }

    private IEnumerator DecreaseMoneyWithAnimation()
    {
        _decrementValue = (int)((_currentMoneyValue - _newMoneyValue) * _decrementMultiplier);
        while (_currentMoneyValue > _newMoneyValue)
        {
            MoneyDecrement(_decrementValue);
            yield return null;
        }
    }

    #endregion

    #endregion
}