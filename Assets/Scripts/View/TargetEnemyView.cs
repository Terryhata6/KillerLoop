
using System.Collections;
using UnityEngine;

public class TargetEnemyView : EnemyView,
    ITargetDistanceUpdater //Service
{
    #region PrivateFields

    private bool _spawningCollectables;
    [SerializeField] private float _itemSpawnDelay;
    private float _counter;

    #endregion

    #region PublicMethods

    #region IInitialize

    public override void Initialize()
    {
        base.Initialize();
        InitializeService();
        InitializeFields();
        StartStateAction(EnemyState.Move);
    }

    #endregion

    #region Actions

    public override void MoveToNextPoint(float speed)
    {
        base.MoveToNextPoint(speed);
        SpawnCollectables();
        UpdateConsumersInfo();
    }

    #endregion

    #region Coroutine

    public override IEnumerator DeadAnimation()
    {
        yield return base.DeadAnimation();
        LevelEvents.Current.LevelWin();
    }

    #endregion

    #endregion

    #region PrivateMethods

    private void InitializeFields()
    {
        _counter = 0.0f;
        _spawningCollectables = true;
    }

    private void SpawnCollectables()
    {

        if (_spawningCollectables)
        {
            _counter += Time.deltaTime;
            if(_counter >= _itemSpawnDelay)
            {
                _counter = 0.0f;
                CollectableSpawner.Instance.LoadCollectable(Position + Vector3.up);
            }
        }
    }

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