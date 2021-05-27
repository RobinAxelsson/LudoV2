using System.Threading.Tasks;
using LudoGame.GameEngine.Classes;
using LudoWeb.GameClasses;
using Microsoft.AspNetCore.SignalR;

namespace LudoWeb.GameInterfaces
{
    public interface IGameNetworkManager
    {
        IGameRoom AddGameRoom(string gameId);
        Task SendGameMessage(string playerName, string message, string gameId);
        Task UpdatePawns(Pawn[] pawns, string gameId);
        Task AskPlayerOption(string connectionId, PlayerOption playerOption);
        IClientProxy GetClientProxy(string connectionId);
        IClientProxy GetGroupProxy(string connectionId);
        Task StartGame(string gameId);
    }
}