using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LudoDataAccess.Database;
using LudoGame.GameEngine;
using LudoGame.GameEngine.Configuration;
using LudoGame.GameEngine.Interfaces;
using LudoDataAccess.Models;
using LudoGame.GameEngine.AI;
using LudoWeb.GameInterfaces;
using Newtonsoft.Json;

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
            _waitingPlayers = new ();
            NetworkPlayers = new();
            Colors = new List<GameEnum.TeamColor>
            {
                GameEnum.TeamColor.Blue,
                GameEnum.TeamColor.Green,
                GameEnum.TeamColor.Red,
                GameEnum.TeamColor.Yellow
            };
            GameId = gameId;
        }
        
        public string GameId { get; }
        public Game Game { get; private set; }
        public List<Client> Clients { get; set; }
        public List<INetworkPlayer> NetworkPlayers { get; private set; }
        private List<(GameEnum.TeamColor Color, Client Client)> _waitingPlayers { get; set; }
        private List<GameEnum.TeamColor> Colors { get; set; }
        private Client GetClient(string connectionId) => Clients.SingleOrDefault(x => x.ConnectionId == connectionId);
        public async Task StartGame()
        {
            var gamePlayers = CreateGamePlayers();
            Debug.WriteLine(("gameroom with game id " + GameId + " created gameplayer: " + JsonConvert.SerializeObject(gamePlayers)));
            
            Game = new Game(GameId);
            Game.GameStatus = ModelEnum.GameStatus.Created;
            Debug.WriteLine("GamePlay is about to start");
            _gamePlay.Start(gamePlayers);
        }
        private List<IGamePlayer> CreateGamePlayers()
        {
            var gamePlayers = new List<IGamePlayer>();
            foreach (var wp in _waitingPlayers)
            {
             if(wp.Client.Player.Type == ModelEnum.PlayerType.Stephan)
                {
                    var aiPlayer = _factory.AIPlayer(wp.Color, _boardCollection);
                    gamePlayers.Add(aiPlayer);
                }
             else
                {
                    var networkPlayer = _factory.NetworkPlayer(
               _networkManager, wp.Color, wp.Client,
               _factory.AIPlayer(wp.Color, _boardCollection));
                    NetworkPlayers.Add(networkPlayer);
                    gamePlayers.Add(networkPlayer);
                }
            }

            return gamePlayers;
        }
        public void ConnectAiPlayer(Client client)
        {
            if (_waitingPlayers.Count < 4)
            {
                _waitingPlayers.Add((Colors[0], client));
                Colors.RemoveAt(0);
               
              //  Debug.WriteLine(Colors[0]);
            }
        }
        public void ConnectNetworkPlayer(Client client)
        {
           // var client = GetClient(connectionId);
            if (_waitingPlayers.Count < 4)
            {
                _waitingPlayers.Add((Colors[0], client));
                Colors.RemoveAt(0);
            }
        }

    }
}
