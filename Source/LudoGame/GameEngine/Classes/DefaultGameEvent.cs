using System;
using LudoGame.GameEngine.Interfaces;

namespace LudoGame.GameEngine.Classes
{
    public class DefaultGameEvent : IGameEvent
    {

        //Invoked in GamePlay
        private event Action OnRoundCompletedEvent;
        private event Action<IGamePlayer> OnInvalidResponseEvent;

        //Invoked in GameAction
        private event Action<Pawn[]> OnMoveEvent;
        private event Action<GameEnum.TeamColor> OnTakeOutTwoEvent;
        private event Action OnNewGameEvent;
        private event Action OnRestartedGameEvent;

        private event Action<GameEnum.TeamColor> OnBounceEvent;
        private event Action<GameEnum.TeamColor, int> OnGoalEvent;
        private event Action<GameEnum.TeamColor> OnAllTeamPawnsOutEvent;
        private event Action<GameEnum.TeamColor, GameEnum.TeamColor, int> OnEradicationEvent;
        private event Action<GameEnum.TeamColor> GameLoserEvent;
        private event Action OnGameOverEvent;
        private event Action<GameEnum.TeamColor> OnSafeZoneEvent;

        public void InvokeOnMoveEvent(Pawn[] pawns) => OnMoveEvent?.Invoke(pawns);
        public void InvokeOnRoundCompletedEvent() => OnRoundCompletedEvent?.Invoke();
        public void InvokeOnNewGameEvent() => OnNewGameEvent?.Invoke();
        public void InvokeOnInvalidResponseEvent(IGamePlayer player) => OnInvalidResponseEvent?.Invoke(player);

        public void InvokeOnAllTeamPawnsOutEvent(GameEnum.TeamColor pawnColor) => OnAllTeamPawnsOutEvent?.Invoke(pawnColor);
        public void InvokeOnGoalEvent(GameEnum.TeamColor color, int pawnsLeft) => OnGoalEvent?.Invoke(color, pawnsLeft);
        public void InvokeOnGameOverEvent() => OnGameOverEvent?.Invoke();
        public void InvokeOnEradicationEvent(GameEnum.TeamColor pawnColor, GameEnum.TeamColor enemyColor, int deadEnemies) => OnEradicationEvent?.Invoke(pawnColor, enemyColor, deadEnemies);
        public void InvokeOnBounceEvent(GameEnum.TeamColor color) => OnBounceEvent?.Invoke(color);
        public void InvokeOnSafeZoneEvent(GameEnum.TeamColor color) => OnSafeZoneEvent?.Invoke(color);
    }
}