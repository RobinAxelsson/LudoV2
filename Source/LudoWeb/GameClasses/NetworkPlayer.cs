using System.Collections.Generic;
using System.Runtime.InteropServices;
using LudoGame.GameEngine;
using LudoGame.GameEngine.Classes;
using LudoGame.GameEngine.Interfaces;

namespace LudoWeb.GameClasses
{
    public class NetworkPlayer : IGamePlayer
    {
        private Client _client;
        private GameNetworkManager _networkManager;
        public NetworkPlayer(GameNetworkManager manager, GameEnum.TeamColor color, Client client)
        {
            _networkManager = manager;
            Color = color;
            _client = client;
        }
        public ICollection<Pawn> Pawns { get; set; }
        public int Result { get; set; }
        public bool NextToThrow { get; set; }
        public GameEnum.TeamColor Color { get; set; }
        public Pawn[] ChoosePlay(PlayerOption playerOption)
        {
            //Send to client
            return null;
        }
    }
}