using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    #region PrivateFields

    [SerializeField] private List<BaseMenuPanel> _menues;

    #endregion

    public void Awake()
    {
        UIEvents.Current.OnButtonStartGame += StartGame;
        UIEvents.Current.OnButtonRestartGame += RestartGame;
        UIEvents.Current.OnButtonNextLevel += NextLevel;

        LevelEvents.Current.OnLevelFinish += WinLevel;
        LevelEvents.Current.OnLevelChanged += OpenGameMenu;
        LevelEvents.Current.OnLevelLose += LoseLevel;
        
        HideUI();
        OpenGameMenu();
    }

    #region ActionsReaction

    private void OpenGameMenu()
    {
        SwitchUI(UIState.MainMenu);
    }

    private void StartGame()
    {
        //Time.timeScale = 1.0f;
        SwitchUI(UIState.InGame);
        LevelEvents.Current.LevelStart();
    }

    private void WinLevel()
    {
        //Time.timeScale = 0.0f;
        SwitchUI(UIState.WinLevel);
    }

    private void LoseLevel()
    {
        //Time.timeScale = 0.0f;
        SwitchUI(UIState.LoseLevel);
    }

    private void NextLevel()
    {
        LevelEvents.Current.ChangeLevel();
    }

    private void RestartGame()
    {
        LevelEvents.Current.LevelRestart();
        SwitchUI(UIState.MainMenu);
    }

    #endregion

    #region OptionalMethods

    private void HideUI()
    {
        for (int i = 0; i < _menues.Count; i++)
        {
            _menues[i].Hide();
        }
    }

    #endregion

    #region SwitchPanels
    private void SwitchUI(UIState state)
    {
        if (_menues.Count == 0)
        {
            Debug.LogWarning("There is no menues to switch.");
        }
        switch (state)
        {
            case UIState.MainMenu:
                SwitchMenu(typeof(MainMenuPanel));
                break;
            case UIState.InGame:
                SwitchMenu(typeof(InGamePanel));
                break;
            case UIState.LoseLevel:
                SwitchMenu(typeof(LoseLevelPanel));
                break;
            case UIState.WinLevel:
                SwitchMenu(typeof(WinLevelPanel));
                break;
        }
    }
    private void SwitchMenu(System.Type type)
    {
        bool isFound = false;

        for (int i = 0; i < _menues.Count; i++)
        {
            if (_menues[i].GetType() == type)
            {
                _menues[i].Show();
                isFound = true;
            }
            else
            {
                _menues[i].Hide();
            }

            if (i == _menues.Count - 1f && !isFound)
            {
                Debug.LogWarning($"Oops! Menu {type} not found");
            }
        }
    }
    #endregion
}