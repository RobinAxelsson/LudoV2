using System;
using System.Diagnostics;
using System.Threading.Tasks;
using LudoDataAccess;
using LudoGame.GameEngine.Classes;
using Microsoft.AspNetCore.SignalR;

namespace LudoWeb.GameClasses
{
    //This class bridges client-server functions
    public class GameHub : Hub
    {
        private GameNetworkManager _networkManager;
        private readonly IDatabaseManagement _dbm;
        public GameHub(GameNetworkManager manager, IDatabaseManagement dbm)
        {
            _networkManager = manager;
            _dbm = dbm;
        }
        public async Task AddAi(string color)
        {

        }
        public async Task SelectColor(string color)
        {

        }
        public async Task ConnectToGameRoom(string gameId)
        {
          // await _networkManager.AddClientToRoom(gameId, Context.ConnectionId);
        }
        public async Task MovePawn(Pawn[] pawns)
        {
            //To gamelogic
            //Return error if invalid pawn
        }
        public async Task SendChatMessage(string message)
        {
            var proxy = _networkManager.GetGroupProxy(Context.ConnectionId);
            await proxy.SendAsync("NewChat", message);
        }
        public async Task OnLostConnection()
        {

        }

        public async Task ValidateToken(string token)
        {
            //If token is null we will just skip ahead and return the gameId
            if (token == null)
                await Clients.Caller.SendAsync("TokenValidated", false, Guid.NewGuid().ToString("N"));
            
            
            var result = _dbm.ValidateToken(token);
            if(result.success)
                await Clients.Caller.SendAsync("TokenValidated", true);
            else 
                //If token validation fails we generate a gameId for proper redirection later on
                await Clients.Caller.SendAsync("TokenValidated", false, Guid.NewGuid().ToString("N"));
        
        }

        public async Task AddGameRoom(string gameId)
        {
            try
            {
                if (gameId == null)
                    gameId = Guid.NewGuid().ToString("N");

                var room = _networkManager.AddGameRoom(gameId);
                var client = new Client(Context.ConnectionId);
                room.Clients.Add(client);
                await Clients.Caller.SendAsync("GameRoomAdded");
            }
            catch(Exception ex)
            {
                Debug.Write(ex.ToString());
            }
       
        }
    }
}