using UnityEngine;

public class PlayerJumpingStateModel : BasePlayerStateModel
{
    private Vector3 _tempVector;
    private float _baseY;
    private float _y;
    private float _x;
    private float _timer;
    private Vector2 _movingVector2D;
    private Vector3 _movingVector;
    private float _magnitude;

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
            
            player.Rotate(Quaternion.LookRotation(_movingVector, Vector3.up));
            player.Move(player.Forward * _magnitude * Time.deltaTime);
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
        magnitude *= 0.01f;
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
            FindLand(player);
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
            player.WallRun();
        }
        else if (player.RayCastCheck(player.Position - (_tempVector - Vector3.up) * 0.5f, player.Forward.normalized  + Vector3.up, 1f, 1 << 11))
        {
            player.Land();
            player.WallRun();
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