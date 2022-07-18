
using System.Collections;
using UnityEngine;

public class TargetEnemyView : EnemyView,
    ITargetDistanceUpdater //Service
{
    #region PrivateFields

    #region SerializeField

    [SerializeField] private RoadRunSave _roadRunSave;
    [SerializeField] private int _savePointCounter;

    #endregion

    private Vector3 _nextPosition;

    #endregion

    #region PublicMethods

    #region IInitialize

    public override void Initialize()
    {
        base.Initialize();
        InitializeService();
        _savePointCounter = 0;
        _nextPosition = Position;
    }

    #endregion

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
                    CheckForAWall();
                    WallRun();
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    public override void Move(Vector3 dir)
    {
        if (_roadRunSave 
            && _savePointCounter < _roadRunSave.Points.Count)
        {

            UpdateRoadPoint();
            _transform.position = Vector3.MoveTowards(Position, _nextPosition, Time.deltaTime * 2.5f);

        }
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

    private void UpdateRoadPoint()
    {

        if ((Position - _nextPosition).magnitude <= 0.07f)
        {
            UpdateState();
            UpdateRotation();
            UpdateNextPosition();
            _savePointCounter++;
        }

        _nextPosition.y = Position.y;
    }

    private void UpdateRotation()
    {
        if (_savePointCounter > 0)
        {
            _transform.rotation = _roadRunSave.Points[_savePointCounter].Rotation;
        }
    }

    private void UpdateNextPosition()
    {
        if (_savePointCounter + 1 < _roadRunSave.Points.Count)
        {
            _nextPosition = _roadRunSave.Points[_savePointCounter + 1].Position;
        }
    }

    private void UpdateState()
    {
        if (_roadRunSave.Points[_savePointCounter].State == EnemyState.Inactive)
        {
            return;
        }
        if (State == EnemyState.Jump
                && _roadRunSave.Points[_savePointCounter].State == EnemyState.Move)
        {
            return;
        }
        ChangeState(_roadRunSave.Points[_savePointCounter].State);

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