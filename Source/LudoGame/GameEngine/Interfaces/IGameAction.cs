using System.Collections.Generic;
using LudoGame.GameEngine.Classes;

namespace LudoGame.GameEngine.Interfaces
{
    public interface IGameAction
    {
        void Act(Pawn[] pawns, int diceRoll);
        void SetUpPawns(List<Pawn> allPawns);
    }
}