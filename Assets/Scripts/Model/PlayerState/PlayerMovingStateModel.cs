using UnityEngine;

public class PlayerMovingStateModel : BasePlayerStateModel
{
    #region PrivateFields

    private Vector3 _movingVector = Vector3.zero;
    private Vector2 _movingVector2D;
    private float _magnitude;
    private Vector3 _tempVector;

    #endregion

    #region PublicMethods

    public override void Execute(Vector2 positionBegan, Vector2 positionDelta, PlayerView player)
    {
        base.Execute(positionBegan, positionDelta, player);
        if (IdlePosition(positionBegan, positionDelta))
        {
            player.Stand();
            return;
        }

        _movingVector2D = positionDelta - positionBegan;
        CalculateMovingVector3d(_movingVector2D, out _movingVector, out _magnitude);
        player.Rotate(Quaternion.LookRotation(_movingVector, Vector3.up));
        player.Move(Vector3.forward * _magnitude * Time.deltaTime);
        player.SetMovingBlend(_magnitude);
        GravityEffect(player);

        CheckToJump(player);
        CheckToKill(player);
    }

    #endregion

    #region PrivateMethods

    private void CalculateMovingVector3d(Vector2 vector2d, out Vector3 movingVector3d, out float magnitude)
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

    private void CheckToJump(PlayerView player)
    {
        if (!player.RayCastCheck(player.Position + Vector3.up * 1.5f, player.Forward * 2f + Vector3.down * 4f, 3f, (1 << 11) | (1 << 12))
        || player.RayCastCheck(player.Position + Vector3.up, player.Forward * 1f, 2f, 1 << 11))
        {
            Debug.Log("jump");
            player.Jump();
        }
    }
    private void CheckToKill(PlayerView player)
    {
        _tempVector.x = player.Forward.z;
        _tempVector.z = -player.Forward.x;
        _tempVector.y = 0f;
        if (player.RayCastCheck(player.Position + player.Forward * 0.6f + (_tempVector + Vector3.up) * 0.5f, -_tempVector, 1f, 1 << 13))
        {
            Debug.Log("kill");
            player.GroundKill();
        }
    }

    #endregion
}