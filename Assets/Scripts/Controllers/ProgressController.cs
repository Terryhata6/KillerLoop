
public class ProgressController : BaseController
{
    #region PrivateFields

    private MoneyStorage _moneyStorage;
    private LevelProgressManager _currentProgress;

    #endregion

    public ProgressController()
    {
        _moneyStorage = new MoneyStorage();
        _currentProgress = new LevelProgressManager();
    }

    #region PublicMethods

    #region Iinitialize

    public override void Initialize()
    {
        base.Initialize();
        SetEvents();
        ResetValues();
    }

    #endregion

    #endregion

    #region PrivateMethods

    private void ResetValues()
    {
        _currentProgress.InstanceMultiplying = false;
    }

    private void SetEvents()
    {
        GameEvents.Current.OnCollectableCollected += CountMoney;

        LevelEvents.Current.OnLevelWin += ProcessWin;
        LevelEvents.Current.OnLevelWin += InstanceMultiplyingMode;
        LevelEvents.Current.OnNextLevel += ResetCurrentProgress;
        LevelEvents.Current.OnNextLevel += ResetValues;

        UIEvents.Current.OnCollectX2Button += CollectMoneyX2;
        UIEvents.Current.OnCollectButton += CollectMoney;
    }

    private void CountMoney(CollectableView collectable)
    {
        _currentProgress.CountMoney(collectable);
    }

    private void InstanceMultiplyingMode()
    {
        _currentProgress.InstanceMultiplying = true;
    }

    private void ProcessWin()
    {
        _currentProgress.CalculateMultiplier();
        _currentProgress.CalculateMoneyWithMultiplier();
    }

    private void ResetCurrentProgress()
    {
        _currentProgress.ResetProgress();
    }

    private void CollectMoneyX2()
    {
        _currentProgress.CalculateMoneyWithMultiplier(2);
        CollectMoney();
    }

    private void CollectMoney()
    {
        _moneyStorage.CollectMoney();
    }

    #endregion

}