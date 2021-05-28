using System;
using LudoGame.GameEngine.Classes;

namespace LudoGame.GameEngine.Interfaces
{
    public interface IGameEvent
    {
        //Invoked in GamePlay
        public event Action<IGamePlayer, PlayerOption> OnNewPlayerOptionEvent;
        public event Action OnFullRoundCompletedEvent;
        public event Action<IGamePlayer> OnInvalidResponseEvent;
        public event Action<Pawn[]> OnPlayerHasMovedEvent;
       

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
        public void InvokeOnNewPlayerOptionEvent(IGamePlayer player, PlayerOption option);
        public void InvokeOnPlayerHasMoved(Pawn[] pawns);
        public void InvokeOnFullRoundCompletedEvent();
        public void InvokeOnNewGameEvent();
        public void InvokeOnInvalidResponseEvent(IGamePlayer player);
        public void InvokeOnAllTeamPawnsOutEvent(GameEnum.TeamColor pawnColor);
        public void InvokeOnGoalEvent(GameEnum.TeamColor color, int pawnsLeft);
        public void InvokeOnGameOverEvent();
        public void InvokeOnMoveEvent(Pawn[] pawns);
        public void InvokeOnEradicationEvent(GameEnum.TeamColor pawnColor, GameEnum.TeamColor enemyColor,
            int deadEnemies);

        public void InvokeOnBounceEvent(GameEnum.TeamColor color);
        public void InvokeOnSafeZoneEvent(GameEnum.TeamColor color);
    }
}