using System.Collections.Generic;
using System.Threading.Tasks;
using LudoGame.GameEngine;
using LudoGame.GameEngine.Classes;
using LudoGame.GameEngine.Interfaces;
using LudoWeb.GameClasses;

namespace LudoWeb.GameInterfaces
{
    public interface INetworkPlayer: IGamePlayer
    {
        ICollection<Pawn> Pawns { get; set; }
        List<Pawn> PawnsToMove { get; set; }
        int Result { get; set; }
        bool NextToThrow { get; set; }
        bool Disconnected { get; set; }
        Client Client { get; set; }
        
        GameEnum.TeamColor Color { get; set; }
        Task<Pawn[]> ChoosePlay(PlayerOption playerOption);
        bool HasReceived { get; set; }
    }
}