using System.Collections.Concurrent;
using LudoAPI.Models;
using LudoGame.GameEngine.Classes;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace LudoWeb.Hubs
{
    public class GameHub : Hub
    {
        //List of active players on the server
        private static readonly ConcurrentBag<Player> Players = new ConcurrentBag<Player>();
        //List of active games on the server
        private static readonly ConcurrentBag<Game> Games = new ConcurrentBag<Game>();
        
        
        public void OnGameStart(string connectionId)
        {
            //Send data to API
            //Get data back
            //Send that data in SignalR
          //  Clients.Client(Context.ConnectionId).SendAsync();
        }

        public void BroadcastMessage(string user, string message)
        {
            Clients.All.SendAsync("ReceiveMessage", user, message);
        }
       
    }
}