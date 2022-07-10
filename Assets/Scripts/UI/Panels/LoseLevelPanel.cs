
using UnityEngine;
using UnityEngine.UI;

public class LoseLevelPanel : BaseMenuPanel, IWaitingCounter
{
    #region PrivateFields

    [Header("Panel")]
    [SerializeField] private GameObject _panel;

    [Header("Elements")]
    [SerializeField] private Button _reviveButton;
    [SerializeField] private ReviveCounterView _reviveCounterView;

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
    }

    public override void Show()
    {
        if (IsShow) return;
        _panel.gameObject.SetActive(true);
        IsShow = true;
        StartReviveCounter();
    }

    public void CounterFinish(bool isFinished)
    {
        if (isFinished)
        {
            UIEvents.Current.ToMainMenu();
        }
    }

    #endregion

    #region PrivateMethods

    private void Initialize()
    {
        InitializeButtonEvents();
    }

    private void InitializeButtonEvents()
    {
        BindListenerToButton(_reviveButton, StopReviveCounter);
        BindListenerToButton(_reviveButton, UIEvents.Current.ReviveButton);
    }

    private void StartReviveCounter()
    {
        if (_reviveCounterView)
        {
            _reviveCounterView.StartCounter(this);
        }
    }

    private void StopReviveCounter()
    {
        if (_reviveCounterView)
        {
            _reviveCounterView.StopCounter();
        }
    }

    private void OnDestroy()
    {
        RemoveListenersFromButton(_reviveButton);
    }

    #endregion
}