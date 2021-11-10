using UnityEngine;

public class PlayerSlideStateModel : BasePlayerStateModel
{
    private float _timer = 1.2f;
    private float _timeSlide = 1.2f;
    private float _slideSpeed = 4.5f;
    private float _slide = 4.5f;


    public override void Execute(PlayerController controller, PlayerView player)
    {
        base.Execute(controller, player);

        player.Animator.SetBool("Slide", true);

        player.transform.rotation = player.Trigger.transform.rotation;
        player.transform.Translate(player.transform.forward * _slide * Time.deltaTime);

        _timer -= Time.deltaTime;
        _slide -= Time.deltaTime;
        if (_timer <= 0.0f)
        {
            _timer = _timeSlide;
            _slide = _slideSpeed;
            player.Animator.SetBool("Slide", false);
            player.SetState(PlayerState.Move);
        }
    }
}