
using UnityEngine;

public class KillTrigger : BaseObjectView
{
    [SerializeField] private PlayerView _player;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 13)
        {
            other.gameObject.TryGetComponent(out EnemyView enemy);
            _player.KillEnemy(enemy);
        }
    }
}