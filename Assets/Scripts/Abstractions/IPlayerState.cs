using UnityEngine;

namespace Abstractions
{
    public interface IPlayerState
    {
        public void Execute(Vector2 positionBegan, Vector2 positionDelta, PlayerView player);
    }
}