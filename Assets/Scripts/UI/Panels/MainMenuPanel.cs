using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : BaseMenuPanel
{
    [Header("Panel")]
    [SerializeField] private GameObject _panel;

    [Header("Elements")]
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _shopButton;
    [SerializeField] private Button _optionsButton;
    [SerializeField] private List<Animation> _menuAnimations;
    [SerializeField] private Text _moneyCounter;
    [SerializeField] private TargetsPanelView _targetsPanel;

    private void Awake()
    { 
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

    private void ProcessTargetInfo(TargetsUIInfo info)
    {
        _targetsPanel.AddTargetInfo(info);
    }

    private void OnDestroy()
    {
    }
}