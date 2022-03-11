using UnityEngine;

public class CameraController : BaseController
{
    private VirtualCameraView _camera;
    private int _basePriority = 10;
    private int _highPriority = 99;

    public CameraController(VirtualCameraView cam)
    {
        _camera = cam;
    }

    public void SetCameraTarget(Transform target)
    {
        if (_camera)
        {
            _camera.SetTarget(target, target);
            _camera.SetPriority(_highPriority); 
        }
    }
}