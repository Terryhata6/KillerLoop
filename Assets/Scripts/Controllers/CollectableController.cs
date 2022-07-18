
using System.Collections.Generic;
using UnityEngine;

public class CollectableController : BaseController, IExecute,
    IServiceConsumer<ICollectablesLevelInfoUpdater>
{
    #region PrivateFields

    private CollectablesLevelInfo _options;
    private List<Vector3> _spawnPoints;
    private CollectableView _tempColletable;
    private ObjectPool<CollectableView> _pool;
    private List<CollectableView> _activeColl;
    private int _index;
    private List<Transform> _tempTransforms;

    private int _dropMoneyCount;
    private float _sprayHeight;
    private float _sprayRadius;
    private int _sizeOfObjectPool;

    #endregion

    public CollectableController() : base()
    {
    }

    #region IInitialize

    public override void Initialize()
    {
        SetEvents();
        InitializeFields();

        Debug.Log("CollectableController start");
    }

    #endregion

    #region IExecute

    public void Execute()
    {
        if (!IsActive)
        {
            return;
        }
        for (_index = 0; _index < _activeColl.Count; _index++)
        {
            MoveCollectable(_activeColl[_index]); // движение money
        }
    }

    #endregion

    #region PrivateMethods

    #region Initialize

    private void SetEvents()
    {
        GameEvents.Current.OnEnemyDead += SprayCollectables;
        GameEvents.Current.OnCollectableTriggered += SetMovingCollectable;

        LevelEvents.Current.OnLevelStart += Enable;
        LevelEvents.Current.OnLevelContinue += Enable;
        LevelEvents.Current.OnLevelLoaded += Disable;
        LevelEvents.Current.OnLevelLoaded += LoadCollectables;

        UIEvents.Current.OnToMainMenu += Disable;
    }

    private void InitializeFields()
    {
        _pool = new ObjectPool<CollectableView>();
        _activeColl = new List<CollectableView>();
        _tempTransforms = new List<Transform>();
        _sizeOfObjectPool = 40;
        _dropMoneyCount = 10;
        _sprayHeight = 3f;
        _sprayRadius = 3f;
    }


    #endregion

    #region CollectableManage

    private void LoadCollectables()
    {
        if (_spawnPoints == null)
        {
            return;
        }
        for (_index = 0; _index < _spawnPoints.Count; _index++)
        {
            LoadCollectable(_spawnPoints[_index]);
        }
    }

    private CollectableView LoadCollectable(Vector3 position)
    {
        _tempColletable = _pool.GetObject(position);
        CollectableInit(_tempColletable);
        return _tempColletable;
    }

    private void CollectableInit(CollectableView collectable)
    {
        if (!collectable)
        {
            return;
        }
        collectable.Initialize();
    }

    private void MoveCollectable(CollectableView collectable)
    {
        if (!collectable
            || !collectable.enabled
            || collectable.AtTarget)
        {
            DeleteMovingMoney(collectable);
        }
        else
        {
            collectable.MoveToTarget();
        }
    }

    private void SetMovingCollectable(CollectableView collectable)
    {
        if (!_activeColl.Contains(collectable))
        {
            _activeColl.Add(collectable);
        }

    }

    private void DeleteMovingMoney(CollectableView collectable)
    {
        if (_activeColl.Contains(collectable))
        {
            _activeColl.Remove(collectable);
        }
    }


    #endregion

    private void PoolInit()
    {
        _pool.CleanPool();
        if (_options.PoolExamples != null)
        {
            Debug.Log("Collectables pool initialize");
            _pool.Initialize(_options.PoolExamples, _sizeOfObjectPool);
        }
        else
        {
            Debug.Log("Oops! Pool examples missing");
        }
    }

    #region SprayCollectables

    private void SprayCollectables(BaseObjectView source)
    {
        CollectableSpray.Instance.SprayCollectables(
            GetMoneyAtPosition(_dropMoneyCount, source.Position),
            _sprayRadius,
            _sprayHeight);
    }

    private List<Transform> GetMoneyAtPosition(int count, Vector3 basePosition)
    {
        _tempTransforms.Clear();
        for (int _index = 0; _index < count; _index++)
        {
            _tempTransforms.Add(LoadCollectable(basePosition).transform);
        }
        return _tempTransforms;
    }

    #endregion

    #endregion

    #region IConsumer

    public void UseService(ICollectablesLevelInfoUpdater service)
    {
        if (service == null)
        {
            return;
        }
        SetCollectablesInfo(service.CollectablesInfo);
    }

    private void SetCollectablesInfo(CollectablesLevelInfo options)
    {
        Debug.Log("Collectables Parameters set");
        _options = options;
        PoolInit();
        if (_options.Spawns)
        {
            _spawnPoints = _options.Spawns?.Points;
        }
    }

    #endregion
}
