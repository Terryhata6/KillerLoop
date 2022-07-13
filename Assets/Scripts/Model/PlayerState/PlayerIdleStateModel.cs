using UnityEngine;

public class PlayerIdleStateModel : BasePlayerStateModel
{
    #region PublicMethods

    public override void Execute(Vector2 positionBegan, Vector2 positionDelta, PlayerView player)
    {
        base.Execute(positionBegan, positionDelta, player);
        if (PositionChanged(positionBegan))
        {
            player.Run();
        }
        GravityEffect(player);
        player.SetMovingBlend(0f);
    }

    #endregion
}