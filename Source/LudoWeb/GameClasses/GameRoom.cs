using System.Collections.Generic;
using LudoDataAccess.Models;

namespace LudoWeb.GameClasses
{
    public class GameRoom
    {
        public GameRoom(Game game)
        {
            Game = game;
            Clients = new();
        }
        public string Id() => Game.GameId;
        public Game Game { get; }
        public List<Client> Clients { get; set; }

    }
}
