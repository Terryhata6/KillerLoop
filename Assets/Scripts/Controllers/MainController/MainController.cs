using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{
    
    [Header("Player")]
    [SerializeField] private Transform                      _playerStarterPoint;
    [SerializeField] private PlayerView                     _playerView;
    [SerializeField] private VirtualCameraView              _virtualCamera;

    [Header("Enemy")]
    [SerializeField] private List<EnemyView> _tempEnemies; //Не забудь убрать это и доделать спавн врагов - EnterAlt
    
    [Header("Settings")]
    [SerializeField] private bool                           _useMouse = true;
    [SerializeField] private bool                           _debugTestingScene = false;
    [SerializeField] private string                         _testingSceneName = "";
    
    private List<BaseController>                            _controllers = new List<BaseController>();
    
    public bool UseMouse => _useMouse;

    
    private void Awake()
    {
        ///----------Services-------------
        DontDestroyOnLoad(this.gameObject);
        
        _controllers.Add(new InputController().SetMainController(this));
        _controllers.Add(new PlayerController().SetMainController(this));
        _controllers.Add(new CameraController(_virtualCamera).SetMainController(this));
        _controllers.Add(new EnemyController().SetMainController(this));
        
        if (_debugTestingScene)
        {
            SceneManager.LoadSceneAsync(_testingSceneName, LoadSceneMode.Additive);
        }

        GetController<CameraController>().SetCameraTarget(_playerView.transform);
        if (_playerView)
        {
            GetController<PlayerController>().SetPlayerViewInstance(_playerView);
        }
        GetController<EnemyController>().SetEnemies(_tempEnemies);
    }

    private void Start()
    {
        //------SceneSettings------
        for (int i = 0; i < _controllers.Count; i++)
        {
            _controllers[i].Initialize();
        }
        
        //------SceneSettings------
        if (_playerStarterPoint != null)
        {
            _playerView.transform.position = _playerStarterPoint.position;
            //_playerStarterPoint = FindObjectOfType<PlayerStarterPosition>().transform;
        }
        else
        {
            CustomDebug.Log($"Нет стартовой точки", this.gameObject);
        }
    }

    private void Update()
    {        
        for (int i = 0; i < _controllers.Count; i++)
        {
            if (_controllers[i] is IExecute)
            {
                (_controllers[i] as IExecute).Execute();
            }
        }
        
        
    }

    private void LateUpdate()
    {        
        for (int i = 0; i < _controllers.Count; i++)
        {
            if (_controllers[i] is ILateExecute)
            {
                (_controllers[i] as ILateExecute).LateExecute();
            }
        }
    }


    /// <summary>
    /// Add controller in Controller's list
    /// </summary>
    /// <param name="controller"></param>
    public void AddController(BaseController controller)
    {
        if (!_controllers.Contains(controller))
        {
            _controllers.Add(controller);
        }
    }

    /// <summary>
    /// Remove controller from Controller's list
    /// </summary>
    /// <param name="controller"></param>
    public void RemoveController(BaseController controller)
    {
        if (_controllers.Contains(controller))
        {
            _controllers.Remove(controller);
        }
    }
 
    /// <summary>
    /// Return controller's instance from controller's list
    /// </summary>
    /// <param Type="type"></param>
    /// <returns></returns>
    public T GetController<T>() where T : BaseController
    {
        foreach (BaseController obj in _controllers)
        {
            if (obj.GetType() == typeof(T))
            {
                return (T)obj;
            }
        }
        return null;
    }

    private void OnApplicationPause(bool pause)
    {
        //GameEvents.Current.GeneralApplicationPause(pause);
    }

    #region Will Replaced or Deleted

    /// <summary>
    /// don't forget syntaxis
    /// </summary>
    public void stupid()
    {        
        //GetController<BaseController>();
        InputController a = new InputController();
        a = GetController<InputController>();
    }
    #endregion
}
