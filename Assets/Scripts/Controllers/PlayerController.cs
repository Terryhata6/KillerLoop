using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BaseController, IExecute
{
    private PlayerView _playerView;
    private Vector2 _positionDelta = Vector2.zero;
    private Vector2 _positionBegan = Vector2.zero;
    private Vector2 _positionEnd = Vector2.zero;
    private Dictionary<PlayerState, BasePlayerStateModel> _playerStates;

    public Vector2 PositionDelta => _positionDelta;
    public Vector2 PositionBegan => _positionBegan;
    public Vector2 PositionEnd => _positionEnd;


    public PlayerController()
    {

    }



    public override void Initialize()
    {
        base.Initialize();
        InputEvents.Current.OnTouchBeganEvent += UpdateBeganPosition;
        InputEvents.Current.OnTouchMovedEvent += UpdateDeltaPosition;
        InputEvents.Current.OnTouchEndedEvent += UpdateEndPosition;

        _playerStates = new Dictionary<PlayerState, BasePlayerStateModel>
        {
            {PlayerState.Idle, new PlayerIdleStateModel()},
            {PlayerState.Jumping, new PlayerJumpingStateModel()},
            {PlayerState.Move, new PlayerMovingStateModel()},
            {PlayerState.WallRunLeft, new PlayerWallRunLeftModel()},
            {PlayerState.Slide, new PlayerSlideStateModel()},
            {PlayerState.JumpOver, new PlayerJumpOverStateModel()},
            {PlayerState.FallBack, new PlayerFallBackStateModel()},
            {PlayerState.Dead, new PlayerDeadStateModel()}
        };
    }

    public void Execute()
    {
        if (_playerView == null)
        {
            return;
        }
        _playerStates[_playerView.State].Execute(this, _playerView);
    }

    public void SetPlayerViewInstance(PlayerView view)
    {
        _playerView = view;
    }

    private void UpdateBeganPosition(Vector2 beganPosition)
    {
        _positionBegan = beganPosition;
        if (_playerView.State == PlayerState.Idle)
        {
            _playerView.SetState(PlayerState.Move);
        }
    }

    private void UpdateDeltaPosition(Vector2 deltaPosition)
    {
        _positionDelta = deltaPosition;
        if (_playerView.State == PlayerState.Idle)
        {
            _playerView.SetState(PlayerState.Move);
        }
    }

    private void UpdateEndPosition(Vector2 endPosition)
    {
        _positionEnd = endPosition;
        UpdateBeganPosition(Vector2.zero);
        UpdateDeltaPosition(Vector2.zero);
        if (_playerView.State == PlayerState.Move)
        {
            _playerView.SetState(PlayerState.Idle);
        }
    }
}