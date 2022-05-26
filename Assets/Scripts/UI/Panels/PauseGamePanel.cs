using UnityEngine;
using UnityEngine.UI;

public class PauseGamePanel : BaseMenuPanel
{
    [Header("Panel")]
    [SerializeField] private GameObject _panel;

    [Header("Elements")]
    [SerializeField] private Button _buttonRestart;
    [SerializeField] private Button _buttonResume;
        
    private void Awake()
    { 
        _buttonRestart.onClick.AddListener(UIEvents.Current.ButtonRestartGame);
        _buttonResume.onClick.AddListener(UIEvents.Current.ButtonResume);
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
        _buttonRestart.onClick.RemoveAllListeners();
        _buttonResume.onClick.RemoveAllListeners();
    }
}