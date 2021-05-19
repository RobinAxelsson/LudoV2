using System;
using LudoGame.GameEngine.Classes;

namespace LudoGame.GameEngine.Interfaces
{
    public interface IGameActionEvent
    {
        public event Action<GameEnum.TeamColor, int> OnMoveEvent;
        public event Action<GameEnum.TeamColor> OnTakeOutTwoEvent;
        public event Action OnNewGameEvent;
        public event Action OnRestartedGameEvent;

        public event Action<Pawn> OnBounceEvent;
        public event Action<Pawn, int> OnGoalEvent;
        public event Action<Pawn> OnAllTeamPawnsOutEvent;
        public event Action<Pawn, GameEnum.TeamColor, int> OnEradicationEvent;
        public event Action<GameEnum.TeamColor> GameLoserEvent;
        public event Action GameOverEvent;
        public event Action<Pawn> OnSafeZoneEvent;
    }
}