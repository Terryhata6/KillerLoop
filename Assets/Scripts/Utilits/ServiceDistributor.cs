
using System.Collections.Generic;
using UnityEngine;

public class ServiceDistributor 
{
    #region PrivateFields

    private List<IService> _services;
    private List<IConsumer> _consumers;

    private List<IService> _tempServices;
    private List<IConsumer> _tempConsumers;

    #endregion

    public ServiceDistributor() : this(new List<IService>(), new List<IConsumer>())
    {
    }
    public ServiceDistributor(List<IService> services, List<IConsumer> consumers)
    {
        _services = services;
        _consumers = consumers;
    }

    #region PublicMethods

    public void AddService(IService service)
    {
        if (_services != null 
            && !_services.Contains(service)
            && service!= null)
        {
            _services.Add(service);
        }
    }
    public void AddServices(List<IService> services)
    {
        if (_services != null && services != null)
        {
            for (int i = 0; i < services.Count; i++)
            {
                if (!_services.Contains(services[i]) && services[i] != null)
                {
                    _services.Add(services[i]);
                }
            }
        }
    }
    public void AddConsumer(IConsumer consumer)
    {
        if (_consumers != null 
            && !_consumers.Contains(consumer)
            && consumer != null)
        {
            _consumers.Add(consumer);
        }
    }
    public void AddConsumers(List<IConsumer> consumers)
    {
        if (_consumers != null && consumers != null)
        {
            for (int i = 0; i < consumers.Count; i++)
            {
                if (!_consumers.Contains(consumers[i]) && consumers[i] != null)
                {
                    _consumers.Add(consumers[i]);
                }
            }
        }
    }

    public void Distribute()
    {
        if (_services != null && _consumers != null )
        {
            SetConsumersToServices<IMoneyStorage>(_consumers, _services);
            SetConsumersToServices<INewGoodsChecker>(_consumers, _services);
            SetConsumersToServices<ITargetInfo>(_consumers, _services);
            SetConsumersToServices<IProgressValuesUpdater>(_consumers, _services);
        }
        else
        {
            Debug.Log("Miss services or consumers");
        }
    }

    #endregion

    #region PrivateMethods

    private void SetConsumersToServices<T>(List<IConsumer> consumers, List<IService> services ) where T : IService
    {
        for (int i = 0; i < services.Count; i++)
        {
            if (services[i] is T)
            {
                SetConsumersToService(consumers, (T)services[i]);
            }
        }
    }

    private void SetConsumersToService<T>(List<IConsumer> consumers, T service) where T : IService
    {
        for (int j = 0; j < consumers.Count; j++)
        {
            if (consumers[j] is IServiceConsumer<T>)
            {
                service.AddConsumer(consumers[j]);
            }
        }
    }

    private void SetConsumerToServices<T>(IServiceConsumer<T> consumer, List<IService> services) where T : IService
    {
        for (int i = 0; i < services.Count; i++)
        {
            if (services[i] is T)
            {
                services[i].AddConsumer(consumer);
            }
        }
    }

    #endregion
}