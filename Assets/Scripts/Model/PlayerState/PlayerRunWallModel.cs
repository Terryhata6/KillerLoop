using UnityEngine;

public class PlayerRunWallModel : BasePlayerStateModel
{
    #region PrivateFields

    private Vector3 _movingVector = Vector3.zero;
    private Vector2 _movingVector2D;
    private Vector3 _tempVector;
    private Vector3 _tempVector2d;
    private float _directionTreshold = 0.8f;
    private float _jumpTreshold = 200f;

    #endregion

    #region PublicMethods

    public override void Execute(Vector2 positionBegan, Vector2 positionDelta, PlayerView player)
    {
        base.Execute(positionBegan, positionDelta, player);

        if (IdlePosition(positionBegan, positionDelta))
        {
            IdleOnTheWall(player);
            return;
        }

        _movingVector2D = positionDelta - positionBegan;
        _movingVector = CalculateMovingVector3d(_movingVector2D, player.HitNormal);

        player.Rotate(Quaternion.LookRotation(_movingVector));
        player.Move(Vector3.forward * Time.deltaTime);
        player.SetMovingBlend(CalculateMovingBlend(_movingVector, player.HitNormal));

        CheckForInstanceJump(_movingVector2D, player);
        CheckToJump(player);
    }

    #endregion

    #region PrivateMethods

    private Vector3 CalculateMovingVector3d(Vector2 vector2d, Vector3 normal)
    {
        _tempVector.x = normal.z;
        _tempVector.z = -normal.x;
        _tempVector.y = 0f;
        _tempVector2d.x = _tempVector.x;
        _tempVector2d.y = _tempVector.z;
        if (Vector3.Dot(vector2d, -_tempVector2d.normalized) >= _directionTreshold)
        {
            return -_tempVector;
        }
        else if (Vector3.Dot(vector2d, _tempVector2d.normalized) >= _directionTreshold)
        {
           return _tempVector;
        }
        else
        {
            return Vector3.zero;
        }
    }

    private float CalculateMovingBlend(Vector3 movingVector, Vector3 normal)
    {
        _tempVector.z = -movingVector.x;
        _tempVector.x = movingVector.z;
        _tempVector.y = 0f;
        if (Vector3.Dot(normal, _tempVector.normalized) >= _directionTreshold)
        {
            return -1f;
        }
        if (Vector3.Dot(normal, -_tempVector.normalized) >= _directionTreshold)
        {
            return 1f;
        }

        return 0f;
    }

    private void CheckForInstanceJump(Vector2 vector, PlayerView player)
    {
        _tempVector2d.x = player.HitNormal.x;
        _tempVector2d.y = player.HitNormal.z;
        if (Vector3.Dot(vector, _tempVector2d) >= _jumpTreshold)
        {
            player.Jump();
        }
    }

    private void IdleOnTheWall(PlayerView player)
    {
        player.SetMovingBlend(0f);
        player.Rotate(Quaternion.LookRotation(-player.HitNormal));
    }

    private void CheckToJump(PlayerView player)
    {
        _tempVector.x = player.Forward.z;
        _tempVector.z = -player.Forward.x;
        _tempVector.y = 0f;
        if (!player.RayCastCheck(player.Position, -player.HitNormal, 2f, 1 << 11)
        || player.RayCastCheck(player.Position, _movingVector * 1.5f, 2f, 1 << 11))
        {
            player.Jump();
            Debug.Log("jump");
        }
    }

    #endregion

}