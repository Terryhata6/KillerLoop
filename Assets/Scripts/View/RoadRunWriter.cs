
using UnityEngine;

public class RoadRunWriter : PlayerView
{

    #region PrivateFields

    #region Serialized

    [SerializeField] bool _active;

    #endregion

    private RoadRunSave _savePrefab;

    #endregion

    public void Initialize(RoadRunSave save)
    {
        if (save != null)
        {
            _savePrefab = save;
            _savePrefab.Reset();
            _active = true;
        }
        else
        {
            _active = false;
            Debug.Log("Saving disable, missing save prefab");
        }
    }

    #region PublicMethods

    public override void Move(Vector3 dir)
    {
        base.Move(dir);
        Save();
    }

    public override void ChangeActionState(PlayerState state)
    {
        base.ChangeActionState(state);
        Save();
    }

    #endregion

    #region PrivateMethods

    private void Save()
    {
        if (_active)
        {
            _savePrefab?.SavePoint(new MovementWriterPoint(Position, Rotation, State));
            Debug.Log("save");
            if (Distance >= 1.0f)
            {
                FinishSave();
            }
        }

    }

    private void FinishSave()
    {
        _active = false;
        _savePrefab?.SavePoint(new MovementWriterPoint(Position, Rotation, PlayerState.Idle));
        LevelEvents.Current.LevelWin();
    }

    #endregion
}