using System.Collections.Generic;
using LudoGame.GameEngine.Interfaces;

namespace LudoGameTest.Stubs
{
    public class GameInfo : IGameInfo
    {
        private static int _globalId;

        static GameInfo()
        {
            _globalId++;
        }
        public GameInfo(List<IGamePlayer> players)
        {
            Players = players;
            Id = _globalId;
        }
        public int Id { get; }
        public List<IGamePlayer> Players { get; set; }
    }
}
