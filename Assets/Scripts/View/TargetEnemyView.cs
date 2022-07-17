
using System.Collections;
using UnityEngine;

public class TargetEnemyView : EnemyView,
    ITargetDistanceUpdater //Service
{

    #region PublicMethods

    public override void Initialize()
    {
        base.Initialize();
        InitializeService();
    }

    public override void Move(Vector3 dir)
    {
        base.Move(dir);
        UpdateConsumersInfo();
    }

    public override IEnumerator DeadAnimation()
    {
        yield return base.DeadAnimation();
        LevelEvents.Current.LevelWin();
    }

    #endregion

    #region PrivateMethods

    private void OnDestroy()
    {
        ServiceDistributor.Instance.RemoveService(this);
    }

    #endregion

    #region IService

    private BaseService<ITargetDistanceUpdater> _serviceHelper;

    public float Distance => (float)_splineTracer?.result.percent;

    public void AddConsumer(IConsumer consumer)
    {
        _serviceHelper?.AddConsumer(consumer);
    }

    private void InitializeService()
    {
        _serviceHelper = new BaseService<ITargetDistanceUpdater>(this);
        _serviceHelper.FindConsumers();
    }

    private void UpdateConsumersInfo()
    {
        _serviceHelper?.ServeConsumers();
    }

    #endregion
}