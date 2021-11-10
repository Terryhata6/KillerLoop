using UnityEngine;

public class PlayerJumpOverStateModel : BasePlayerStateModel
{
    private float _jumpTime = 1.8f;
    private float _timer = 1.8f;
    private float _jumpSpeed = 4.0f;


    public override void Execute(PlayerController controller, PlayerView player)
    {
        base.Execute(controller, player);

        player.Animator.SetBool("JumpOver", true);

        player.transform.rotation = player.Trigger.transform.rotation;
        player.transform.Translate(player.transform.forward * _jumpSpeed * Time.deltaTime);
        
        _timer -= Time.deltaTime;
        
        if (_timer <= 0)
        {
            _timer = _jumpTime;
            player.Animator.SetBool("JumpOver", false);
            player.SetState(PlayerState.Move);
        }
    }
}