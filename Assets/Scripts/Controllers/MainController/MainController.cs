using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{
    [SerializeField] private Transform _playerStarterPoint;
    [SerializeField] private PlayerView _playerPrefab;
    [SerializeField] private PlayerView _playerView;
    [SerializeField] private bool _useMouse = true;
    [SerializeField] private bool _debugTestingScene = false;
    [SerializeField] private string _testingSceneName = "";

    private List<BaseController> _controllers = new List<BaseController>();

    public bool UseMouse => _useMouse;
    private void Awake()
    {
        ///----------Services-------------
        DontDestroyOnLoad(this.gameObject);
        //SceneManager.UnloadSceneAsync(_starterSceneName);
        
        

        ///------SceneSettings------
        if (_playerStarterPoint == null)
        {
            //_playerStarterPoint = FindObjectOfType<PlayerStarterPosition>().transform;
            Debug.Log($"Не повезло ");
        }

        _playerView = Instantiate(_playerPrefab, _playerStarterPoint.position, Quaternion.identity);
        //_playerView = _playerPrefab;
        //_playerController.SetPlayer(_playerView);

    }

    private void Start()
    {
        for (int i = 0; i < _controllers.Count; i++)
        {
            if (_controllers[i] is IInitialize)
            {
                _controllers[i].Initialize();
            }
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
        GetController<BaseController>();
        InputController a = new InputController(this);
        a = GetController<InputController>();
    }
    #endregion
}
