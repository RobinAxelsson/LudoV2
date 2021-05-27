using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LudoDataAccess.Database;
using LudoGame.GameEngine.Classes;
using LudoGame.GameEngine.Configuration;
using LudoWeb.GameInterfaces;
using Microsoft.AspNetCore.SignalR;

namespace LudoWeb.GameClasses
{
    public class GameNetworkManager : IGameNetworkManager
    {
        private ILudoRepository _dbRepository;
        private IHubContext<GameHub> _gameContext { get; }
        public List<IGameRoom> Rooms;
        private LudoNetworkFactory _networkFactory;
        private AbstractFactory _gameServiceFactory;
        public GameNetworkManager(IHubContext<GameHub> gameContext, AbstractFactory gameServiceFactory, LudoNetworkFactory networkFactory)
        {
            _networkFactory = networkFactory;
            _gameContext = gameContext;
            _gameServiceFactory = gameServiceFactory;
            Rooms = new();
            
        }
        public IGameRoom AddGameRoom(string gameId)
        {
            var room = _networkFactory.GameRoom(_gameServiceFactory, this, gameId, _networkFactory);
            Rooms.Add(room);
            Debug.WriteLine($"GameRoom was added with id: {gameId}!");
            return room;
        }
        //From server to clients
        public async Task SendGameMessage(string method, string message, string gameId)
        {
            
            await _gameContext.Clients.Group(gameId)
                .SendAsync(method, message);
            /*
            await GetGroupProxy(gameId)
                .SendAsync("SendMessage", message);
                */
        }
        public async Task SendJoinGameMessage(string message, int playerIndex, string gameId)
        {
            
            await _gameContext.Clients.Group(gameId)
                .SendAsync("JoinGameMessage", message, playerIndex);
            /*
            await GetGroupProxy(gameId)
                .SendAsync("SendMessage", message);
                */
        }
   
        public async Task AddUserToGroup(string connectionId, string gameId)
        {
            await _gameContext.Groups.AddToGroupAsync(connectionId, gameId);
        }
        public async Task UpdatePawns(Pawn[] pawns, string gameId)
        {
            await GetGroupProxy(gameId)
                .SendAsync("UpdatePawns", pawns);
        }

        public async Task AskPlayerOption(string connectionId, PlayerOption playerOption)
        {
            await GetClientProxy(connectionId).SendAsync("ReceiveOption", playerOption);
        }
        //public async Task AddClientToRoom(string gameId, string clientId) //Invokes by client OnConnection
        //{
        //    //var game = _dbRepository.Games.SingleOrDefault(x => x.GameId == gameId);
        //    if (gameId == null)
        //    {
        //        Debug.WriteLine("A client try to join a non-existing game/room");
        //    }
        //    else
        //    {
        //        var room = Rooms.SingleOrDefault(x => x.Game.GameId == gameId);
        //        if (room == null)
        //        {
        //            var client = new Client(clientId);
        //            var newRoom = new GameRoom(gameId);
        //            newRoom.Clients.Add(client);
        //            Rooms.Add(newRoom);
        //        }
        //        else
        //        {
        //            if (room.Clients.Exists(x => x.ConnectionId == clientId))
        //            {
        //                Debug.WriteLine("An existing client joined again");
        //            }
        //            else
        //            {
        //                var client = new Client(clientId);
        //                room.Clients.Add(client);
        //                Debug.WriteLine($"A new client joined room {gameId}");
        //            }
        //        }
        //    }
        //}

        //From client to game-room
        public IClientProxy GetClientProxy(string connectionId)
        {
            return _gameContext.Clients.Client(connectionId);
        }
        public IClientProxy GetGroupProxy(string connectionId)
        {
            var roomId = GameId(connectionId);
            if (roomId == null) return null;
            return _gameContext.Clients.Group(roomId);
        }
        private string GameId(string connectionId)
        {
            var room = Rooms.SingleOrDefault(r => r.Clients.Select(c => c.ConnectionId).Contains(connectionId));
            return room.GameId;
        }

        //private bool IsNotGameOver(ModelEnum.GameStatus status)
        //{
        //    return
        //        status == ModelEnum.GameStatus.Paused ||
        //        status == ModelEnum.GameStatus.Playing ||
        //        status == ModelEnum.GameStatus.Created ||
        //        status == ModelEnum.GameStatus.WaitingForAllPlayers;
        //}
    }
}
