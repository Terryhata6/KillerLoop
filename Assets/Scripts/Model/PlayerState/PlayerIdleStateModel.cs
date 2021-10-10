using UnityEngine;

public class PlayerIdleStateModel : BasePlayerStateModel
{
    public override void Execute(PlayerController controller, PlayerView player)
    {
        base.Execute(controller, player);
        player.Animator.SetFloat("MovingBlend", 0);
    }    
}