using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using LudoDataAccess.Database;
using LudoGame.GameEngine;
using LudoGame.GameEngine.Configuration;
using LudoGame.GameEngine.Interfaces;
using LudoDataAccess.Models;
using LudoGame.GameEngine.AI;
using LudoWeb.GameInterfaces;

namespace LudoWeb.GameClasses
{
    public class GameRoom : IGameRoom
    {
        private IGameEvent _gameEvent;
        private readonly IGamePlay _gamePlay;
        private readonly IBoardCollection _boardCollection;
        private readonly IGameNetworkManager _networkManager;
        private readonly LudoNetworkFactory _factory;
        private readonly IGameMessager _messager;
        public GameRoom(AbstractFactory gameFactory, IGameNetworkManager manager, string gameId, LudoNetworkFactory factory)
        {
            _networkManager = manager;
            var provider = gameFactory.BuildLudoProvider();
            _gamePlay = provider.GetGameService<IGamePlay>();
            _gameEvent = provider.GetGameService<IGameEvent>();
            _boardCollection = provider.GetGameService<IBoardCollection>();
            Clients = new();
            _factory = factory;
            _messager = factory.GameMessager(_gameEvent, manager, this);
            Colors = new List<GameEnum.TeamColor>
            {
                GameEnum.TeamColor.Blue,
                GameEnum.TeamColor.Green,
                GameEnum.TeamColor.Red,
                GameEnum.TeamColor.Yellow
            };
        }
        public string GameId { get; }
        public Game Game { get; private set; }
        public List<Client> Clients { get; set; }
        private List<(GameEnum.TeamColor Color, Client Client)> WaitingPlayers { get; set; }
        private List<GameEnum.TeamColor> Colors { get; set; }
        private Client GetClient(string connectionId) => Clients.SingleOrDefault(x => x.ConnectionId == connectionId);
        public void Start()
        {
            var gamePlayers = CreateGamePlayers();

            Game = new Game(GameId);
            Game.GameStatus = ModelEnum.GameStatus.Created;
            Debug.WriteLine("Game created with id");
            _gamePlay.Start(gamePlayers);
        }
        private List<IGamePlayer> CreateGamePlayers()
        {
            var gamePlayers = new List<IGamePlayer>();
            if (WaitingPlayers.Count < 4)
            {
                while (WaitingPlayers.Count < 4)
                {
                    AddAiPlayer();
                }
            }
            foreach (var wp in WaitingPlayers)
            {
                if (wp.Client == null)
                {
                    gamePlayers.Add(_factory.AIPlayer(wp.Color, _boardCollection));
                }
                else
                {
                    gamePlayers.Add(_factory.NetworkPlayer(
                        _networkManager, wp.Color, wp.Client, 
                        _factory.AIPlayer(wp.Color, _boardCollection)));
                }
            }

            return gamePlayers;
        }
        public void AddAiPlayer()
        {
            if (WaitingPlayers.Count < 4)
            {
                WaitingPlayers.Add((Colors[0], null));
                Colors.Remove(0);
            }
        }
        public void ConnectNetworkPlayer(string connectionId)
        {
            var client = GetClient(connectionId);
            if (client.Account != null && WaitingPlayers.Count < 4)
            {
                WaitingPlayers.Add((Colors[0], client));
                Colors.Remove(0);
            }
        }

    }
}
