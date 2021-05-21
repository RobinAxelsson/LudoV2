using System.Collections.Generic;

namespace LudoGame.GameEngine.Interfaces
{
    public interface IGamePlay
    {
        public int RoundCount { get; set; }
        List<IGamePlayer> ReadPlayers();
        void Start(IGameInfo gameInfo);
        GameEnum.GameStatus GameStatus { get; set; }
    }
}