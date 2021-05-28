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
        private IGameNetworkManager _networkManager;
        private IGamePlayer _backupStephan;
        public NetworkPlayer(IGameNetworkManager manager, GameEnum.TeamColor color, Client client, IGamePlayer stephan)
        {
            _networkManager = manager;
            Color = color;
            Client = client;

            _backupStephan = stephan;
            PawnsToMove = new();
        }
        public ICollection<Pawn> Pawns { get; set; }
        public List<Pawn> PawnsToMove { get; set; }
        public int Result { get; set; }
        public bool NextToThrow { get; set; }
        public bool Disconnected { get; set; }
        public Client Client { get; set; }
        public GameEnum.TeamColor Color { get; set; }
        public bool HasReceived { get; set; } = false;
        public async Task<Pawn[]> ChoosePlay(PlayerOption playerOption)
        {
            Debug.WriteLine("Player ChoosePlay");

            Debug.WriteLine("About to ask player for option");
            //await kills next
            _networkManager.AskPlayerOption(Client.ConnectionId, playerOption);
            Debug.WriteLine("Before loop; HasReceived value: " + HasReceived);
            bool first = true;
            do
            {
                if (first)
                {
                    Debug.WriteLine("Networkplayer is waiting");
                    first = false;
                }

                await Task.Delay(1000);
            } while (!HasReceived);
            //TODO thread never gets here? Yes but without await!
            Debug.WriteLine("Networkplayer has received");
            HasReceived = false;
            var responsePawn = PawnsToMove.ToArray();
            PawnsToMove.Clear();
            return responsePawn;
        }
    }
}