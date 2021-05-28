using System.Collections.Generic;
using System.Threading.Tasks;
using LudoDataAccess.Models;
using LudoWeb.GameClasses;

namespace LudoWeb.GameInterfaces
{
    public interface IGameRoom
    {
        List<INetworkPlayer> NetworkPlayers { get; }
        string GameId { get; }
        Game Game { get; }
        List<Client> Clients { get; set; }
        Task StartGame();
        void AddAiPlayer();
        void ConnectNetworkPlayer(Client client);
    }
}