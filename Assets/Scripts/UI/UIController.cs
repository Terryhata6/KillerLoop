using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    #region PrivateFields

    #region Serialized

    [SerializeField] private List<BaseMenuPanel> _menues;

    #endregion

    private ServiceDistributor _serviceDistributor;

    #endregion

    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        UIEvents.Current.ToMainMenu();
    }

    #region PrivateMethods

    #region ActionsReaction

    private void OpenGameMenu()
    {
        SwitchUI(UIState.MainMenu);
    }

    private void ReviveScreen()
    {
        SwitchUI(UIState.Revive);
    }

    private void ContinueGame()
    {
        SwitchUI(UIState.InGame);
    }

    private void OpenShopMenu()
    {
        SwitchUI(UIState.ShopMenu);
    }
    private void StartGame()
    {
        SwitchUI(UIState.InGame);
        LevelEvents.Current.LevelStart();
    }

    private void WinLevel()
    {
        SwitchUI(UIState.WinLevel);
    }

    private void LoseLevel()
    {
        SwitchUI(UIState.LoseLevel);
    }

    private void NextLevel()
    {
        LevelEvents.Current.NextLevel();
    }

    private void OpenOptionsMenu()
    {
        SwitchUI(UIState.OptionsMenu);
    }

    #endregion

    #region Initialize

    private void Initialize()
    {
        SetEvents();
        InitializePanels();
        SetServiceDistributor();
        SetConsumersToDistributor();
        HideUI();
    }

    private void SetEvents()
    {
        LevelEvents.Current.OnLevelWin += WinLevel;
        LevelEvents.Current.OnLevelLoaded += OpenGameMenu;
        LevelEvents.Current.OnLevelLose += LoseLevel;
        LevelEvents.Current.OnLevelContinue += ContinueGame;
        LevelEvents.Current.OnLevelRestart += OpenGameMenu;

        UIEvents.Current.OnToMainMenu += OpenGameMenu; //Исправь Enter-alt
        UIEvents.Current.OnStartLevelButton += StartGame;
        UIEvents.Current.OnExitShopButton += OpenGameMenu;
        UIEvents.Current.OnExitOptionsButton += OpenGameMenu;
        UIEvents.Current.OnOptionMenuButton += OpenOptionsMenu;
        UIEvents.Current.OnShopMenuButton += OpenShopMenu;
        UIEvents.Current.OnCollectButton += NextLevel;
        UIEvents.Current.OnCollectX2Button += NextLevel;

        GameEvents.Current.OnRevive += ReviveScreen;
    }

    private void SetServiceDistributor()
    {
        _serviceDistributor = ServiceDistributor.Instance;
    }

    private void InitializePanels()
    {
        for (int i = 0; i < _menues.Count; i++)
        {
            _menues[i].Initialize();
        }
    }

    #endregion

    private void SetConsumersToDistributor()
    {
        if (_serviceDistributor == null)
        {
            return;
        }
        for (int i = 0; i < _menues.Count; i++)
        {
            if (_menues[i] 
                && _menues[i] is IConsumer)
            {
                _serviceDistributor.AddConsumer(_menues[i] as IConsumer);
            }
        } 
    }

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
                SwitchMenu(typeof(WinGamePanel));
                break;
            case UIState.Revive:
                SwitchMenu(typeof(ReviveScreenPanel));
                break;
            case UIState.ShopMenu:
                SwitchMenu(typeof(ShopMenuPanel));
                break;
            case UIState.OptionsMenu:
                SwitchMenu(typeof(OptionsMenuPanel));
                break;
            default:
                {
                    Debug.Log("Unkown Type of UI Panel");
                    break;
                }
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