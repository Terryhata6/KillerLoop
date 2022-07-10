using System;
public class UIEvents
{ 
    public static UIEvents Current = new UIEvents();

    #region ButtonActions

    #region MainMenuButtonsActions

    public event Action OnStartLevelButton;
    public void StartLevelButton()
    {
        OnStartLevelButton?.Invoke();
    }

    public event Action OnShopMenuButton;
    public void ShopMenuButton()
    {
        OnShopMenuButton?.Invoke();
    }

    public event Action OnOptionMenuButton;
    public void OptionMenuButton()
    {
        OnOptionMenuButton?.Invoke();
    }

    #endregion

    #region OptionMenuButtonsActions

    public event Action OnExitMenuButton;
    public void ExitMenuButton()
    {
        OnExitMenuButton?.Invoke();
    }

    public event Action OnSoundOffButton;
    public void SoundOffButton()
    {
        OnSoundOffButton?.Invoke();
    }

    public event Action OnSoundOnButton;
    public void SoundOnButton()
    {
        OnSoundOnButton?.Invoke();
    }

    #endregion

    #region LoseMenuButtonsActions

    public event Action OnReviveButton;
    public void ReviveButton()
    {
        OnReviveButton?.Invoke();
    }

    #endregion

    #endregion

    #region OptionalEvents

    public event Action OnToMainMenu;
    public void ToMainMenu()
    {
        OnToMainMenu?.Invoke();
    }

    #endregion
}