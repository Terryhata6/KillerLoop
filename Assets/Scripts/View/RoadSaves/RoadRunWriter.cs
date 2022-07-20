
using UnityEngine;

public class RoadRunWriter : PlayerView
{

    #region PrivateFields

    #region Serialized

    [SerializeField] private bool _activeRunSaving;
    [SerializeField] private bool _activeSpawnsCreating;
    [SerializeField] private Transform _spawnPointVisualize;
    [SerializeField] private int _spawningPeriod;
    [SerializeField] private bool _spawningOnCoolDown;

    #endregion

    private CollectablesSpawnPoints _spawnsStorage;

    private RoadRunSave _savePrefab;
    private Quaternion _lastRotation;
    private PlayerState _lastState;
 //   private float _spawningDelay;
  //  private float _counter;
//private Transform _collectablesParent;

    #endregion

    #region PublicMethods

    public void Initialize(RoadRunSave save, CollectablesSpawnPoints spawnsStorage)
    {
        InitializeFields();
        SetSavingSource(save);
 //       SetCollectablesSpawnsStorage(spawnsStorage);
 //       CreateCollectablesParent();
        Debug.Log("Writer Loaded");
    }

    public override void Move(Vector3 dir)
    {
        base.Move(dir);
        Save();
     //   CollectableSpawning();
    }

    #endregion

    #region PrivateMethods

    private void InitializeFields()
    {
  //      _spawningDelay = 1.5f;
 //       _counter = 0.0f;
    }


    //#region CreatingCollectablesSpawns

    //private void CreateCollectablesParent()
    //{
    //    if (!_spawnPointVisualize)
    //    {
    //        return;
    //    }
    //    _collectablesParent = new GameObject("Collectables").transform;
    //}

    //private void SetCollectablesSpawnsStorage(CollectablesSpawnPoints storage)
    //{
    //    if (storage != null)
    //    {
    //        _spawnsStorage = storage;
    //        _spawnsStorage.Reset();
    //        _activeSpawnsCreating = true;
    //        Debug.Log("Creating spawn points enable");
    //    }
    //    else
    //    {
    //        _activeSpawnsCreating = false;
    //        Debug.Log("Creating spawns disable, missing spawns storage");
    //    }
    //}

    //private void CollectableSpawning()
    //{
    //    if (!_activeSpawnsCreating || _spawningOnCoolDown)
    //    {
    //        return;
    //    }
    //    SpawnWithDelay(_spawnPointVisualize, _spawningDelay);
    //}

    //private void SpawnWithDelay(Transform example, float delay)
    //{
    //    if (_activeSpawnsCreating)
    //    {
    //        _counter += Time.deltaTime;
    //        if (_counter >= delay)
    //        {
    //            _counter = 0.0f;
    //            Instantiate(example,Position, Rotation,_collectablesParent);
    //        }
    //    }
    //}

    //#endregion

    #region SavingRoadRun

    private void SetSavingSource(RoadRunSave save)
    {
        if (save != null)
        {
            _savePrefab = save;
            _savePrefab.Reset();
            _activeRunSaving = true;
            Debug.Log("Saving Run");
        }
        else
        {
            _activeRunSaving = false;
            Debug.Log("Saving disable, missing save prefab");
        }
    }

    private void Save()
    {
        if (_activeRunSaving)
        {
            if (_lastRotation != Rotation || _lastState != State)
            {
                CreateRoadPoint();
                _lastRotation = Rotation;
                _lastState = State;
            }
            if (Distance >= 1.0f)
            {
                FinishSave();
            }
        }

    }

    private void CreateRoadPoint()
    {
        _savePrefab?.Points.Add(new MovementWriterPoint(Rotation, Position, DetermineState()));
    }

    private PlayerState DetermineState()
    {
        if (_lastState == State)
        {
            return PlayerState.Inactive;
        }
        return State;
    }

    private void FinishSave()
    {
        _activeRunSaving = false;
        _savePrefab?.SavePoint(new MovementWriterPoint(Rotation, Position, PlayerState.Idle));
        LevelEvents.Current.LevelWin();
    }

    #endregion

    #endregion
}