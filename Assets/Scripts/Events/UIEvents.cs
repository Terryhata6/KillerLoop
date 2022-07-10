using System;
public class UIEvents
{ 
    public static UIEvents Current = new UIEvents();

    #region ButtonActions

    #region MainMenuButtonsActions

    public event Action OnStartLevelButton;
    public void StartLevelButton()
    {
        OnStartLevelButton?.Invoke();
    }

    public event Action OnShopMenuButton;
    public void ShopMenuButton()
    {
        OnShopMenuButton?.Invoke();
    }

    public event Action OnOptionMenuButton;
    public void OptionMenuButton()
    {
        OnOptionMenuButton?.Invoke();
    }

    #endregion



    #endregion

    #region ComponentRequires

    #region MenuRequires

    public event Action<IMoneyStorage> OnMoneyStorageRequire;
    public void MoneyStorageRequire(IMoneyStorage storage)
    {
        OnMoneyStorageRequire?.Invoke(storage);
    }

    public event Action<INewGoodsChecker> OnNewGoodsCheckerRequire;
    public void NewGoodsCheckerRequire(INewGoodsChecker checker)
    {
        OnNewGoodsCheckerRequire?.Invoke(checker);
    }

    public event Action<ITargetInfo> OnTargetInfoRequire;
    public void TargetInfoRequire(ITargetInfo info)
    {
        OnTargetInfoRequire?.Invoke(info);
    }

    #endregion

    #endregion
}