using System;
using LudoAPI.Models;
using ModelEnum = LudoAPI.DataAccess.ModelEnum;

namespace LudoAPI.GameEngine.Interfaces
{
    public interface IGameActionEvents
    {
        public event Action<ModelEnum.TeamColor, int> OnMoveEvent;
        public event Action<ModelEnum.TeamColor> OnTakeOutTwoEvent;

        public event Action<Pawn> OnBounceEvent;
        public event Action<Pawn, int> OnGoalEvent;
        public event Action<Pawn> OnAllTeamPawnsOutEvent;
        public event Action<Pawn, ModelEnum.TeamColor, int> OnEradicationEvent;
        public event Action<ModelEnum.TeamColor> GameLoserEvent;
        public event Action GameOverEvent;
        public event Action<Pawn> OnSafeZoneEvent;
    }
}