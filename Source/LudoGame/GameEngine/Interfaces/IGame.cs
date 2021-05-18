using System.Collections.Generic;

namespace LudoGame.GameEngine.Interfaces
{
    public interface IGameInfo
    {
        public int Id { get; set; }
        public List<IGamePlayer> Players { get; set; }
    }
}
