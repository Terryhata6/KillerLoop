using UnityEngine;


public class PlayerWallRunLeftModel : BasePlayerStateModel
{
    private PlayerWallRunState _state;
    private float _timer;
    private float _landingTime = 1.0f;
    private Vector3 _direction;
    private float _jumpSpeed = 3.0f;
    private float _wallRunSpeed = 5.0f;
    private float _wallRunMaxForce = 1.0f;
    private float _wallRunMinForce = -1.0f;
    private float _currentForce;

    private float _rayLenght = 0.3f;


    public override void Execute(PlayerController controller, PlayerView player)
    {
        base.Execute(controller, player);

        player.Rigidbody.isKinematic = true;

        switch (_state)
        {
            case PlayerWallRunState.None:
                _state = PlayerWallRunState.JumpOn;
                break;
            case PlayerWallRunState.JumpOn:
                JumpOn(player);
                break;
            case PlayerWallRunState.Run:
                Run(player);
                break;
            case PlayerWallRunState.JumpOff:
                JumpOff(player);
                break;
            case PlayerWallRunState.Landing:
                Landing(player);
                break;
        }
    }


    private void JumpOn(PlayerView player)
    {
        player.Animator.SetBool("Jump", true);

        player.transform.rotation = player.Trigger.transform.rotation;
        _direction = (player.transform.right * -1.0f) + player.transform.up + player.transform.forward;
        player.transform.Translate(_direction * _jumpSpeed * Time.deltaTime);

        if (Physics.Raycast(player.transform.position, Vector3.left, _rayLenght))
        {
            _state = PlayerWallRunState.Run;
            player.Animator.SetBool("Jump", false);
            _currentForce = _wallRunMaxForce;
        }
    }

    private void Run(PlayerView player)
    {
        player.Animator.SetBool("WRLeft", true);

        _direction = player.transform.forward + Vector3.up * _currentForce;
        _currentForce -= Time.deltaTime;
        player.transform.Translate(_direction * _wallRunSpeed * Time.deltaTime);
        if (_currentForce <= _wallRunMinForce)
        {
            player.Animator.SetBool("WRLeft", false);
            _state = PlayerWallRunState.JumpOff;
        }
    }

    private void JumpOff(PlayerView player)
    {
        player.Animator.SetBool("Jump", true);

        _direction = player.transform.right + player.transform.up * -1.0f + player.transform.forward;
        player.transform.Translate(_direction * _jumpSpeed * Time.deltaTime);
        if (Physics.Raycast(player.transform.position, Vector3.down, _rayLenght))
        {
            _state = PlayerWallRunState.Landing;
            player.Animator.SetBool("Jump", false);
            _timer = _landingTime;
        }
    }

    private void Landing(PlayerView player)
    {
        player.Animator.SetBool("Landing", true);
        FindLand(player);
        player.transform.Translate(player.transform.forward * player.MovementSpeed * Time.deltaTime);
        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            player.SetState(PlayerState.Idle);
        }
    }
}