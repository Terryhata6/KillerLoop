using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BaseController, IExecute
{
    #region PrivateFields

    private PlayerView _playerView;
    private Vector2 _positionDelta = Vector2.zero;
    private Vector2 _positionBegan = Vector2.zero;
    private Vector2 _positionEnd = Vector2.zero;
    private Dictionary<PlayerState, BasePlayerStateModel> _playerStates;

    #endregion

    public PlayerController(PlayerView player)
    {
        SetPlayerView(player);
    }

    #region PublicMethods

    public override void Initialize()
    {
        base.Initialize();
        SetEvents();
        CreateStateDictionary();
    }

    public void Execute()
    {
        if (!_playerView || !IsActive)
        {
            return;
        }
        _playerStates[_playerView.State].Execute(_positionBegan, _positionDelta, _playerView);
    }

    #endregion

    #region PrivateMethods

    private void SetPlayerView(PlayerView view)
    {
        if (view)
        {
            Enable();
            _playerView = view;
            PlayerInit(_playerView);
        }
        else
        {
            Disable();
            Debug.Log("Oops! Player view not set");
        }
    }

    private void CreateStateDictionary()
    {
        _playerStates = new Dictionary<PlayerState, BasePlayerStateModel>
        {
            {PlayerState.Idle, new PlayerIdleStateModel()},
            {PlayerState.Jumping, new PlayerJumpingStateModel()},
            {PlayerState.Move, new PlayerMovingStateModel()},
            {PlayerState.Killing, new PlayerKillStateModel()},
            {PlayerState.WallRun, new PlayerRunWallModel()},
            {PlayerState.Slide, new PlayerSlideModel()}
        };
    }

    private void SetEvents()
    {
        InputEvents.Current.OnTouchBeganEvent += UpdateBeganPosition;
        InputEvents.Current.OnTouchMovedEvent += UpdateDeltaPosition;
        InputEvents.Current.OnTouchEndedEvent += UpdateEndPosition;

        LevelEvents.Current.OnLevelStart += Enable;
        LevelEvents.Current.OnLevelLose += Disable;
        LevelEvents.Current.OnLevelFinish += Disable;
    }

    private void DeleteEvents()
    {
        InputEvents.Current.OnTouchBeganEvent -= UpdateBeganPosition;
        InputEvents.Current.OnTouchMovedEvent -= UpdateDeltaPosition;
        InputEvents.Current.OnTouchEndedEvent -= UpdateEndPosition;

        LevelEvents.Current.OnLevelStart -= Enable;
        LevelEvents.Current.OnLevelLose -= Disable;
        LevelEvents.Current.OnLevelFinish -= Disable;
    }

    private void PlayerInit(PlayerView player)
    {
        player.Stand(); //State - idle
    }

    private void UpdateBeganPosition(Vector2 beganPosition)
    {
        _positionBegan = beganPosition;
    }

    private void UpdateDeltaPosition(Vector2 deltaPosition)
    {
        _positionDelta = deltaPosition;
    }

    private void UpdateEndPosition(Vector2 endPosition)
    {
        _positionEnd = endPosition;
        UpdateDeltaPosition(Vector2.zero);
        UpdateBeganPosition(Vector2.zero);
    }

    #endregion

}