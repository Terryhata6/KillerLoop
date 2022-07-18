
using UnityEngine;

public class RoadRunWriter : PlayerView
{

    #region PrivateFields

    #region Serialized

    [SerializeField] private bool _activeRunSaving;
    [SerializeField] private bool _activeSpawnsCreating;
    [SerializeField] private Transform _spawnPointVisualize;

    #endregion

    private CollectablesSpawnPoints _spawnsStorage;

    private RoadRunSave _savePrefab;
    private Quaternion _lastRotation;
    private PlayerState _lastState;

    #endregion

    #region PublicMethods

    public void Initialize(RoadRunSave save, CollectablesSpawnPoints spawnsStorage)
    {
        SetSavingSource(save);
        SetCollectablesSpawnsStorage(spawnsStorage);
    }

    public override void Move(Vector3 dir)
    {
        base.Move(dir);
        Save();
        CreateCollectableSpawnPoint();
    }

    #endregion

    #region PrivateMethods

    #region CreatingCollectablesSpawns

    private void SetCollectablesSpawnsStorage(CollectablesSpawnPoints storage)
    {
        if (storage != null)
        {
            _spawnsStorage = storage;
            _spawnsStorage.Reset();
            _activeSpawnsCreating = true;
        }
        else
        {
            _activeSpawnsCreating = false;
            Debug.Log("Creating spawns disable, missing spawns storage");
        }
    }

    private void CreateCollectableSpawnPoint()
    {
        if (!_activeSpawnsCreating)
        {
            return;
        }
        if (_spawnPointVisualize)
        {
            Instantiate(_spawnPointVisualize, Position, Rotation);
        }
        _spawnsStorage.SavePoint(Position);
    }

    #endregion


    #region SavingRoadRun

    private void SetSavingSource(RoadRunSave save)
    {
        if (save != null)
        {
            _savePrefab = save;
            _savePrefab.Reset();
            _activeRunSaving = true;
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
                Debug.Log("SAVE");
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