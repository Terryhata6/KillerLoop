
using System.Collections;
using UnityEngine;

public class TargetEnemyView : EnemyView,
    ITargetDistanceUpdater //Service
{
    [SerializeField] private RoadRunSave _roadRunSave;
    [SerializeField] private int _savePointCounter;

    #region PublicMethods

    public override void Initialize()
    {
        base.Initialize();
        InitializeService();
        _savePointCounter = 0;
    }

    #region Actions


    public void ChangeState(EnemyState state)
    {
        switch (state)
        {
            case EnemyState.Idle:
                {
                    Stand();
                    break;
                }
            case EnemyState.Move:
                {
                    Run();
                    break;
                }
            case EnemyState.Jump:
                {
                    Jump();
                    break;
                }
            case EnemyState.Slide:
                {
                    Slide();
                    break;
                }
            case EnemyState.WallRun:
                {
                    WallRun();
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    public override void MoveWithSpeed(Vector3 dir, float speed)
    {
        SetNewRoadPosition();
        UpdateConsumersInfo();
    }

    private void SetNewRoadPosition()
    {
        if (_roadRunSave && _savePointCounter < _roadRunSave.Points.Count)
        {
            if (State != _roadRunSave.Points[_savePointCounter].State)
            {
                ChangeState(_roadRunSave.Points[_savePointCounter].State);
            }
            _transform.position = _roadRunSave.Points[_savePointCounter].Position;
            _transform.rotation = _roadRunSave.Points[_savePointCounter].Rotation;
            _savePointCounter++;
        }
    }
    #endregion

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