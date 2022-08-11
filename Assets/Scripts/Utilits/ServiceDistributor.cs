
using System.Collections.Generic;
using UnityEngine;

public class ServiceDistributor : Singleton<ServiceDistributor>
{
    #region PrivateFields

    private List<IService> _services;
    private List<IConsumer> _consumers;

    #endregion

    public ServiceDistributor() : this(new List<IService>(), new List<IConsumer>())
    {
    }
    public ServiceDistributor(List<IService> services, List<IConsumer> consumers)
    {
        _services = services;
        _consumers = consumers;
        Instance = this;
        Debug.Log("Service Distributor is Ready");
    }

    #region PublicMethods
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

    public void AddService(IService service)
    {
        if (_services != null
            && service != null
            && !_services.Contains(service))
        {
            _services.Add(service);
        }
    }

    public void AddConsumer(IConsumer consumer)
    {
        if (_consumers != null 
            && consumer != null
            && !_consumers.Contains(consumer))
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
        if (_services != null && _consumers != null)
        {
            SetConsumersToServices<IMoneyStorage>(_consumers, _services);
            SetConsumersToServices<INewGoodsChecker>(_consumers, _services);
            SetConsumersToServices<ITargetInfo>(_consumers, _services);
            SetConsumersToServices<IBeatenEnemyCounter>(_consumers, _services);
            SetConsumersToServices<ICollectedMoneyCounter>(_consumers, _services);
            SetConsumersToServices<IPlayerDistanceUpdater>(_consumers, _services);
            SetConsumersToServices<ITargetDistanceUpdater>(_consumers, _services);
            SetConsumersToServices<ICameraTargetSpawner>(_consumers, _services);
        }
        else
        {
            Debug.Log("Miss services or consumers");
        }
    }

    public void FindServicesForConsumer<T>(IServiceConsumer<T> consumer) where T : IService
    {
        AddConsumer(consumer);
        SetConsumerToServices(consumer, _services);
    }

    public void FindConsumersForService<T>(T service) where T : IService
    {
        AddService(service);
        SetConsumersToService(_consumers, service);
    }

    public void RemoveService(IService service)
    {
        if (_services.Contains(service))
        {
            _services.Remove(service);
        }
    }

    public void RemoveConsumer(IConsumer consumer)
    {
        if (_consumers.Contains(consumer))
        {
            _consumers.Remove(consumer);
        }
    }

    #endregion

    #region PrivateMethods

    private void SetConsumersToServices<T>(List<IConsumer> consumers, List<IService> services) where T : IService
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
        if (service == null)
        {
            return;
        }
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
        if (consumer == null)
        {
            return;
        }
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