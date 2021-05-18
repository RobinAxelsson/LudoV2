using LudoAPI.Models;
using ModelEnum = LudoAPI.DataAccess.ModelEnum;

namespace LudoAPI.GameEngine.Interfaces
{
    public interface IGamePlayer
    {
        ModelEnum.TeamColor Color { get; set; }
        Pawn[] ChoosePlay(PlayerOption playerOption);
    }
}