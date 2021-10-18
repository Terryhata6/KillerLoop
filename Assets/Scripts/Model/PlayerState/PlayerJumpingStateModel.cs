using UnityEngine;


public class PlayerJumpingStateModel : BasePlayerStateModel
{
    private float _rayLenth = 1.0f;
    private bool _isGround;

    public override void Execute(PlayerController controller, PlayerView player)
    {
        base.Execute(controller, player);


    }
}