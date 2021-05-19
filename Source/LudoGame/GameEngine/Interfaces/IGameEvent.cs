using System;
using LudoGame.GameEngine.Classes;

namespace LudoGame.GameEngine.Interfaces
{
    public interface IGameEvent
    {
        //event Action OnRoundCompletedEvent;
        //event Action<IGamePlayer> OnInvalidResponseEvent;
        //event Action<Pawn[]> OnMoveEvent;
        //event Action<GameEnum.TeamColor> OnTakeOutTwoEvent;
        //event Action OnNewGameEvent;
        //event Action OnRestartedGameEvent;
        //event Action<GameEnum.TeamColor> OnBounceEvent;
        //event Action<GameEnum.TeamColor, int> OnGoalEvent;
        //event Action<GameEnum.TeamColor> OnAllTeamPawnsOutEvent;
        //event Action<GameEnum.TeamColor, GameEnum.TeamColor, int> OnEradicationEvent;
        //event Action<GameEnum.TeamColor> GameLoserEvent;
        //event Action OnGameOverEvent;
        //event Action<GameEnum.TeamColor> OnSafeZoneEvent;
        public void InvokeOnRoundCompletedEvent();
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