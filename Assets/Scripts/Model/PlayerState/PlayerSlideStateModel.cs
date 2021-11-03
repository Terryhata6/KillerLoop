using UnityEngine;

public class PlayerSlideStateModel : BasePlayerStateModel
{
    private float _timer = 1.0f;
    private float _timeSlide = 1.0f;
    private float _slideSpeed = 4.0f;


    public override void Execute(PlayerController controller, PlayerView player)
    {
        base.Execute(controller, player);

        player.Animator.SetBool("Slide", true);
        player.transform.Translate(player.transform.forward * _slideSpeed * Time.deltaTime);

        _timer -= Time.deltaTime;
        if (_timer <= 0.0f)
        {
            _timer = _timeSlide;
            player.Animator.SetBool("Slide", false);
            player.SetState(PlayerState.Idle);
        }
    }
}