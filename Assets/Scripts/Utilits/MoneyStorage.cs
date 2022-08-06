
public class MoneyStorage : IMoneyStorage,
    ISaveable, IServiceConsumer<ICollectedMoneyCounter>
{
    #region PrivateFields

    private int _moneyValue;
    private int _moneyToAdd;
    private int _moneyProperty
    {
        get { return _moneyValue; }
        set
        {
            _moneyValue = value;
            MoneyValueSave = value;
            UpdateConsumersInfo();
        }
    }

    #endregion

    #region AccessFields

    public SaveDataType Type => DataType; //ISaveable
    public readonly SaveDataType DataType;

    #endregion

    #region FieldsToSave

    public int MoneyValueSave;

    #endregion

    public MoneyStorage()
    {
        InitializeService();
        InitializeConsumer();
        DataType = SaveDataType.MoneyData;
    }

    public void CollectMoney()
    {
        _moneyProperty += _moneyToAdd;
        SaveProgressManager.Instance.SaveData(this);
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
        ServiceDistributor.Instance.FindServicesForConsumer<ICollectedMoneyCounter>(this);
    }

    private void SetCollectedMoney(int value)
    {
        _moneyToAdd = value;
    }

    #endregion

    #region LoadProgress

    public void SetData(MoneyStorage data)
    {
        if (data == null)
        {
            return;
        }
        _moneyProperty = data.MoneyValueSave;
    }

    #endregion
}