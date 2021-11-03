using UnityEngine;

public class CameraController : BaseController, IExecute
{
    private VirtualCameraView _playerCamera;
    private int _basePriority = 10;
    private int _highPriority = 99;

    public CameraController()
    {

    }


    public override void Initialize()
    {
        base.Initialize();

        //Camera instantiate
        _playerCamera = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Camera/PlayerCamera")).GetComponent<VirtualCameraView>();
        _playerCamera.SetTarget(_main.PlayerTransform, _main.PlayerTransform);
        _playerCamera.SetPriority(_highPriority);
    }

    public void Execute()
    {

    }
}