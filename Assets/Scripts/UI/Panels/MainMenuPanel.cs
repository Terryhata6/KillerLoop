using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : BaseMenuPanel, IUseServices
{
    #region PrivateFiels
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

    //Dependencies
    private IMoneyStorage _moneyStorage;
    private INewGoodsChecker _newGoodsChecker;
    private ITargetInfo _targetInfoSource;

    #endregion

    private void Start()
    {
        Initialize();
    }

    #region PublicMethods

    public void SetServices(List<IService> services)
    {
        for (int i = 0; i < services.Count; i++)
        {
            if (services[i] is IMoneyStorage)
            {
                _moneyStorage = services[i] as IMoneyStorage;
            }
            if (services[i] is INewGoodsChecker)
            {
                _newGoodsChecker = services[i] as INewGoodsChecker;
            }
            if (services[i] is ITargetInfo)
            {
                _targetInfoSource = services[i] as ITargetInfo;
            }
        }
    }

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

    #endregion

    #region PrivateMethods

    private void Initialize()
    {
        InitializeButtonEvents();
        _targetsPanel.Initialize(_targetInfoSource);
    }

    private void InitializeButtonEvents()
    {
        BindListenerToButton(_shopButton, UIEvents.Current.ShopMenuButton);
        BindListenerToButton(_startButton, UIEvents.Current.StartLevelButton);
        BindListenerToButton(_optionsButton, UIEvents.Current.OptionMenuButton);
    }

    private void ProcessNewGoods()
    {
        if (_newGoodsChecker != null)
        {
            _shopIcon.SetAttention(_newGoodsChecker.NewGoodsAvaible);
        }
    }

    private void ProcessTargetsInfo()
    {
        _targetsPanel.UpdateTargetName();
        _targetsPanel.UpdateTargetsIcons();
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

    private void UpdateMoneyCounter()
    {
        if (_moneyStorage != null)
        {
            _moneyValue.text = _moneyStorage.MoneyValue.ToString();
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