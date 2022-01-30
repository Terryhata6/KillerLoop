
using UnityEngine;

public class PlayerRunWallModel : BasePlayerStateModel
{
    private Vector3 _movingVector = Vector3.zero;
    private Vector2 _movingVector2D;
    private float _magnitude;
    private Vector3 _wallVector;
    private float _wallSide;
    private Vector3 _tempVector;
    private float _directionTreshold = 0.8f;
    private float _timer;
    private float _timeJump = 0.5f;
    private Vector3 _hitNormal;


    public override void Execute(PlayerController controller, PlayerView player)
    {
        base.Execute(controller, player);

        if (!player.WallRunning)
        {
            WallRunSetUp(player);
        }
        if (controller.PositionBegan.Equals(Vector2.zero))
        {
            return;
        }
        if (player.WallRunning)
        {
            _movingVector2D = controller.PositionDelta - controller.PositionBegan;
            if (ReadyToJump(_movingVector2D, player))
            {
                return;
            }
            CalculateMovingVector3d(player,_movingVector2D, out _movingVector, out _magnitude);
            player.Move(_movingVector * _magnitude * player.MovementSpeed * Time.deltaTime);
            player.Rotate(Quaternion.LookRotation(_movingVector));
            
            CheckToJump(player);
        }
    }

    private void CalculateMovingVector3d(PlayerView player,Vector2 vector2d,out Vector3 movingVector3d, out float magnitude)
    {
        magnitude = vector2d.magnitude;

        if (magnitude > 100)
        {
            magnitude = 100.0f;
        }

        _magnitude *= 0.01f;
        
        if (Vector3.Dot(vector2d, Vector2.up) >= _directionTreshold)
        {
            movingVector3d = -_wallVector * _wallSide;
            player.SetMovingBlend(_wallSide * _magnitude);
        }
        else if (Vector3.Dot(vector2d, Vector2.down) >= _directionTreshold)
        {
            movingVector3d = _wallVector * _wallSide;
            player.SetMovingBlend(-_wallSide * _magnitude);
        }
        else
        {
            movingVector3d = Vector3.zero;
        }
    }
    
    private void WallRunSetUp(PlayerView player)
    {
        _wallSide = player.WallSide;
        _wallVector = CalculateWallVector(player.Hit);
        player.Position = MovePlayerToWall( player.Hit.point, player.Position,_hitNormal);
        player.WallRunning = true;
        _timer = _timeJump;
    }

    private Vector3 CalculateWallVector(RaycastHit hit)
    {
        if (_wallSide.Equals(0f))
        {
            return Vector3.up;
        }

        _hitNormal = hit.normal;
        Debug.Log(Vector3.Cross(_hitNormal, Vector3.up));
        return Vector3.Cross(_hitNormal, Vector3.up);
    }

    private Vector3 MovePlayerToWall(Vector3 hitPoint, Vector3 playerPos, Vector3 hitNormal)
    {
        _tempVector = Vector3.Project(playerPos, hitPoint) + hitNormal * 0.3f;
        _tempVector.y = playerPos.y;
        Debug.Log(hitPoint);
        return _tempVector;
    }

    private bool ReadyToJump(Vector2 vector, PlayerView player)
    {
        if (Vector3.Dot(vector, Vector2.left * _wallSide) >= _directionTreshold * 100f)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                player.StopWallRun();
                player.Jump();
            }
            return true;
        }
        else if (_timer != _timeJump)
        {
            _timer = _timeJump;
        }
        return false;
    }

    private void CheckToJump(PlayerView player)
    {
        _tempVector.x = player.Forward.z;
        _tempVector.z = -player.Forward.x;
        _tempVector.y = 0f;
        if (!player.RayCastCheck(player.Position, -_hitNormal, 2f, 1 << 11))
        {
            player.StopWallRun();
            player.Jump();
            Debug.Log("jump");
        }
        if (player.RayCastCheck(player.Position, player.Forward * 1.5f, 2f , 1 << 11))
        {
            player.StopWallRun();
            player.Jump();
            Debug.Log("jump");
        }
    }
}