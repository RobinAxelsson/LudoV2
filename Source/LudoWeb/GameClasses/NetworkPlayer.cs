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
        }
        public ICollection<Pawn> Pawns { get; set; }
        public List<Pawn> MovePawn;
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
            _networkManager.AskPlayerOption(_client.ConnectionId, playerOption);
            int Count = 0;
            do
            {
                Thread.Sleep(1000);
                if (_timeoutSeconds < Count)
                {
                    Debug.WriteLine("Player was timedout");
                    return await _backupStephan.ChoosePlay(playerOption);
                }
            } while (MovePawn.Count == 0);

            var responsePawn = MovePawn.ToArray();
            MovePawn.Clear();
            return responsePawn;
        }
    }
}