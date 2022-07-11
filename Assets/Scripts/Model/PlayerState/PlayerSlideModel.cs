using UnityEngine;

public class PlayerSlideModel : BasePlayerStateModel
{
    #region PublicMethods

    public override void Execute(Vector2 positionBegan, Vector2 positionDelta, PlayerView player)
    {
        base.Execute(positionBegan, positionDelta, player);

        player.LookRotation(player.SplineDirection);
        player.Move(Vector3.forward * Time.deltaTime);
    }

    #endregion
}