using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController : IInitialize
{
    public BaseController()
    {
       
    }

    protected bool _isActive = true;

    public bool IsActive => _isActive;
    protected virtual void SetState(bool state)
    {
        _isActive = state;
    }

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
}