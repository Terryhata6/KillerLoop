
using UnityEngine;

public class DeathZoneView : BaseObjectView
{
    #region PrivateFields

    private PlayerView _player;
    private EnemyView _enemy;

    #endregion

    #region OnTriggerEnter

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out _player))
        {
            _player.Dead();
        }
        if (other.gameObject.TryGetComponent(out _enemy))
        {
            _enemy.DeadWithRagdoll();
        }
    }

    #endregion
}