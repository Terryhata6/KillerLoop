
public class LevelProgressManager : 
    ICollectedMoneyCounter, IMultiplierCounter, //Service
    IServiceConsumer<IBeatenEnemyCounter>, IServiceConsumer<ITargetDistanceUpdater>//Consumer
{
    #region PrivateFields

    private int _collectedMoneyCounter;
    private int _multiplier;
    private float _targetDistance;
    private int _beatenEnemyCounter;
    private float _maxTargetDistance;
    private float _targetDistanceMultiplier;

    #endregion

    public bool InstanceMultiplying;

    public LevelProgressManager()
    {
        InitializeService();
        InitializeConsumers();
        ResetProgress();
        InitializeFields();
    }

    #region PublicMethods

    public void CountMoney(CollectableView collectable)
    {
        if (!collectable)
        {
            return;
        }
        if (InstanceMultiplying)
        {
            _collectedMoneyCounter += MultiplyMoneyValue(collectable.Points, _multiplier);
        }
        else
        {
            _collectedMoneyCounter += collectable.Points;
        }
        UpdateConsumersInfo();
    }

    public void CalculateMultiplier()
    {
        _multiplier = 1
            + CalculateBeatenEnemyMultiplier()
            + CalculateTargetDistanceMultiplier();
        UpdateConsumersInfo();
    }

    public void CalculateMoneyWithMultiplier()
    {
        CalculateMoneyWithMultiplier(_multiplier);
    }

    public void CalculateMoneyWithMultiplier(int multiplier)
    {
        _collectedMoneyCounter *= multiplier;
        UpdateConsumersInfo();
    }

    public void ResetProgress()
    {
        _multiplier = 1;
        _collectedMoneyCounter = 0;
        _beatenEnemyCounter = 0;
        _targetDistance = 0.0f;
        UpdateConsumersInfo();
    }


    #endregion

    private void InitializeFields()
    {
        _targetDistanceMultiplier = 30.0f;
        _maxTargetDistance = 1.0f;
    }

    #region Calculating

    private int CalculateBeatenEnemyMultiplier()
    {
        return _beatenEnemyCounter;
    }

    private int MultiplyMoneyValue(int money, int multiplier)
    {
        return money * multiplier;
    }

    private int CalculateTargetDistanceMultiplier()
    {
        return (int)((_maxTargetDistance - _targetDistance) * _targetDistanceMultiplier);
    }

    #endregion

    #region IService

    private BaseService<ICollectedMoneyCounter> _collectedMoneyCounterHelper;
    private BaseService<IMultiplierCounter> _multiplierCounterHelper;

    public int MoneyCollected => _collectedMoneyCounter;

    public int Multiplier => _multiplier;

    public void AddConsumer(IConsumer consumer)
    {
        _multiplierCounterHelper?.AddConsumer(consumer);
        _collectedMoneyCounterHelper?.AddConsumer(consumer);
    }

    private void InitializeService()
    {
        _collectedMoneyCounterHelper = new BaseService<ICollectedMoneyCounter>(this);
        _multiplierCounterHelper = new BaseService<IMultiplierCounter>(this);
        _collectedMoneyCounterHelper.FindConsumers();
        _multiplierCounterHelper.FindConsumers();
    }

    private void UpdateConsumersInfo()
    {
        _collectedMoneyCounterHelper.ServeConsumers();
        _multiplierCounterHelper.ServeConsumers();
    }

    #endregion

    #region IConsumer

    public void UseService(IBeatenEnemyCounter service)
    {
        if(service == null)
        {
            return;
        }
        SetEnemyBeaten(service.EnemyBeaten);
    }

    public void UseService(ITargetDistanceUpdater service)
    {
        if (service == null)
        {
            return;
        }
        SetTargetDistance(service.Distance);
    }

    private void InitializeConsumers()
    {
        ServiceDistributor.Instance.FindServicesForConsumer<ITargetDistanceUpdater>(this);
        ServiceDistributor.Instance.FindServicesForConsumer<IBeatenEnemyCounter>(this);
    }

    private void SetEnemyBeaten(int value)
    {
        _beatenEnemyCounter = value;
    }

    private void SetTargetDistance(float value)
    {
        _targetDistance = value;
    }


    #endregion

}