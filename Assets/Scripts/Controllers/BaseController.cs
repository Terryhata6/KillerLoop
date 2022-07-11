
public abstract class BaseController : IInitialize
{
    #region ProtectedFields

    protected bool _isActive = true;

    #endregion

    #region AccessFields
    public bool IsActive => _isActive;

    #endregion
    public BaseController()
    {
       
    }

    #region PublicMethods

    #region IInitialize
    public virtual void Initialize()
    {

    }
    #endregion

    public virtual void Enable()
    {
        SetState(true);
    }

    public virtual void Disable()
    {
        SetState(false);
    }

    #endregion

    #region ProtectedMethods
    protected virtual void SetState(bool state)
    {
        _isActive = state;
    }

    #endregion
}