using System;
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
        public async Task SendGameMessage(string playerName, string message, string gameId)
        {
            
            await _gameContext.Clients.Group(gameId)
                .SendAsync("ReceiveGameMessage",playerName, message);
        }
        public async Task SendRolledMessage(string playerName, string rollMessage, int diceRoll, string gameId)
        {

            await _gameContext.Clients.Group(gameId)
                .SendAsync("ReceiveRolledMessage", playerName, rollMessage, diceRoll);
        }

        public async Task StartGame(string gameId)
        {
            await _gameContext.Clients.Group(gameId)
                .SendAsync("GameStarted");
            var room = Rooms.Find(r => r.GameId == gameId);
            room.StartGame(); //should not be awaited
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

        public async Task SendNewPawns(string gameId, Pawn[] pawns)
        {
            await _gameContext.Clients.Group(gameId)
                .SendAsync("JoinGameMessage", pawns);
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
            await GetClientProxy(connectionId).SendAsync("ReceiveOption", playerOption.PawnsToMove, playerOption.CanTakeOutTwo, playerOption.DiceRoll);
        }

        public void RedirectPickedPawnFromGameHub(Pawn[] pawns, string connectionId)
        {
            var player = Rooms.SelectMany(r => r.NetworkPlayers)
                .SingleOrDefault(p => p.Client.ConnectionId == connectionId);
            if (player == null) throw new Exception("ConnectionId is not in any room");
            player.PawnsToMove = pawns.ToList();
            player.HasReceived = true;
            Debug.WriteLine("Networkplayer state changed: has received == true");
        }
        public IClientProxy GetClientProxy(string connectionId)
        {
        
            return _gameContext.Clients.Client(connectionId);
        }
        public IClientProxy GetGroupProxy(string gameId)
        {
            if (gameId == null) return null;
            return _gameContext.Clients.Group(gameId);
        }
        /*
        private string GameId(string connectionId)
        {
            var room = Rooms.SingleOrDefault(r => r.Clients.Select(c => c.ConnectionId).Contains(connectionId));
            return room.GameId;
        }
        */

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
    }
}
