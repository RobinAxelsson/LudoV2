using System.Collections.Generic;

namespace LudoGame.GameEngine.Interfaces
{
    public interface IGameInfo
    {
        public int Id { get; }
        public List<IGamePlayer> Players { get; set; }
    }
}
