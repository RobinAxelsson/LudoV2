using System.Collections.Generic;
using LudoGame.GameEngine;
using LudoGame.GameEngine.Classes;
using LudoGame.GameEngine.Interfaces;

namespace LudoWeb.GameInterfaces
{
    public interface INetworkPlayer: IGamePlayer
    {
        ICollection<Pawn> Pawns { get; set; }
        int Result { get; set; }
        bool NextToThrow { get; set; }
        bool Disconnected { get; set; }
        GameEnum.TeamColor Color { get; set; }
        Pawn[] ChoosePlay(PlayerOption playerOption);
    }
}