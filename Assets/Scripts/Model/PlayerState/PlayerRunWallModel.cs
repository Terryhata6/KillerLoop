using UnityEngine;

public class PlayerRunWallModel : BasePlayerStateModel
{
    #region PrivateFields

    private Vector3 _movingVector = Vector3.zero;
    private Vector2 _movingVector2D;
    private float _magnitude;
    private Vector3 _tempVector;
    private Vector3 _tempVector2d;
    private float _directionTreshold = 0.8f;
    private float _jumpTreshold = 200f;
    private float _timer;
    private float _jumpCounterSpeed = 2f;

    #endregion

    #region PublicMethods

    public override void Execute(Vector2 positionBegan, Vector2 positionDelta, PlayerView player)
    {
        base.Execute(positionBegan, positionDelta, player);

        if (IdlePosition(positionBegan, positionDelta))
        {
            IdleOnTheWall(player);
            NotReadyToJump(player);
            return;
        }
        _movingVector2D = positionDelta - positionBegan;
        if (ReadyToJump(_movingVector2D, player))
        {
            return;
        }
        CalculateMovingVector3d(_movingVector2D, player.HitNormal, out _movingVector, out _magnitude);
        player.Rotate(Quaternion.LookRotation(_movingVector));
        player.Move(Vector3.forward * Time.deltaTime);
        player.SetMovingBlend(CalculateMovingBlend(_movingVector, player.HitNormal));

        CheckToJump(player);
    }

    #endregion

    #region PrivateMethods

    private void CalculateMovingVector3d(Vector2 vector2d, Vector3 normal, out Vector3 movingVector3d, out float magnitude)
    {
        magnitude = vector2d.magnitude;
        if (magnitude > 100)
        {
            magnitude = 100.0f;
        }
        magnitude *= 0.01f;

        _tempVector.x = normal.z;
        _tempVector.z = -normal.x;
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

    private bool ReadyToJump(Vector2 vector, PlayerView player)
    {
        _tempVector2d.x = player.HitNormal.x;
        _tempVector2d.y = player.HitNormal.z;
        if (Vector3.Dot(vector, _tempVector2d) >= _jumpTreshold)
        {
            IdleOnTheWall(player);
            _timer += Time.deltaTime * _jumpCounterSpeed;
            player.IndicatorImage.transform.LookAt(player.MainCam.transform);
            player.IndicatorImage.fillAmount = _timer;
            if (_timer >= 1f)
            {
                player.Jump();
                player.IndicatorImage.fillAmount = 0f;
                _timer = 0f;
            }
            return true;
        }
        else if (_timer != 0f)
        {
            NotReadyToJump(player);
        }
        return false;
    }

    private void IdleOnTheWall(PlayerView player)
    {
        player.SetMovingBlend(0f);
        player.Rotate(Quaternion.LookRotation(-player.HitNormal));
    }

    private void NotReadyToJump(PlayerView player)
    {
        _timer = 0f;
        player.IndicatorImage.fillAmount = _timer;
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