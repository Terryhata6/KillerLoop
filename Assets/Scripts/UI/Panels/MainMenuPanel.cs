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
    [SerializeField] private TextMeshProUGUI _moneyCounter;
    [SerializeField] private TargetsPanelView _targetsPanel;
    [SerializeField] private ShopIconView _shopIcon;

    //Dependencies
    private IMoneyStorage _moneyStorage;
    private INewGoodsChecker _newGoodsChecker;

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
                Debug.Log("Money");
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
    }

    private void InitializeButtonEvents()
    {
        _startButton.onClick.AddListener(UIEvents.Current.StartLevelButton);
        _shopButton.onClick.AddListener(UIEvents.Current.ShopMenuButton);
        _optionsButton.onClick.AddListener(UIEvents.Current.OptionMenuButton); 
    }



    private void Test(IMoneyStorage yes)
    {
        Debug.Log(yes.MoneyValue);
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
            _moneyCounter.text = _moneyStorage.MoneyValue.ToString();
        }
    }

    private void OnDestroy()
    {
        _startButton.onClick.RemoveAllListeners();
        _shopButton.onClick.RemoveAllListeners();
        _optionsButton.onClick.RemoveAllListeners();
    }

    #endregion

}