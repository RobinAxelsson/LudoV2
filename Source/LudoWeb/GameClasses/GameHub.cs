using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using LudoDataAccess;
using LudoDataAccess.Database;
using LudoDataAccess.Models;
using LudoDataAccess.SMTP;
using LudoGame.GameEngine.Classes;
using Microsoft.AspNetCore.SignalR;

namespace LudoWeb.GameClasses
{
    //This class bridges client-server functions
    public class GameHub : Hub
    {
        private GameNetworkManager _networkManager;
        private readonly IDatabaseManagement _dbm;
        private readonly EmailClient _emailClient;
        public GameHub(GameNetworkManager manager, IDatabaseManagement dbm, EmailClient emailClient)
        {
            _networkManager = manager;
            _dbm = dbm;
            _emailClient = emailClient;
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

        public async Task AddGameRoom(string connectionId, string gameId, string playerName, string token)
        {
            try
            {
                if (gameId == null)
                    gameId = Guid.NewGuid().ToString("N");

                
                var player = _dbm.GetPlayerFromToken(token);
                if (player == null)
                {
                    throw new Exception("Dang it!");
                }
                var room = _networkManager.AddGameRoom(gameId);
                var client = new Client(Context.ConnectionId) {Player = player, Name = playerName};
                room.Clients.Add(client);
                await _networkManager.AddUserToGroup(connectionId, gameId);
                await Clients.Caller.SendAsync("GameRoomAdded", gameId, playerName);
            }
            catch(Exception ex)
            {
                Debug.Write(ex.ToString());
            }
       
        }

        public async Task GetPlayerName()
        {
            await Clients.Caller.SendAsync("PlayerNameReceived", ReturnPlayerName());
        }
        public async Task InvitePlayer(string recipient, string gameId, string gameUrl, string token)
        {
            var account = _dbm.GetAccountFromToken(token);
            if (account != null)
            {
                var stringArray = new string[1];
                stringArray[0] = recipient;
                _emailClient.SendInvite(stringArray, gameId, gameUrl, account);
                await Clients.Caller.SendAsync("PlayerInvited");
            }
        }

        public async Task AddAi(string gameId)
        {
            var room = _networkManager.Rooms.SingleOrDefault(r => r.GameId == gameId);
            var player = new Player()
            {
                Account = null,
                Type = ModelEnum.PlayerType.Stephan
            };
            var client = new Client(null) {Player = player, Name = ReturnPlayerName() + "(AI)"};
            room.Clients.Add(client);
            await Clients.Caller.SendAsync("AiAdded");
        }
        public async Task JoinGameMessage(string playerName, string gameId)
        {
            var room = _networkManager.Rooms.SingleOrDefault(r => r.GameId == gameId);
            var client = room.Clients
                .SingleOrDefault(c => c.Name == playerName);
            var clientIndex = room.Clients.IndexOf(client);
         
            await _networkManager.SendJoinGameMessage(playerName, clientIndex, gameId);
        } 
        public async Task RetrieveJoinNotifications(string playerName, string gameId)
        {
            var room = _networkManager.Rooms.SingleOrDefault(r => r.GameId == gameId);
            var clients = room.Clients.Where(c => c.Name != playerName).ToArray();
            var indexList = new List<int>();
            var nameList = new List<string>();
            foreach (var client in clients)
            {
                indexList.Add(room.Clients.IndexOf(client));
                nameList.Add(client.Name);
            }
            await Clients.Caller.SendAsync("RetrieveJoinNotifications",nameList.ToArray(), indexList.ToArray(), room.Clients.Count);
        } 
        //RetrieveJoinNotifications
        public async Task JoinRoom(string connectionId, string gameId, string playerName, string token)
        {
            var account = _dbm.GetAccountFromToken(token);
            if (account != null)
            {
                var room = _networkManager.Rooms.SingleOrDefault(g => g.GameId == gameId);
                if (room != null)
                {
                    try
                    {
                        
                        var player = _dbm.GetPlayerFromToken(token);
                        if (player == null)
                        {
                            throw new Exception("Dang it!");
                        }
                        var client = new Client(Context.ConnectionId) {Player = player, Name = playerName};
                        room.Clients.Add(client);
                        await _networkManager.AddUserToGroup(connectionId, gameId);
                        await Clients.Caller.SendAsync("JoinedRoom",true, "success");
                    }
                    catch (Exception ex)
                    {
                        await Clients.Caller.SendAsync("JoinedRoom",false, ex.Message);
                    }
                }
                else
                {
                    await Clients.Caller.SendAsync("JoinedRoom", false, $"Room with the Game ID {gameId} was not found.");
                }
            }
            else
            {
                await Clients.Caller.SendAsync("JoinedRoom", false, $"Account connected to the token {token} was not found.");
            }
            
        }

        public async Task RoomExists(string gameId)
        {
            var room = _networkManager.Rooms.SingleOrDefault(g => g.GameId == gameId);
            if (room == null)
                await Clients.Caller.SendAsync("RoomExists", false);
            else 
                await Clients.Caller.SendAsync("RoomExists", true);
        }

        public async Task GetAllRooms()
        {
            await Clients.Caller.SendAsync("AllRooms", _networkManager.Rooms.ToArray());
        }

        public async Task SendRoomMessage(string playerName, string input, string gameId)
        {
         await _networkManager.SendGameMessage(playerName, input, gameId);
        }
        private string ReturnPlayerName()
        {
            var adjectives = GetResourceArray("LudoWeb.Resources.adjectives.txt");
          var nouns = GetResourceArray("LudoWeb.Resources.nouns.txt");
            var randomNounIndex = new Random().Next(0, nouns.Length - 1);
            var randomAdjectiveIndex = new Random().Next(0, adjectives.Length - 1);
            return $"{ToUpperFirstLetter(adjectives[randomAdjectiveIndex])} {ToUpperFirstLetter(nouns[randomNounIndex])}";
        }
        private string ToUpperFirstLetter(string source)
        {
            if (string.IsNullOrEmpty(source))
                return string.Empty;
            // convert to char array of the string
            var letters = source.ToCharArray();
            // upper case the first char
            letters[0] = char.ToUpper(letters[0]);
            // return the array made of the new char array
            return new string(letters);
        }

        private string[] GetResourceArray(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
           // var resourceName = "MyCompany.MyProduct.MyFile.txt";
           var result = "";
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                 result = reader.ReadToEnd();
            }
            return result.Split(new string[] {System.Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
        }

        public async Task StartGame(string gameId)
        {
            await _networkManager.StartGame(gameId);
        }
        
    }
}