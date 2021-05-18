using LudoGame.GameEngine.Classes;

namespace LudoGame.GameEngine.Interfaces
{
    public interface IOptionsValidator
    {
        PlayerOption GetPlayerOption(GameEnum.TeamColor color, int diceRoll);
        bool ValidateResponse(PlayerOption option, Pawn[] pawnResponse);
    }
}