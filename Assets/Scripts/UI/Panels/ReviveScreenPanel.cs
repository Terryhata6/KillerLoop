using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReviveScreenPanel : BaseMenuPanel
{
    #region PrivateFields

    #region Serialized

    [Header("Panel")]
    [SerializeField] private GameObject _panel;

    [Header("Elements")]
    [SerializeField] private TextMeshProUGUI _reviveCounter;
    [SerializeField] private List<Animation> _animations;

    #endregion

    private float _timer;
    private float _revivingTime;
    private int _currentTime;

    #endregion

    #region PublicMethods

    public override void Initialize()
    {
        _revivingTime = 3;
    }

    public override void Hide()
    {
        if (!IsShow) return;
        _panel.gameObject.SetActive(false);
        IsShow = false;
        StopAnimations();
    }

    public override void Show()
    {
        if (IsShow) return;
        _panel.gameObject.SetActive(true);
        IsShow = true;
        ResetValues();
        StartAnimations();
        StartCoroutine(CountToRevive());
    }

    #endregion

    #region PrivateMethods
    
    private void ResetValues()
    {
        _currentTime = (int)_revivingTime;
        UpdateCounterValue(_currentTime);
    }

    private void DecrementTime()
    {
        _currentTime--;
        UpdateCounterValue(_currentTime);
    }

    private void Revive()
    {
        LevelEvents.Current.LevelContinue();
    }

    #region UIUpdates

    private void UpdateCounterValue(int value)
    {
        if (_reviveCounter)
        {
            _reviveCounter.text = $"{value}";
        }
    }

    #endregion

    private void StartAnimations()
    {
        if (_animations != null)
        {
            for (int i = 0; i < _animations.Count; i++)
            {
                _animations[i]?.Play();
            }
        }
    }

    private void StopAnimations()
    {
        if (_animations != null)
        {
            for (int i = 0; i < _animations.Count; i++)
            {
                _animations[i]?.Stop();
            }
        }
    }

    #region Coroutine

    private IEnumerator CountToRevive()
    {
        _timer = 0f;
        while (_currentTime > 0)
        {
            _timer += Time.deltaTime;
            if (_timer >= 1f)
            {
                _timer = 0f;
                DecrementTime();
            }
            yield return null;
        }
        Revive();
    }

    #endregion

    #endregion
}
