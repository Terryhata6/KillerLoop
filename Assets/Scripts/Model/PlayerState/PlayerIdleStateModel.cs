using UnityEngine;

public class PlayerIdleStateModel : BasePlayerStateModel
{
    public override void Execute(PlayerController controller, PlayerView player)
    {
        base.Execute(controller, player);
        //FindLand(player);
        if (controller.PositionBegan.magnitude > 0f)
        {
            player.SetState(PlayerState.Move);
        }
        
        player.Animator.SetFloat("MovingBlend", 0);
    }    
}