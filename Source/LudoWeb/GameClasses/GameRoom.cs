using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using LudoGame.GameEngine;
using LudoGame.GameEngine.Configuration;
using LudoGame.GameEngine.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using LudoDataAccess.Models;

namespace LudoWeb.GameClasses
{
    public class GameRoom
    {
        private IGameEvent _gameEvent;
        private IGamePlay _gamePlay;
        private GameNetworkManager _networkManager;
        public GameRoom(LudoFactory factory, GameNetworkManager manager)
        {
            _networkManager = manager;
            var provider = factory.BuildLudoProvider();
            _gamePlay = provider.GetGameService<IGamePlay>();
            _gameEvent = provider.GetGameService<IGameEvent>();
            Clients = new();

            Colors = new List<GameEnum.TeamColor>
            {
                GameEnum.TeamColor.Blue,
                GameEnum.TeamColor.Green,
                GameEnum.TeamColor.Red,
                GameEnum.TeamColor.Yellow
            };
        }
        public string Id() => Game.GameId;
        public Game Game { get; }
        public List<Client> Clients { get; set; }
        private List<(GameEnum.TeamColor Color, Client Client)> WaitingPlayers { get; set; }
        private List<GameEnum.TeamColor> Colors { get; set; }
        private Client GetClient(string connectionId) => Clients.SingleOrDefault(x => x.ConnectionId == connectionId);
        public void ConnectOrStart(string connectionId)
        {
            var client = GetClient(connectionId);
            if (client.Account != null && WaitingPlayers.Count < 4)
            {
                WaitingPlayers.Add((Colors[0], client));
                Colors.Remove(0);
            }
            if (WaitingPlayers.Count == 4)
            {
                var gamePlayers = new List<IGamePlayer>();
                foreach (var wp in WaitingPlayers)
                {
                    gamePlayers.Add(new NetworkPlayer(_networkManager, wp.Color, wp.Client));
                }
                _gamePlay.Start(gamePlayers);
            }
        }
    }
}
