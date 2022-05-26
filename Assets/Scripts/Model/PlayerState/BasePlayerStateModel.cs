using UnityEngine;
using Abstractions;

public abstract class BasePlayerStateModel : IPlayerState
{
    private float _gravity = 5f;
    public virtual void Execute(PlayerController controller, PlayerView player)
    {
        
    }
    
    
    
    protected void Gravity(PlayerView player)
    {
        if (player.RayCastCheck(player.Position + Vector3.up, Vector3.down, 1.2f, 1<<11))
        {
            player.Move(Vector3.down * (player.Hit.distance - 1f) * Time.deltaTime, _gravity);
        }
        else
        {
            player.Move(Vector3.down * Time.deltaTime, _gravity);
        }
    }
}