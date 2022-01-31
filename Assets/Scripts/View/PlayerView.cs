
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : BaseObjectView
{
    [SerializeField] private PlayerState _state = PlayerState.Idle;
    [SerializeField] private float _movingBlend = 0;
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private Image _indicatorImage;
    private bool _jumping;
    private RaycastHit _hit;
    private Vector3 _tempVector;
    private float _iterator = 0f;
    private float _x = -1f;
    private bool _wallRunning;
    private Camera _mainCam;

    public Image IndicatorImage => _indicatorImage;
    public RaycastHit Hit => _hit;
    public PlayerState State => _state;
    public Animator Animator => _animator;
    public Rigidbody Rigidbody => _rigidbody;
    public Camera MainCam => _mainCam;

    public bool Jumping
    {
        get => _jumping;
        set => _jumping = value;
    }
    public bool WallRunning
    {
        get => _wallRunning;
        set => _wallRunning = value;
    }


    private void Awake()
    {
        if (_rigidbody == null)
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
        _mainCam = Camera.main;
        SetRagdoll(false);
    }


    // public void SetState(PlayerState state)
    // {
    //     _state = state;
    // }

    #region Actions

    public void Move(Vector3 dir)
    {
        transform.position += dir * _movementSpeed;
    }

    public void Rotate(Quaternion dir)
    {
        transform.rotation = dir;
    }

    public void Stand()
    {
        _animator.SetBool("Run", true);
        _state = PlayerState.Idle;
    }
    
    public void Land()
    {
        _rigidbody.useGravity = true;
        _wallRunning = false;
        _animator.SetBool("Jump", false);
    }

    public void WallRun()
    {
        _rigidbody.useGravity = false;
        _wallRunning = false;
        _animator.SetBool("WallRun",true);
        _state = PlayerState.WallRun;
    }

    public void StopWallRun()
    {
        _wallRunning = false;
        _animator.SetBool("WallRun",false);
    }
    
    public void StopRun()
    {
        _animator.SetBool("Run", false);
    }
    public void Run()
    {
        _state = PlayerState.Move;
    }

    public void Jump()
    {
        _rigidbody.useGravity = false;
        _jumping = false;
        _state = PlayerState.Jumping;
    }

    #endregion
    public bool RayCastCheck(Vector3 origin,Vector3 dir, float length, LayerMask layerToCheck)
    {
        Debug.DrawRay(origin , dir.normalized * length, Color.blue);
        if (Physics.Raycast(origin , dir, out _hit, length, layerToCheck))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetMovingBlend(float newValue)
    {
        _movingBlend = newValue;
        Animator.SetFloat("MovingBlend", _movingBlend);
    }

    public void SetRagdoll(bool value)
    {
        Rigidbody[] bodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody body in bodies)
        {
            body.isKinematic = !value;
            body.transform.GetComponent<Collider>().enabled = value;
        }
        GetComponent<CapsuleCollider>().enabled = !value; 
        _rigidbody.isKinematic = false;
        _animator.enabled = !value;
    }
    

    public void LevelFail()
    {
        CustomDebug.Log("Level Failed");
    }
}