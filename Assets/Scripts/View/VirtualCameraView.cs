using Cinemachine;
using UnityEngine;


[RequireComponent(typeof(CinemachineVirtualCamera))]
public class VirtualCameraView : BaseObjectView
{
    private CinemachineVirtualCamera _camera;


    private void Awake()
    {
        _camera = GetComponent<CinemachineVirtualCamera>();
    }


    public void SetPriority(int priority)
    {
        _camera.Priority = priority;
    }

    public void SetTarget(Transform follow, Transform lookAt)
    {
        _camera.Follow = follow;
        _camera.LookAt = lookAt;
    }
}