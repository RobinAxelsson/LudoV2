using System.Collections.Generic;
using System.Threading.Tasks;

namespace LudoGame.GameEngine.Interfaces
{
    public interface IGamePlay
    {
        public int RoundCount { get; set; }
        List<IGamePlayer> ReadPlayers();
        Task Start(List<IGamePlayer> players);
        GameEnum.GameStatus GameStatus { get; set; }
    }
}