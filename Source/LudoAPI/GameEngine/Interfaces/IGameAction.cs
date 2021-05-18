using LudoAPI.Models;

namespace LudoAPI.GameEngine.Interfaces
{
    public interface IGameAction
    {
        void Act(Pawn[] pawns, int diceRoll);
    }
}