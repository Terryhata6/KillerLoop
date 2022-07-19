
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
    private bool _spawningCollectables;
    private float _spawningDelay;
    private float _counter;

    #endregion

    #region PublicMethods

    #region IInitialize

    public override void Initialize()
    {
        base.Initialize();
        InitializeService();
        InitializeFields();
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
        if (!_roadRunSave 
            && _savePointCounter >= _roadRunSave.Points.Count)
        {
            return;
        }
        UpdateRoadPoint();
        MoveToNextPoint(_nextPosition);
        SpawnCollectables();
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
        _savePointCounter = 0;
        _nextPosition = Position;
        _spawningDelay = 1.5f;
        _counter = 0.0f;
        _spawningCollectables = true;
    }

    private void MoveToNextPoint(Vector3 nextPosition)
    {
        _transform.position = Vector3.MoveTowards(Position, nextPosition, Time.deltaTime * 2.5f);
    }

    private void UpdateRoadPoint()
    {
        if (_savePointCounter >= _roadRunSave.Points.Count)
        {
            return;
        }
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

    private void SpawnCollectables()
    {

        if (_spawningCollectables)
        {
            _counter += Time.deltaTime;
            if(_counter >= _spawningDelay)
            {
                _counter = 0.0f;
                CollectableSpawner.Instance.LoadCollectable(Position);
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