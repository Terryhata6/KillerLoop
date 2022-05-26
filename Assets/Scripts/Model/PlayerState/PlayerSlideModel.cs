using UnityEngine;

public class PlayerSlideModel : BasePlayerStateModel
{

    public override void Execute(PlayerController controller, PlayerView player)
    {
        base.Execute(controller, player);
        
        player.LookRotation(player.SplineDirection);
        player.Move(Vector3.forward * Time.deltaTime);
    }
}