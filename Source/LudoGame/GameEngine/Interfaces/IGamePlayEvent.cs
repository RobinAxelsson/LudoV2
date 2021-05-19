using System;

namespace LudoGame.GameEngine.Interfaces
{
    public interface IGamePlayEvent
    {
        public event Action OnRoundCompletedEvent;
        public event Action<IGamePlayer> OnInvalidResponseEvent;
    }
}