using UnityEngine;


public class PlayerFallBackStateModel : BasePlayerStateModel
{
    private Vector3 _direction;
    private float _fallPower = 1.0f;
    private float _currentPower = 1.0f;
    private float _speed = 3.0f;
    private float _rayLenght = 0.5f;


    public override void Execute(PlayerController controller, PlayerView player)
    {
        base.Execute(controller, player);

        player.Animator.SetBool("Jump", true);

        _direction = player.transform.forward * -1.0f + Vector3.up * _currentPower;
        player.transform.Translate(_direction * _speed * Time.deltaTime);

        _currentPower -= Time.deltaTime;


        if (_currentPower <= 0.0f)
        {
            if (Physics.Raycast(player.transform.position, Vector3.down, _rayLenght))
            {
                _currentPower = _fallPower;
                player.Animator.SetBool("Jump", false);
                player.SetState(PlayerState.Move);
            }
        }
    }
}