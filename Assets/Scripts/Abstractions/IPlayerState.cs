using UnityEngine;

namespace Abstractions
{
    public interface IPlayerState
    {
        public void Execute(PlayerController controller, PlayerView player);
    }
}