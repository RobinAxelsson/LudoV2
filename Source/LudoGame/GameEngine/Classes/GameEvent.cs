using System;
using LudoGame.GameEngine.Interfaces;

namespace LudoGame.GameEngine.Classes
{
    public class GameEvent : IGameEvent
    {
        //GamePlay
        public event Action OnRoundCompletedEvent;
        public event Action<IGamePlayer> OnInvalidResponseEvent;

        //GameAction
        public event Action<GameEnum.TeamColor, int> OnMoveEvent;
        public event Action<GameEnum.TeamColor> OnTakeOutTwoEvent;
        public event Action OnNewGameEvent;
        public event Action OnRestartedGameEvent;

        public event Action<GameEnum.TeamColor> OnBounceEvent;
        public event Action<GameEnum.TeamColor, int> OnGoalEvent;
        public event Action<GameEnum.TeamColor> OnAllTeamPawnsOutEvent;
        public event Action<GameEnum.TeamColor, GameEnum.TeamColor, int> OnEradicationEvent;
        public event Action<GameEnum.TeamColor> GameLoserEvent;
        public event Action OnGameOverEvent;
        public event Action<GameEnum.TeamColor> OnSafeZoneEvent;

        //Invoke-methods
        public void InvokeOnRoundCompletedEvent() => OnRoundCompletedEvent?.Invoke();
        public void InvokeOnInvalidResponseEvent(IGamePlayer player) => OnInvalidResponseEvent?.Invoke(player);

        public void InvokeOnAllTeamPawnsOutEvent(GameEnum.TeamColor pawnColor) => OnAllTeamPawnsOutEvent?.Invoke(pawnColor);
        public void InvokeOnGoalEvent(GameEnum.TeamColor color, int pawnsLeft) => OnGoalEvent?.Invoke(color, pawnsLeft);
        public void InvokeOnGameOverEvent() => OnGameOverEvent?.Invoke();
        public void InvokeOnEradicationEvent(GameEnum.TeamColor pawnColor, GameEnum.TeamColor enemyColor, int deadEnemies) => OnEradicationEvent?.Invoke(pawnColor, enemyColor, deadEnemies);
        public void InvokeOnBounceEvent(GameEnum.TeamColor color) => OnBounceEvent?.Invoke(color);
        public void InvokeOnSafeZoneEvent(GameEnum.TeamColor color) => OnSafeZoneEvent?.Invoke(color);
    }
}