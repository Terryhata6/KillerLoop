
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CollectableController : BaseController, IExecute,
    IServiceConsumer<ICollectablesLevelInfoUpdater>
{
    #region PrivateFields

    private CollectablesLevelInfo _options;
    private List<Vector3> _spawnPoints;
    private List<CollectableView> _activeColl;
    private int _index;
    private CollectableSpawner _spawner;

    private int _dropMoneyCount;
    private float _sprayHeight;
    private float _sprayRadius;

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
 //       LevelEvents.Current.OnLevelLoaded += LoadCollectables;

        UIEvents.Current.OnToMainMenu += Disable;
    }

    private void InitializeFields()
    {
        _activeColl = new List<CollectableView>();
        _spawner = new CollectableSpawner();
        _dropMoneyCount = 10;
        _sprayHeight = 2f;
        _sprayRadius = 1.2f;
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
            _spawner.LoadCollectable(_spawnPoints[_index]);
        }
    }

    private void MoveCollectable(CollectableView collectable)
    {
        if (!collectable
            || !collectable.enabled
            || collectable.AtTarget)
        {
            DeleteMovingCollectable(collectable);
            collectable.Collected();
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

    private void DeleteMovingCollectable(CollectableView collectable)
    {
        if (_activeColl.Contains(collectable))
        {
            _activeColl.Remove(collectable);
        }
    }

    private void PrepareSpawner()
    {
        _spawner?.InitializePool(_options.PoolExamples);
    }

    #endregion

    #region SprayCollectables

    private void SprayCollectables(BaseObjectView source)
    {
        _spawner.LoadCollectablesWithSpray(
            source.Position,
            _dropMoneyCount,
            _sprayHeight,
            _sprayRadius);
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
        PrepareSpawner();
        if (_options.Spawns)
        {
            _spawnPoints = _options.Spawns?.Points;
        }
    }

    #endregion
}
