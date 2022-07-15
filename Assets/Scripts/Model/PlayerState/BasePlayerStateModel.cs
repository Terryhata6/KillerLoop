using UnityEngine;
using Abstractions;

public abstract class BasePlayerStateModel : IPlayerState
{
    #region PrivateFields
    private float _gravity = 5f;
    #endregion

    #region PublicMethods
    public virtual void Execute(Vector2 positionBegan, Vector2 positionDelta, PlayerView player)
    {

    }

    #endregion

    #region ProtectedMethods
    protected void GravityEffect(PlayerView player)
    {
        if (player.RayCastCheck(player.Position + Vector3.up, Vector3.down, 1.2f, 1 << 11))
        {
            player.MoveWithSpeed(Vector3.down * (player.Hit.distance - 1f) * Time.deltaTime, _gravity);
        }
        else
        {
            player.MoveWithSpeed(Vector3.down * Time.deltaTime, _gravity);
        }
    }

    protected bool IdlePosition(Vector2 positionBegan, Vector2 positionDelta)
    {
        return (positionBegan - positionDelta) == Vector2.zero;
    }

    protected bool PositionChanged(Vector2 positionBegan)
    {
        return positionBegan.magnitude > 0.0f;
    }
    #endregion

}