using System.Collections.Generic;
using System.Threading.Tasks;
using LudoGame.GameEngine;
using LudoGame.GameEngine.Classes;
using LudoGame.GameEngine.Interfaces;

namespace LudoWeb.GameInterfaces
{
    public interface INetworkPlayer: IGamePlayer
    {
        ICollection<Pawn> Pawns { get; set; }
        List<Pawn> PawnsToMove { get; set; }
        int Result { get; set; }
        bool NextToThrow { get; set; }
        bool Disconnected { get; set; }
        public bool PlayerClientHasConnectionId(string connectionId);
        GameEnum.TeamColor Color { get; set; }
        Task<Pawn[]> ChoosePlay(PlayerOption playerOption);
    }
}