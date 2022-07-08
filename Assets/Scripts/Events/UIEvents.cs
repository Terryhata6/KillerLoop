using System;
public class UIEvents
{ 
    public static UIEvents Current = new UIEvents();

    #region ButtonActions

    public event Action OnButtonStartGame;
    public void ButtonStartGame()
    {
        OnButtonStartGame?.Invoke();
    }

    public event Action OnButtonRestartGame;
    public void ButtonRestartGame()
    {
        OnButtonRestartGame?.Invoke();
    }

    public event Action OnButtonNextLevel;
    public void ButtonNextLevel()
    {
        OnButtonNextLevel?.Invoke();
    }

    public event Action OnButtonPause;

    public void ButtonPause()
    {
        OnButtonPause?.Invoke();
    }

    public event Action OnButtonResume;

    public void ButtonResume()
    {
        OnButtonResume?.Invoke();
    }

    #endregion
}