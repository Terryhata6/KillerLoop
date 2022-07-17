using UnityEngine;

public class CameraController : BaseController,
    IServiceConsumer<IPlayerSpawner> //Consumer
{
    #region PrivateFields

    private VirtualCameraView _camera;
    private int _basePriority = 10;
    private int _highPriority = 99;

    #endregion

    public CameraController(VirtualCameraView cam)
    {
        _camera = cam;
    }

    #region PublicMethods

    public void SetCameraTarget(Transform target)
    {
        if (_camera 
            && target)
        {
            _camera.SetTarget(target, target);
            _camera.SetPriority(_highPriority);
        }
    }

    #endregion

    #region IServiceConsumer

    public void UseService(IPlayerSpawner service)
    {
        if (service != null)
        {
            SetCameraTarget(service.CurrentPlayer.transform);
        }
    }

    #endregion
}