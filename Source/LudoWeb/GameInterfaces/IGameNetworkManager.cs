using System.Threading.Tasks;
using LudoGame.GameEngine.Classes;
using Microsoft.AspNetCore.SignalR;

namespace LudoWeb.GameInterfaces
{
    public interface IGameNetworkManager
    {
        void AddGameRoom(string gameId);
        void SendGameMessage(string message, string gameId);
        Task UpdatePawns(Pawn[] pawns, string gameId);
        Task AskPlayerOption(string connectionId, PlayerOption playerOption);
        IClientProxy GetClientProxy(string connectionId);
        IClientProxy GetGroupProxy(string connectionId);
    }
}