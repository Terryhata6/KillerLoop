
using UnityEngine;

public class PlayerRunWallModel : BasePlayerStateModel
{
    private Vector3 _movingVector = Vector3.zero;
    private Vector2 _movingVector2D;
    private float _magnitude;
    private Vector3 _tempVector;
    private Vector3 _tempVector2d;
    private float _directionTreshold = 0.8f;
    private float _jumpTreshold = 150f;
    private float _timer;
    private float _jumpCounterSpeed = 2f;
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
            IdleOnTheWall(player);
            return;
        }
        if (player.WallRunning)
        {
            _movingVector2D = controller.PositionDelta - controller.PositionBegan;
            if (ReadyToJump(_movingVector2D, player))
            {
                return;
            }
            CalculateMovingVector3d(_movingVector2D, out _movingVector, out _magnitude);
            player.Move(_movingVector * Time.deltaTime);
            player.SetMovingBlend(CalculateMovingBlend(player,_hitNormal));
            player.Rotate(Quaternion.LookRotation(_movingVector));
            
            CheckToJump(player);
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

        _tempVector.x = _hitNormal.z;
        _tempVector.z = -_hitNormal.x;
        _tempVector.y = 0f;
        _tempVector2d.x = _tempVector.x;
        _tempVector2d.y = _tempVector.z;
        if (Vector3.Dot(vector2d, -_tempVector2d.normalized) >= _directionTreshold)
        {
            movingVector3d = -_tempVector * magnitude;
        }
        else if (Vector3.Dot(vector2d, _tempVector2d.normalized) >= _directionTreshold)
        {
            movingVector3d = _tempVector * magnitude;
        }
        else
        {
            movingVector3d = Vector3.zero;
        }
    }

    private float CalculateMovingBlend(PlayerView player, Vector3 normal)
    {
        _tempVector.z = -player.Forward.x;
        _tempVector.x = player.Forward.z;
        _tempVector.y = 0f;
        if (Vector3.Dot( player.Position.normalized - normal,-_tempVector) >= _directionTreshold)
        {
            return -1f;
        }
        if (Vector3.Dot(player.Position.normalized - normal,_tempVector) >= _directionTreshold)
        {
            return 1f;
        }

        return 0f;
    }
    
    private void WallRunSetUp(PlayerView player)
    {
        _hitNormal = player.Hit.normal;
        player.Position = MovePlayerToWall( player.Hit.point, player.Position,_hitNormal);
        player.WallRunning = true;
        _timer = 0f;
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
        if (Vector3.Dot(vector, _hitNormal.normalized) >= _jumpTreshold)
        {
            IdleOnTheWall(player);
            _timer += Time.deltaTime * _jumpCounterSpeed;
            player.IndicatorImage.transform.LookAt(player.MainCam.transform);
            player.IndicatorImage.fillAmount = _timer;
            if (_timer >= 1f)
            {
                player.StopWallRun();
                player.Jump();
                player.IndicatorImage.fillAmount = 0f;
            }
            return true;
        }
        else if (_timer != 0f)
        {
            _timer = 0f;
            player.IndicatorImage.fillAmount = _timer;
        }
        return false;
    }

    private void IdleOnTheWall(PlayerView player)
    {
        player.SetMovingBlend(0f);
        player.Rotate(Quaternion.LookRotation(-_hitNormal));
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
        if (player.RayCastCheck(player.Position, _movingVector * 1.5f, 2f , 1 << 11))
        {
            player.StopWallRun();
            player.Jump();
            Debug.Log("jump");
        }
    }
}