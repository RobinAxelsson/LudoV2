using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LudoGame.GameEngine.Classes;
using Microsoft.AspNetCore.SignalR;

namespace LudoWeb.GameClasses
{
    public class GameNetworkManager
    {
        //private ILudoRepository _dbRepository;
        private IHubContext<GameHub> _gameContext { get; }
        private List<GameRoom> Rooms { get; set; }
        public GameNetworkManager(IHubContext<GameHub> gameContext)
        {
            _gameContext = gameContext;
            Rooms = new();
        }

        //From server to clients
        public async Task SendGameMessage(string message, string gameId)
        {
            await _gameContext.Clients.Group(gameId)
                .SendAsync("GameMessage", message);
        }
        public async Task UpdatePawns(Pawn[] pawns, string gameId)
        {
            await GetGroupProxy(gameId)
                .SendAsync("UpdatePawns", pawns);
        }

        //public async Task AddClientToRoom(string gameId, string clientId) //Invokes by client OnConnection
        //{
        //    //var game = _dbRepository.Games.SingleOrDefault(x => x.GameId == gameId);
        //    if (game == null)
        //    {
        //        Debug.WriteLine("A client try to join a non-existing game/room");
        //    }
        //    else
        //    {
        //        var room = Rooms.SingleOrDefault(x => x.Game.GameId == gameId);
        //        if (room == null)
        //        {
        //            var client = new Client(clientId);
        //            var newRoom = new GameRoom(game);
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

        public IClientProxy GetGroupProxy(string connectionId)
        {
            var roomId = RoomId(connectionId);
            if (roomId == null) return null;
            return _gameContext.Clients.Group(roomId);
        }
        private string RoomId(string connectionId)
        {
            var room = Rooms.SingleOrDefault(r => r.Clients.Select(c => c.ConnectionId).Contains(connectionId));
            return room.Id();
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
