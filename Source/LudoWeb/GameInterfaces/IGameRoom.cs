using System.Collections.Generic;
using System.Threading.Tasks;
using LudoDataAccess.Models;
using LudoWeb.GameClasses;

namespace LudoWeb.GameInterfaces
{
    public interface IGameRoom
    {
        string GameId { get; }
        Game Game { get; }
        List<Client> Clients { get; set; }
        Task Start();
        void AddAiPlayer();
        void ConnectNetworkPlayer(string connectionId);
    }
}