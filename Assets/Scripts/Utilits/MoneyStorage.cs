using UnityEngine;

public class MoneyStorage : IMoneyStorage, IServiceConsumer<ICollectedMoneyCounter>
{

    private int _moneyValue;
    private int _moneyToAdd;

    public MoneyStorage()
    {
        InitializeService();
        InitializeConsumer();
    }

    public void CollectMoney()
    {
        _moneyValue += _moneyToAdd;
        Debug.Log(_moneyToAdd);
        UpdateConsumersInfo();
    }

    #region IService

    private BaseService<IMoneyStorage> _serviceHelper;

    public int MoneyValue => _moneyValue;

    public void AddConsumer(IConsumer consumer)
    {
        _serviceHelper?.AddConsumer(consumer);
    }

    private void InitializeService()
    {
        _serviceHelper = new BaseService<IMoneyStorage>(this);
        _serviceHelper.FindConsumers();
    }

    private void UpdateConsumersInfo()
    {
        _serviceHelper?.ServeConsumers();
    }

    #endregion

    #region IServiceConsumer

    public void UseService(ICollectedMoneyCounter service)
    {
        if (service == null)
        {
            return;
        }
        SetCollectedMoney(service.MoneyCollected);
    }

    private void InitializeConsumer()
    {
        ServiceDistributor.Instance.FindServicesForConsumer(this);
    }
    private void SetCollectedMoney(int value)
    {
        _moneyToAdd = value;
    }

    #endregion
}