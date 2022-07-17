
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
    private List<TargetsUIInfo> _tempTargetsInfos;
    #endregion

    #region PublicMethods
    public void Initialize(ITargetInfo targetInfoSource)
    {
        _indexOffset = _targetsIcons.Count / 2;
        _tempTargetsInfos = new List<TargetsUIInfo>(_targetsIcons.Count);
        UpdateTargetsInfo(targetInfoSource);
    }

    public void UpdateTargetsInfo(ITargetInfo info)
    {
        _targetInfoSource = info;
        UpdateTargetsIcons(info);
        UpdateTargetName(info);
    }

    #endregion

    #region PrivateMethods

    private void UpdateTargetsIcons(ITargetInfo info)
    {
        if (info != null)
        {
            _tempTargetsInfos.Clear();
            for (int i = _firstTargetNumber; i <= _lastTargetNumber; i++)
            {
                _tempTargetsInfos.Add(info.GetTargetInfo(i));
            }
            SetTargetsInfos(_tempTargetsInfos);
        }
    }

    private void UpdateTargetName(ITargetInfo info)
    {
        if (info != null)
        {
            _tempTargetInfo = info.GetTargetInfo(info.CurrentTargetNumber);
            SetCurrentTargetName(_tempTargetInfo);
        }
    }

    private void SetTargetsInfos(List<TargetsUIInfo> infos)
    {
        if (infos != null)
        {
            for (int i = 0; i < _targetsIcons.Count; i++)
            {
                SetTargetInfo(infos[i], _targetsIcons[i]);
            }
        }
    }

    private void SetCurrentTargetName(TargetsUIInfo info)
    {
        switch (info.LevelType)
        {
            case LevelType.Common:
                {
                    _currentTargetName.text = $"target {info.TargetId}";
                    break;
                }
            case LevelType.Boss:
                {
                    _currentTargetName.text = $"target {info.TargetId}";
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
        view.SetText(info.TargetId.ToString());
    }

    #endregion
}