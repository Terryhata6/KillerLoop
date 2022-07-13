using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuPanel : BaseMenuPanel
{
    #region PrivateFields

    #region Serialized

    [Header("Panel")]
    [SerializeField] private GameObject _panel;

    [Header("Elements")]
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _soundOffButton;
    [SerializeField] private Button _soundOnButton;

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
    }

    #endregion

    #region PrivateMethods

    private void SetButtonEvents()
    {
        BindListenerToButton(_exitButton, UIEvents.Current.ExitOptionsButton);
        BindListenerToButton(_soundOffButton, UIEvents.Current.SoundOffButton);
        BindListenerToButton(_soundOnButton, UIEvents.Current.SoundOnButton);
        BindListenerToButton(_soundOffButton, ChangeSoundButton);
        BindListenerToButton(_soundOnButton, ChangeSoundButton);
    }

    private void ChangeSoundButton()
    {
        if (_soundOffButton && _soundOffButton.IsActive())
        {
            EnableSoundOnButton();
        }
        else if (_soundOnButton && _soundOnButton.IsActive())
        {
            EnableSoundOffButton();
        }
    }

    private void EnableSoundOnButton()
    {
        _soundOffButton.gameObject.SetActive(false);
        _soundOnButton.gameObject.SetActive(true);
    }

    private void EnableSoundOffButton()
    {
        _soundOffButton.gameObject.SetActive(true);
        _soundOnButton.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        RemoveListenersFromButton(_exitButton);
        RemoveListenersFromButton(_soundOnButton);
        RemoveListenersFromButton(_soundOffButton);
    }

    #endregion
}