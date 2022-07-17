using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BaseController, IExecute, 
    IPlayerSpawner, //Service
    IServiceConsumer<IPlayerLevelInfoUpdater> //Consumer
{
    #region PrivateFields

    private PlayerView _playerView;
    private PlayerView _playerPrefab;
    private PlayerView _temp;
    private Vector3 _tempPos;
    private PlayerLevelInfo _playerLevelInfo;
    private Vector2 _positionDelta = Vector2.zero;
    private Vector2 _positionBegan = Vector2.zero;
    private Vector2 _positionEnd = Vector2.zero;
    private Dictionary<PlayerState, BasePlayerStateModel> _playerStates;

    #endregion
    public PlayerController(PlayerView prefab) : base()
    {
        SetPlayerPrefab(prefab);
    }

    #region PublicMethods

    public override void Initialize()
    {
        base.Initialize();
        SetEvents();
        CreateStateDictionary();
        InitializeService();
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

    #region Initialize

    private void CreateStateDictionary()
    {
        _playerStates = new Dictionary<PlayerState, BasePlayerStateModel>
        {
            {PlayerState.Idle, new PlayerIdleStateModel()},
            {PlayerState.Jumping, new PlayerJumpingStateModel()},
            {PlayerState.Move, new PlayerMovingStateModel()},
            {PlayerState.WallRun, new PlayerRunWallModel()},
            {PlayerState.Slide, new PlayerSlideModel()},
            {PlayerState.Inactive, new PlayerInactiveStateModel()}
        };
    }

    private void SetEvents()
    {
        InputEvents.Current.OnTouchBeganEvent += UpdateBeganPosition;
        InputEvents.Current.OnTouchMovedEvent += UpdateDeltaPosition;
        InputEvents.Current.OnTouchEndedEvent += UpdateEndPosition;

        LevelEvents.Current.OnLevelStart += Enable;
        LevelEvents.Current.OnLevelContinue += Enable;
        LevelEvents.Current.OnLevelLose += Disable;
        LevelEvents.Current.OnLevelWin += Disable;
        LevelEvents.Current.OnLevelLoaded += LoadNewPlayer;

        UIEvents.Current.OnToMainMenu += Disable;
        UIEvents.Current.OnReviveButton += RevivePlayer;
    }

    #endregion

    #region UpdateMethods

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

    #region PlayerManage

    private void SetPlayerPrefab(PlayerView prefab)
    {
        if (prefab)
        {
            Enable();
            _playerPrefab = prefab;
        }
        else
        {
            Disable();
            Debug.Log("Oops! Player prefab is null");
        }
    }

    private void LoadNewPlayer()
    {
        DeleteCurrentPlayer();
        _playerView = SpawnPlayer(_playerLevelInfo.Spawn);
        if (_playerView)
        {
            PlayerInitialize(_playerView);
            UpdateConsumerInfo();
        }
    }

    private PlayerView SpawnPlayer(Transform spawn)
    {
        if (_playerPrefab
            && spawn)
        {
            _temp =  GameObject.Instantiate(_playerPrefab,
                spawn.position,
                spawn.rotation);
            return _temp;
        }
        else
        {
            Debug.Log($"Player Spawn Data missing: Spawn{spawn}, Prefab{_playerPrefab}");
            return null;
        }
    }

    private void PlayerInitialize(PlayerView player)
    {
        player.Initialize(_playerLevelInfo);
    }

    private void RevivePlayer()
    {
        _tempPos = GetRevivePosition(_playerView);
        LoadNewPlayer();
        LoadRevivePosition(_playerView);
        GameEvents.Current.Revive();
    }

    private void DeleteCurrentPlayer()
    {
        if (_playerView)
        {
            _playerView.Delete();
        }
    }

    private Vector3 GetRevivePosition(PlayerView player)
    {
        Debug.Log(player?.SaveRevivePosition());
        return player?.SaveRevivePosition() ?? Vector3.zero;
    }

    private void LoadRevivePosition(PlayerView player)
    {
        if (player)
        {
            if (_tempPos == Vector3.zero)
            {
                player.LoadRevivePosition(_playerLevelInfo.Spawn.position);
            }
            else
            {
                player.LoadRevivePosition(_tempPos);
            }
            player.Jump();
        }
    }

    #endregion

    #endregion

    #region IService

    private BaseService<IPlayerSpawner> _serviceHelper;
    public PlayerView CurrentPlayer => _playerView;

    public void AddConsumer(IConsumer consumer)
    {
        _serviceHelper?.AddConsumer(consumer);
    }

    private void InitializeService()
    {
        _serviceHelper = new BaseService<IPlayerSpawner>(this);
        _serviceHelper.FindConsumers();
    }

    private void UpdateConsumerInfo()
    {
        _serviceHelper?.ServeConsumers();
    }

    #endregion

    #region IConsumer

    public void UseService(IPlayerLevelInfoUpdater service)
    {
        if (service != null)
        {
            _playerLevelInfo = service.PlayerInfo;
        }
    }

    #endregion

}