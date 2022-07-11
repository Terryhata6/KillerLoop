using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : BaseMenuPanel, 
    IServiceConsumer<IMoneyStorage>, IServiceConsumer<INewGoodsChecker>, IServiceConsumer<ITargetInfo>
{
    #region PrivateFiels

    #region Serialized

    [Header("Panel")]
    [SerializeField] private GameObject _panel;

    [Header("Elements")]
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _shopButton;
    [SerializeField] private Button _optionsButton;
    [SerializeField] private List<Animation> _menuAnimations;
    [SerializeField] private TextMeshProUGUI _moneyValue;
    [SerializeField] private TargetsPanelView _targetsPanel;
    [SerializeField] private ShopIconView _shopIcon;

    #endregion

    //Services
    private IMoneyStorage _moneyStorage;
    private INewGoodsChecker _newGoodsChecker;
    private ITargetInfo _targetInfoSource;

    #endregion

    private void Start()
    {
        Initialize();
    }

    #region PublicMethods

    public override void Hide()
    {
        if (!IsShow) return;
        _panel.gameObject.SetActive(false);
        IsShow = false;
        StopAnimations();
    }

    public override void Show()
    {
        if (IsShow) return;
        _panel.gameObject.SetActive(true);
        IsShow = true;
        ProcessNewGoods();
        ProcessTargetsInfo();
        StartAnimations();
        UpdateMoneyCounter();
    }

    #region IServiceConsumer

    public void UseService(IMoneyStorage service)
    {
        _moneyStorage = service;
        if (IsShow)
        {
            UpdateMoneyCounter();
        }
    }

    public void UseService(INewGoodsChecker service)
    {
        _newGoodsChecker = service;
        if (IsShow)
        {
            ProcessNewGoods();
        }
    }

    public void UseService(ITargetInfo service)
    {
        _targetInfoSource = service;
        if (IsShow)
        {
            ProcessTargetsInfo();
        }
    }

    #endregion

    #endregion

    #region PrivateMethods

    private void Initialize()
    {
        SetButtonEvents();
        _targetsPanel.Initialize(_targetInfoSource);
    }

    private void SetButtonEvents()
    {
        BindListenerToButton(_shopButton, UIEvents.Current.ShopMenuButton);
        BindListenerToButton(_startButton, UIEvents.Current.StartLevelButton);
        BindListenerToButton(_optionsButton, UIEvents.Current.OptionMenuButton);
    }

    private void ProcessNewGoods()
    {
        if (_newGoodsChecker != null && _shopIcon != null)
        {
            _shopIcon.SetAttention(_newGoodsChecker.NewGoodsAvaible);
        }
    }

    private void ProcessTargetsInfo()
    {
        _targetsPanel.UpdateTargetsInfo(_targetInfoSource);
    }

    private void UpdateMoneyCounter()
    {
        if (_moneyStorage != null && _moneyValue != null)
        {
            _moneyValue.text = _moneyStorage.MoneyValue.ToString();
        }
    }

    private void StartAnimations()
    {
        if (_menuAnimations.Count > 0)
        {
            for (int i = 0; i < _menuAnimations.Count; i++)
            {
                _menuAnimations[i].Play();
            }
        }
    }

    private void StopAnimations()
    {
        if (_menuAnimations.Count > 0)
        {
            for (int i = 0; i < _menuAnimations.Count; i++)
            {
                _menuAnimations[i].Stop();
            }
        }
    }

    private void OnDestroy()
    {
        RemoveListenersFromButton(_shopButton);
        RemoveListenersFromButton(_startButton);
        RemoveListenersFromButton(_optionsButton);
    }

    #endregion

}