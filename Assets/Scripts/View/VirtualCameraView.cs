using Cinemachine;
using Dreamteck.Splines;
using UnityEngine;


[RequireComponent(typeof(CinemachineVirtualCamera))]
public class VirtualCameraView : BaseObjectView
{
    [SerializeField] private CinemachineVirtualCamera _camera;

    private void CameraInit()
    {
        _camera = gameObject.GetComponent<CinemachineVirtualCamera>();
    }

    public void SetPriority(int priority)
    {
        _camera.Priority = priority;
    }

    public void SetTarget(Transform follow, Transform lookAt)
    {
        if (!_camera)
        {
            CameraInit();
        }
        if (follow && lookAt)
        {
            Debug.Log(_camera);
            _camera.Follow = follow;
            _camera.LookAt = lookAt;
        }
    }
}