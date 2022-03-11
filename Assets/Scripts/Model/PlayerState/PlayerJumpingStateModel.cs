
using UnityEngine;

public class PlayerJumpingStateModel : BasePlayerStateModel
{
    private Vector3 _tempVector;
    private Vector2 _movingVector2D;
    private Vector3 _movingVector;
    private float _magnitude;

    public override void Execute(PlayerController controller, PlayerView player)
    {
        base.Execute(controller, player);
        player.Jumping();
        if (controller.PositionBegan != Vector2.zero)
        {
            _movingVector2D = controller.PositionDelta - controller.PositionBegan;
        }
        CalculateMovingVector3d(_movingVector2D, out _movingVector, out _magnitude);
        player.Rotate(Quaternion.LookRotation(_movingVector, Vector3.up));
        player.Move(Vector3.forward * _magnitude * Time.deltaTime);
            
        CheckToLand(player);
        CheckForAWall(player);
        CheckToKill(player);
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
        if (player.RayCastCheck(player.Position + Vector3.up,  Vector3.down, 1.1f, 1 << 11|(1 << 12)))
        {
            Debug.Log("land");
            player.Land();
            player.Stand();
        }
    }
    
    private void CheckToKill(PlayerView player)
    {
        _tempVector.x = _movingVector.normalized.z;
        _tempVector.z = -_movingVector.normalized.x;
        _tempVector.y = 0f;
        if (player.RayCastCheck(player.Position + _movingVector.normalized * 0.5f - _tempVector,  _tempVector, 2f, 1 << 13)
        || player.RayCastCheck(player.Position,  _movingVector + Vector3.down, 1.1f, 1 << 13))
        {
            Debug.Log("kill");
            player.Land();
            player.AirKill();
        }
    }
    
    private void CheckForAWall(PlayerView player)
    {
        _tempVector.x = player.Forward.z;
        _tempVector.z = -player.Forward.x;
        _tempVector.y = 0f;
        if (player.RayCastCheck(player.Position + (_tempVector + Vector3.up) * 0.5f, player.Forward.normalized + Vector3.up, 1f, 1 << 11)
        || player.RayCastCheck(player.Position - (_tempVector - Vector3.up) * 0.5f, player.Forward.normalized  + Vector3.up, 1f, 1 << 11))
        {
            player.Land();
            player.MovePlayerToWall(player.Hit.point, player.Hit.normal);
            player.WallRun();
        }
    }

    
}