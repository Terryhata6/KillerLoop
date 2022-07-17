using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{
    #region PrivateFields

    #region Serialized

    [Header("Player")]
    [SerializeField] private PlayerView _playerPrefab;
    [SerializeField] private VirtualCameraView _virtualCamera;

    [Header("Levels")]
    [SerializeField] private List<LevelView> _levels;

    [Header("Settings")]
    [SerializeField] private bool _debugTestingScene = false;
    [SerializeField] private string _testingSceneName = "";

    #endregion

    private List<BaseController> _controllers;
    private List<IExecute> _executeControllers;
    private ServiceDistributor _serviceDistributor;

    #endregion

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _serviceDistributor = ServiceDistributor.Instance;
        InitializeFields();
        CreateControllers();
        
        if (_debugTestingScene)
        {
            SceneManager.LoadSceneAsync(_testingSceneName, LoadSceneMode.Additive);
        }       
    }

    private void Start()
    {
        InitializeControllers();
        SetExecuteControllers();
        SetConsumersToDistributor();
        _serviceDistributor.Distribute();
        GameEvents.Current.GameStart();
    }

    private void Update()
    {        
        for (int i = 0; i < _executeControllers.Count; i++)
        {
            _executeControllers[i].Execute();
        }   
    }

    #region ServiceMethods

    #region ServiceDistributor

    private void SetConsumersToDistributor()
    {
        for (int i = 0; i < _controllers.Count; i++)
        {
            if (_controllers[i] is IConsumer)
            {
                _serviceDistributor.AddConsumer((_controllers[i] as IConsumer));
            }
        }
    }

    #endregion

    #region Initialize

    private void InitializeFields()
    {
        _controllers = new List<BaseController>();
        _executeControllers = new List<IExecute>();
    }

    private void CreateControllers()
    {
        AddController(new InputController());
        AddController(new PlayerController(_playerPrefab));
        AddController(new CameraController(_virtualCamera));
        AddController(new EnemyController());
        AddController(new LevelController(_levels));
    }

    private void InitializeControllers()
    {
        for (int i = 0; i < _controllers.Count; i++)
        {
            _controllers[i].Initialize();
        }
    }

    private void SetExecuteControllers()
    {
        for (int i = 0; i < _controllers.Count; i++)
        {
            if (_controllers[i] is IExecute)
            {
                SetExecuteController(_controllers[i] as IExecute);
            }
        }
    }

    private void SetExecuteController(IExecute controller)
    {
        if (!_executeControllers.Contains(controller))
        {
            _executeControllers.Add(controller);
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
