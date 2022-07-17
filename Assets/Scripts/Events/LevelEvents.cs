using System;

public class LevelEvents
{
    public static LevelEvents Current = new LevelEvents();

    public Action OnLevelStart;
    public void LevelStart()
    {
        OnLevelStart?.Invoke();
    }

    public Action OnLevelRestart;
    public void LevelRestart()
    {
        OnLevelRestart?.Invoke();
    }

    public Action OnLevelWin;
    public void LevelWin()
    {
        OnLevelWin?.Invoke();
    }

    public Action OnLevelLose;
    public void LevelLose()
    {
        OnLevelLose?.Invoke();
    }

    public Action OnNextLevel;
    public void NextLevel()
    {
        OnNextLevel?.Invoke();
    }

    public Action OnLevelLoaded;
    public void LevelLoaded()
    {
        OnLevelLoaded?.Invoke();
    }

    public Action OnLevelContinue;
    public void LevelContinue()
    {
        OnLevelContinue?.Invoke();
    }
}