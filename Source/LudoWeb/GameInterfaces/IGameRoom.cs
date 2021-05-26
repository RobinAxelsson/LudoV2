using System.Collections.Generic;
using LudoDataAccess.Models;
using LudoWeb.GameClasses;

namespace LudoWeb.GameInterfaces
{
    public interface IGameRoom
    {
        string GameId { get; }
        Game Game { get; }
        List<Client> Clients { get; set; }
        void Start();
        void AddAiPlayer();
        void ConnectNetworkPlayer(string connectionId);
    }
}