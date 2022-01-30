using UnityEngine;


public class PlayerJumpingStateModel : BasePlayerStateModel
{
    private Vector3 _tempVector;
    private float _baseY;
    private float _y;
    private float _x;
    private float _timer;
    private float _landingTime = 1.0f;
    private Vector2 _movingVector2D;
    private Vector3 _movingVector;
    private float _magnitude;
    private float _vectorSpeedMagnitude;
    private Quaternion _rotationTemp;
    private Vector3 _translatePositionTemp;

    public override void Execute(PlayerController controller, PlayerView player)
    {
        base.Execute(controller, player);
        if (!player.Jumping)
        {
            player.Animator.SetBool("Jump", true);
            player.Jumping = true;
            _baseY = player.Position.y;
            _x = -1f;
            _y = 0f;
        }

        if (player.Jumping)
        {
            Jump(player);
            if (controller.PositionBegan != Vector2.zero)
            {
                _movingVector2D = controller.PositionDelta - controller.PositionBegan;
            }

            CalculateMovingVector3d(_movingVector2D, out _movingVector, out _magnitude);

            _vectorSpeedMagnitude = _magnitude * 0.01f;
        
            _rotationTemp = Quaternion.LookRotation(_movingVector, Vector3.up);
            _translatePositionTemp = player.Forward * _vectorSpeedMagnitude * player.MovementSpeed * Time.deltaTime;
        
            player.Rotate(_rotationTemp);
            player.Move(_translatePositionTemp);
            CheckToLand(player);
            CheckForAWall(player);
        }
    }

    private void CalculateMovingVector3d(Vector2 vector2d,out Vector3 movingVector3d, out float magnitude)
    {
        magnitude = vector2d.magnitude;

        if (magnitude > 100)
        {
            magnitude = 100.0f;
        }
        movingVector3d.x = vector2d.x;
        movingVector3d.z = vector2d.y;
        movingVector3d.y = 0;
        
    }
    
    private void CheckToLand(PlayerView player)
    {
        if (player.RayCastCheck(player.Position + Vector3.up,  Vector3.down, 1.1f, 1 << 11))
        {
            Debug.Log("land");
            player.Land();
            player.Stand();
        }
    }
    private void CheckForAWall(PlayerView player)
    {
        _tempVector.x = player.Forward.z;
        _tempVector.z = -player.Forward.x;
        _tempVector.y = 0f;
        if (player.RayCastCheck(player.Position + (_tempVector + Vector3.up) * 0.5f, player.Forward.normalized + Vector3.up, 1f, 1 << 11))
        {
            player.Land();
            player.WallRun(WallSide.Right);
            Debug.Log("wallRIGHT");
        }
        else if (player.RayCastCheck(player.Position - (_tempVector - Vector3.up) * 0.5f, player.Forward.normalized  + Vector3.up, 1f, 1 << 11))
        {
            Debug.Log("wallleft");
            player.Land();
            player.WallRun(WallSide.Left);
        }
    }

    private void Jump(PlayerView player)
    {
        
        _tempVector.y = (-1.8f * ((_x) * (_x)) + 2f) + _baseY;
        _tempVector.x = player.Position.x;
        _tempVector.z = player.Position.z;
        player.Position = _tempVector;
        _x += Time.deltaTime * 1.3f;
    }
}