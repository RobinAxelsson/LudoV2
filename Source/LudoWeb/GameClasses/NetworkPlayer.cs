using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using LudoGame.GameEngine;
using LudoGame.GameEngine.AI;
using LudoGame.GameEngine.Classes;
using LudoGame.GameEngine.Interfaces;
using LudoWeb.GameInterfaces;

namespace LudoWeb.GameClasses
{
    public class NetworkPlayer : INetworkPlayer
    {
        private Client _client;
        private IGameNetworkManager _networkManager;
        private IGamePlayer _backupStephan;
        public NetworkPlayer(IGameNetworkManager manager, GameEnum.TeamColor color, Client client, IGamePlayer stephan)
        {
            _networkManager = manager;
            Color = color;
            _client = client;
            _timeoutSeconds = 30;
            _backupStephan = stephan;
            PawnsToMove = new();
        }
        public ICollection<Pawn> Pawns { get; set; }
        public List<Pawn> PawnsToMove { get; set; }
        public bool PlayerClientHasConnectionId(string connectionId)
        {
            return _client.ConnectionId == connectionId;
        }
        public int Result { get; set; }
        public bool NextToThrow { get; set; }
        public bool Disconnected { get; set; }
        private int _timeoutSeconds { get; }
        public GameEnum.TeamColor Color { get; set; }
        public async Task<Pawn[]> ChoosePlay(PlayerOption playerOption)
        {
            Debug.WriteLine("Player ChoosePlay");
       
            if (Disconnected == true && _backupStephan != null)
            {
                return await _backupStephan.ChoosePlay(playerOption);
            }
       
            Debug.WriteLine("About to ask player for option");
            await _networkManager.AskPlayerOption(_client.ConnectionId, playerOption);
        
           int Count = 0;
           do
           {
               Count++;
               await Task.Delay(1000);
               if (Count >= _timeoutSeconds) 
               {
                   Debug.WriteLine("Player was timedout");
                   return await _backupStephan.ChoosePlay(playerOption);
               }
           } while (PawnsToMove.Count == 0);
         
            var responsePawn = PawnsToMove.ToArray();
            PawnsToMove.Clear();
            return responsePawn;
        }
    }
}