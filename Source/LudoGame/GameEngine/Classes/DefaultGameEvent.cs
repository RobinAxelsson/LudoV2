using System;
using LudoGame.GameEngine.Interfaces;

namespace LudoGame.GameEngine.Classes
{
    public class DefaultGameEvent : IGameEvent
    {

        //Invoked in GamePlay
        public event Action<IGamePlayer, PlayerOption> OnNewPlayerOptionEvent;
        public event Action<Pawn[]> OnPlayerHasMovedEvent;
        public event Action OnFullRoundCompletedEvent;
        public event Action<IGamePlayer> OnInvalidResponseEvent;

        //Invoked in GameAction
        public event Action<Pawn[]> OnMoveEvent;
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

        public void InvokeOnMoveEvent(Pawn[] pawns) => OnMoveEvent?.Invoke(pawns);

        public void InvokeOnNewPlayerOptionEvent(IGamePlayer player, PlayerOption option) =>
            OnNewPlayerOptionEvent?.Invoke(player, option);

        public void InvokeOnPlayerHasMoved(Pawn[] pawns) => OnPlayerHasMovedEvent?.Invoke(pawns);
        public void InvokeOnFullRoundCompletedEvent() => OnFullRoundCompletedEvent?.Invoke();
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