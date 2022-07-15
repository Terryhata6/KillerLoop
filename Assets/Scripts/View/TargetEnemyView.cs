
using System.Collections.Generic;
using UnityEngine;

public class TargetEnemyView : EnemyView, ITargetDistanceUpdater
{
    #region PrivateFields

    private List<IServiceConsumer<ITargetDistanceUpdater>> _consumers;

    #endregion

    #region AccessFields

    public float Distance => (float)_splineTracer?.result.percent;

    #endregion

    #region PublicMethods

    public override void Initialize()
    {
        base.Initialize();
        _consumers = new List<IServiceConsumer<ITargetDistanceUpdater>>();
        ServiceDistributor.Instance.FindConsumersForService(this);
    }

    public override void Move(Vector3 dir)
    {
        base.Move(dir);
        UpdateConsumersInfo();
    }

    public void AddConsumer(IConsumer consumer)
    {
        if (consumer != null
            && consumer is IServiceConsumer<ITargetDistanceUpdater>)
        {
            SetConsumer(consumer as IServiceConsumer<ITargetDistanceUpdater>);
        }
    }
    #endregion

    #region PrivateMethods

    private void UpdateConsumersInfo()
    {
        for (int i = 0; i < _consumers.Count; i++)
        {
            UpdateConsumerInfo(_consumers[i]);
        }
    }

    private void UpdateConsumerInfo(IServiceConsumer<ITargetDistanceUpdater> consumer)
    {
        if (consumer == null)
        {
            _consumers.Remove(consumer);
        }
        else
        {
            consumer.UseService(this);
        }
    }

    private void SetConsumer(IServiceConsumer<ITargetDistanceUpdater> consumer)
    {
        if (!_consumers.Contains(consumer))
        {
            _consumers.Add(consumer);
        }
    }

    private void OnDestroy()
    {
        ServiceDistributor.Instance.RemoveService(this);
    }

    #endregion
}