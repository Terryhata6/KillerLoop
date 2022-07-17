using System.Collections.Generic;
using UnityEngine;

public class BaseService<T> where T : IService
{
    #region PrivateFields

    private List<IServiceConsumer<T>> _consumers;
    private T _service;
    private bool _isActive;

    #endregion
    
    public BaseService (T service)
    {
        InitializeFields();
        SetService(service);
    }

    #region PublicMethods

    public void ServeConsumers()
    {
        if (!_isActive)
        {
            return;
        }
        for (int i = 0; i < _consumers.Count; i++)
        {
            if (_consumers[i] != null)
            {
                ServeConsumer(_consumers[i]);
            }
            else
            {
                ServiceDistributor.Instance.RemoveConsumer(_consumers[i]);
            }
        }
    }
    public void AddConsumer(IConsumer consumer)
    {
        if (consumer != null
            && consumer is IServiceConsumer<T>)
        {
            SetConsumer(consumer as IServiceConsumer<T>);
        }
    }

    public void FindConsumers()
    {
        if (_service != null)
        {        
            ServiceDistributor.Instance.FindConsumersForService(_service);
        }
    }

    #endregion

    #region PrivateMethods

    private void InitializeFields()
    {
        _consumers = new List<IServiceConsumer<T>>();
    }

    private void SetService(T service)
    {
        if (service != null)
        {
            _service = service;
            Enable(true);
        }
        else
        {
            Enable(false);
        }
    }

    private void Enable(bool value)
    {
        _isActive = value;
    }

    private void SetConsumer(IServiceConsumer<T> consumer)
    {
        if (!_consumers.Contains(consumer))
        {
            _consumers.Add(consumer);
        }
    }

    private void ServeConsumer(IServiceConsumer<T> consumer)
    {
        consumer.UseService(_service);
    }

    #endregion
}
