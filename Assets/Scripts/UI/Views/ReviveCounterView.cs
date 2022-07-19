
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReviveCounterView : BaseUiView
{
    #region PrivateFileds

    [SerializeField] private Animation _heartAnimation;
    [SerializeField] private Image _fillAmountImage;
    [SerializeField] private TextMeshProUGUI _counter;

    private float _maxImageFillValue;
    private float _fillingMultiplier;
    private float _countingTime;
    private float _timer;
    private bool _isCounting;
    private IWaitingCounter _source;

    #endregion

    private void Awake()
    {
        _maxImageFillValue = 1.0f;
        _countingTime = 10.0f;
        _fillingMultiplier = 1 / _countingTime;
        _isCounting = false;
    }

    #region PublicMethods

    public void StartCounter(IWaitingCounter source)
    {
        PrepareCounter(source);
        StartAnimation();
        StartCoroutine(Counting());
    }

    public void StopCounter()
    {
        CountingFinish(false);
        StopCoroutine(Counting());
    }

    #endregion 

    #region PrivateMethods

    private void CountingFinish(bool value)
    {
        EndCounting();
        if (_source != null)
        {
            _source.CounterFinish(value);
        }
    }
    private void EndCounting()
    {
        StopAnimation();
        _isCounting = false;
    }
    private void PrepareCounter(IWaitingCounter source)
    {
        FillImage(_maxImageFillValue);
        _isCounting = true;
        _source = source;
        _timer = 10.0f;
        SetCounterValue(_timer);
    }
    private void FillImage(float value)
    {
        if (_fillAmountImage)
        {
            _fillAmountImage.fillAmount = value;
        }
    }

    private void SetCounterValue(float time)
    {
        if (_counter)
        {
            _counter.text = ((int)time).ToString();
        }
    }
    private void StartAnimation()
    {
        if (_heartAnimation)
        {
            _heartAnimation.Play();
        }
    }
    private void StopAnimation()
    {
        if (_heartAnimation)
        {
            _heartAnimation.Stop();
        }
    }

    #endregion

    #region Coroutine

    private IEnumerator Counting()
    {
        _timer = _countingTime;
        while (_isCounting)
        {
            _timer -= Time.deltaTime;
            FillImage(_timer * _fillingMultiplier);
            if (_timer % 1 <= 0.05f)
            {
                SetCounterValue(_timer);
            }
            if (_timer <= 0.0f)
            {
                CountingFinish(true);
            }
            yield return null;
        }
    }

    #endregion
}