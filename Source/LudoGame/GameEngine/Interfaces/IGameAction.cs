using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LudoGame.GameEngine.Classes;

namespace LudoGame.GameEngine.Interfaces
{
    public interface IGameAction
    {
        void Act(Pawn[] pawns, int diceRoll);
        void SetUpPawns(List<Pawn> allPawns);
        Func<Pawn[], Task> refreshPawns { get; set; }
    }
}