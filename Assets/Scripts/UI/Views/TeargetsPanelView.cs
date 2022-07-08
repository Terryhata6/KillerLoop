
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TargetsPanelView : BaseUiView
{
    #region PrivateFileds

    [SerializeField] private List<TargetIconView> _targetsIcons;
    [SerializeField] private TextMeshPro _currentTargetName;
    [SerializeField] private Sprite _bossTargetIcon;
    [SerializeField] private Sprite _bonusTargetIcon;
    [SerializeField] private Sprite _targetsBackground;

    #endregion

    private void Awake()
    {
    }

    #region AccessMethods

    public void AddTargetInfo(TargetsUIInfo info)
    {
        if (_targetsIcons.Count > 0)
        {
            for (int i = 0; i < _targetsIcons.Count; i++)
            {
                if ((i+1) == _targetsIcons.Count)
                {
                    SetTargetInfo(info, _targetsIcons[i]);
                    continue;
                }
                _targetsIcons[i].CopyValues(_targetsIcons[i + 1]);
            }
        }
    }

    public void SetCurrentTargetName(TargetsUIInfo info)
    {
        switch (info.LevelType)
        {
            case LevelType.Common:
                {
                    _currentTargetName.text = $"target {info.LevelNumber}";
                    break;
                }
            case LevelType.Boss:
                {
                    _currentTargetName.text = $"target {info.LevelNumber}";
                    break;
                }
            case LevelType.Bonus:
                {
                    _currentTargetName.text = $"bonus target";
                    break;
                }
            default:
                {
                    _currentTargetName.text = $"unknown target";
                    break;
                }
        }
    }

    #endregion

    #region PrivateMethods
    private void SetTargetInfo(TargetsUIInfo info, TargetIconView view)
    {
        switch (info.LevelType)
        {
            case LevelType.Common:
                {
                    view.Enable();
                    view.SetText(info.LevelNumber.ToString());
                    break;
                }
            case LevelType.Boss:
                {
                    view.Enable();
                    view.DisableText();
                    view.SetIcon(_bossTargetIcon);
                    break;
                }
            case LevelType.Bonus:
                {
                    view.Enable();
                    view.DisableText();
                    view.SetIcon(_bonusTargetIcon);
                    break;
                }
            default:
                {
                    view.Disable();
                    break;
                }
        }
    }
    #endregion
}