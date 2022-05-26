using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : BaseMenuPanel
{
    [Header("Panel")]
    [SerializeField] private GameObject _panel;

    [Header("Elements")]
    [SerializeField] private Button _buttonStart;
    private void Awake()
    { 
        _buttonStart.onClick.AddListener(UIEvents.Current.ButtonStartGame);
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
        _buttonStart.onClick.RemoveAllListeners();
    }
}