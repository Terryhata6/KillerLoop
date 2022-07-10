
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TargetsPanelView : BaseUiView
{
    #region PrivateFileds

    [SerializeField] private List<TargetIconView> _targetsIcons;
    [SerializeField] private TextMeshProUGUI _currentTargetName;
    [SerializeField] private Sprite _bossTargetIcon;
    [SerializeField] private Sprite _bonusTargetIcon;

    private ITargetInfo _targetInfoSource;
    private int _indexOffset;
    private int _lastTargetNumber => _targetInfoSource.CurrentTargetNumber +  _indexOffset;
    private int _firstTargetNumber => _targetInfoSource.CurrentTargetNumber - _indexOffset;

    //temp
    private TargetsUIInfo _tempTargetInfo;
    #endregion

    #region PublicMethods
    public void UpdateTargetsIcons()
    {
        if (_targetInfoSource != null)
        {
            for (int i = _firstTargetNumber; i <= _lastTargetNumber; i++)
            {
                _tempTargetInfo = _targetInfoSource.GetTargetInfo(i);
                AddTargetInfo(_tempTargetInfo);
            }
        }
    }

    public void Initialize(ITargetInfo targetInfoSource)
    {
        _indexOffset = _targetsIcons.Count / 2;
        _targetInfoSource = targetInfoSource;
    }

    public void UpdateTargetName()
    {
        if (_targetInfoSource != null)
        {
            _tempTargetInfo = _targetInfoSource.GetTargetInfo(_targetInfoSource.CurrentTargetNumber);
            SetCurrentTargetName(_tempTargetInfo);
        }
    }

    #endregion

    #region PrivateMethods

    private void AddTargetInfo(TargetsUIInfo info)
    {
        if (_targetsIcons.Count > 0)
        {
            for (int i = 0; i < _targetsIcons.Count; i++)
            {
                if ((i + 1) == _targetsIcons.Count)
                {
                    SetTargetInfo(info, _targetsIcons[i]);
                    continue;
                }
                _targetsIcons[i].CopyValues(_targetsIcons[i + 1]);
            }
        }
    }

    private void SetCurrentTargetName(TargetsUIInfo info)
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

    private void SetTargetInfo(TargetsUIInfo info, TargetIconView view)
    {
        switch (info.LevelType)
        {
            case LevelType.Common:
                {
                    SetCommonTargetIcon(info, view);
                    break;
                }
            case LevelType.Boss:
                {
                    SetBossTargetIcon(info, view);
                    break;
                }
            case LevelType.Bonus:
                {
                    SetBonusTargetIcon(info, view);
                    break;
                }
            default:
                {
                    view.Disable();
                    break;
                }
        }
    }

    private void SetBossTargetIcon(TargetsUIInfo info, TargetIconView view)
    {
        if (_bonusTargetIcon == null)
        {
            SetCommonTargetIcon(info, view);
            return;
        }
        view.Enable();
        view.DisableText();
        view.SetIcon(_bonusTargetIcon);
    }

    private void SetBonusTargetIcon(TargetsUIInfo info, TargetIconView view)
    {
        if (_bossTargetIcon == null) 
        {
            SetCommonTargetIcon(info, view);
            return;
        }
        view.Enable();
        view.DisableText();
        view.SetIcon(_bossTargetIcon);
    }

    private void SetCommonTargetIcon(TargetsUIInfo info, TargetIconView view)
    {
        view.Enable();
        view.DisableIcon();
        view.SetText(info.LevelNumber.ToString());
    }
    #endregion
}