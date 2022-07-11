using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{
    #region PrivateFields

    #region Serialized

    [Header("Player")]
    [SerializeField] private Transform _playerStarterPoint;
    [SerializeField] private PlayerView _playerView;
    [SerializeField] private VirtualCameraView _virtualCamera;

    [Header("Enemy")]
    [SerializeField] private List<EnemyView> _tempEnemies; //Не забудь убрать это и доделать спавн врагов - EnterAlt

    [Header("Settings")]
    [SerializeField] private bool _debugTestingScene = false;
    [SerializeField] private string _testingSceneName = "";

    #endregion

    private List<BaseController> _controllers;
    private ServiceDistributor _serviceDistributor;

    #endregion

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _serviceDistributor = new ServiceDistributor();
        InitializeFields();
        CreateControllers();
        
        if (_debugTestingScene)
        {
            SceneManager.LoadSceneAsync(_testingSceneName, LoadSceneMode.Additive);
        }

        GetController<CameraController>().SetCameraTarget(_playerView.transform);
        GetController<EnemyController>().SetEnemies(_tempEnemies);
        
    }

    private void Start()
    {
        InitializeControllers();
        SetServicesToDistributor();
        SetConsumersToDistributor();
        _serviceDistributor.Distribute();

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

    #region ServiceMethods

    #region ServiceDistributor

    private void SetServicesToDistributor()
    {
        for (int i = 0; i < _controllers.Count; i++)
        {
            if (_controllers is IContainServices)
            {
                _serviceDistributor.AddServices((_controllers as IContainServices).GetServices());
            }
        }
    }
    private void SetConsumersToDistributor()
    {
        for (int i = 0; i < _controllers.Count; i++)
        {
            if (_controllers is IContainConsumers)
            {
                _serviceDistributor.AddConsumers((_controllers as IContainConsumers).GetConsumers());
            }
        }
    }

    #endregion

    #region Initialize

    private void InitializeFields()
    {
        _controllers = new List<BaseController>();
    }
    private void CreateControllers()
    {
        AddController(new InputController());
        AddController(new PlayerController(_playerView));
        AddController(new CameraController(_virtualCamera));
        AddController(new EnemyController(_tempEnemies));
        //AddController(new LevelController());
    }
    private void InitializeControllers()
    {
        for (int i = 0; i < _controllers.Count; i++)
        {
            _controllers[i].Initialize();
        }
    }

    #endregion

    #region ControllersManagement

    /// <summary>
    /// Add controller in Controller's list
    /// </summary>
    /// <param name="controller"></param>
    private void AddController(BaseController controller)
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
    private void RemoveController(BaseController controller)
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
    private T GetController<T>() where T : BaseController
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

    #endregion

    #endregion

}
