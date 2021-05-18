using System.Collections.Generic;
using LudoGame.GameEngine.Classes;

namespace LudoGame.GameEngine.Interfaces
{
    public interface IGamePlayer
    {
        ICollection<Pawn> Pawns { get; set; }
        int Result { get; set; }
        bool NextToThrow { get; set; }
        GameEnum.TeamColor Color { get; set; }
        Pawn[] ChoosePlay(PlayerOption playerOption);
    }
}