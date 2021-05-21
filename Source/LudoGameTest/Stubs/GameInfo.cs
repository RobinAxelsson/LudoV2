using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LudoGame.GameEngine.Interfaces;

namespace LudoGame.GameEngine.Classes
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
