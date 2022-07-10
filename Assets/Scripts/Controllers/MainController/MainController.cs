using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{

    [Header("Player")]
    [SerializeField] private Transform _playerStarterPoint;
    [SerializeField] private PlayerView _playerView;
    [SerializeField] private VirtualCameraView _virtualCamera;

    [Header("Enemy")]
    [SerializeField] private List<EnemyView> _tempEnemies; //Не забудь убрать это и доделать спавн врагов - EnterAlt

    [Header("Settings")]
    [SerializeField] private bool _debugTestingScene = false;
    [SerializeField] private string _testingSceneName = "";

    private List<BaseController> _controllers;
    private List<IService> _services;

    private void Awake()
    {
        ///----------Services-------------
        _services = new List<IService>();
        _controllers = new List<BaseController>();

        DontDestroyOnLoad(gameObject);
        AddController(new InputController());
        AddController(new PlayerController());
        AddController(new CameraController(_virtualCamera));
        AddController(new EnemyController());
        AddController(new LevelController());
        
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
        if (controller is IContainService)
        {
            _services.Add((controller as IContainService).service);
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
