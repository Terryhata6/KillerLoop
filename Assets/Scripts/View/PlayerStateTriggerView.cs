using UnityEngine;

public class PlayerStateTriggerView : BaseObjectView
{
    [SerializeField] private PlayerState _playerState;

    public PlayerState PlayerState => _playerState;
    public Transform Transform => transform;


    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerView player))
        {
            player.SetState(_playerState);
            player.SetTrigger(this);
        }
    }
}