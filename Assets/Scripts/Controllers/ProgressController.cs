
public class ProgressController : BaseController
{
    #region PrivateFields

    private MoneyStorage _moneyStorage;
    private LevelProgressManager _currentProgress;
    private SaveProgressManager _saveProgressManager;

    #endregion

    public ProgressController()
    {
        _moneyStorage = new MoneyStorage();
        _currentProgress = new LevelProgressManager();
        _saveProgressManager = SaveProgressManager.Instance;
    }

    #region PublicMethods

    #region Iinitialize

    public override void Initialize()
    {
        base.Initialize();
        SetEvents();
        ResetValues();
        LoadProgress();
    }

    #endregion

    #endregion

    #region Initialize

    private void SetEvents()
    {
        GameEvents.Current.OnCollectableCollected += CountMoney;

        LevelEvents.Current.OnLevelWin += ProcessWin;
        LevelEvents.Current.OnLevelWin += InstanceMultiplyingMode;
        LevelEvents.Current.OnLevelLoaded += ResetValues;
        LevelEvents.Current.OnLevelStart += ResetCurrentProgress;

        UIEvents.Current.OnCollectX2Button += CollectMoneyX2;
        UIEvents.Current.OnCollectButton += CollectMoney;
    }

    private void LoadProgress()
    {
        _moneyStorage.SetData(_saveProgressManager.LoadData<MoneyStorage>(_moneyStorage.Type));
    }

    #endregion

    #region CurrentLevelManager

    private void ResetValues()
    {
        _currentProgress.InstanceMultiplying = false;
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
    }

    private void CollectMoney()
    {
        _moneyStorage.CollectMoney();
    }

    #endregion

}