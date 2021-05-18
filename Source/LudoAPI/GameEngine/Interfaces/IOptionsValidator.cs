using LudoAPI.Models;
using ModelEnum = LudoAPI.DataAccess.ModelEnum;

namespace LudoAPI.GameEngine.Interfaces
{
    public interface IOptionsValidator
    {
        PlayerOption GetPlayerOption(ModelEnum.TeamColor color, int diceRoll);
        bool ValidateResponse(PlayerOption option, Pawn[] pawnResponse);
    }
}