using UnityEngine;
using UnityEngine.UI;

public class InGamePanel : BaseMenuPanel
{
    [Header("Panel")]
    [SerializeField] private GameObject _panel;

    [Header("Elements")]
    [SerializeField] private Button _buttonPause;
        
    private void Awake()
    { 
        _buttonPause.onClick.AddListener(UIEvents.Current.ButtonPause);
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

    private void OnDestroy()
    {
        _buttonPause.onClick.RemoveAllListeners();
    }
}