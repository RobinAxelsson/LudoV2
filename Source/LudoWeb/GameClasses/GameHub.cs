using System.Threading.Tasks;
using LudoGame.GameEngine.Classes;
using Microsoft.AspNetCore.SignalR;

namespace LudoWeb.GameClasses
{
    //This class bridges client-server functions
    public class GameHub : Hub
    {
        private GameNetworkManager _networkManager;
        public GameHub(GameNetworkManager manager)
        {
            _networkManager = manager;
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
    }
}