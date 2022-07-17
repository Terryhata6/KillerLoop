
using UnityEngine;
using UnityEngine.UI;

public class LoseLevelPanel : BaseMenuPanel, IWaitingCounter
{
    #region PrivateFields

    #region Serialized

    [Header("Panel")]
    [SerializeField] private GameObject _panel;

    [Header("Elements")]
    [SerializeField] private Button _reviveButton;
    [SerializeField] private ReviveCounterView _reviveCounterView;

    #endregion

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
        StartReviveCounter();
    }

    public void CounterFinish(bool isFinished)
    {
        if (isFinished)
        {
            LevelEvents.Current.LevelRestart();
        }
    }

    #endregion

    #region PrivateMethods

    private void SetButtonEvents()
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