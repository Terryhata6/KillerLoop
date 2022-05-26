
public class PlayerIdleStateModel : BasePlayerStateModel
{
    public override void Execute(PlayerController controller, PlayerView player)
    {
        base.Execute(controller, player);
        if (controller.PositionBegan.magnitude > 0f)
        {
            player.Run();
        }
        Gravity(player);
        player.SetMovingBlend(0f);
    }    
}