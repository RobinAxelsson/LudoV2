using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LudoGame.GameEngine.Classes;

namespace LudoGame.GameEngine.Interfaces
{
    public interface IGamePlay
    {
        public int RoundCount { get; set; }
        List<IGamePlayer> ReadPlayers();
        Task<GameEnum.GameStatus> Start(List<IGamePlayer> players, Func<Pawn[], Task> refreshPawns);
        GameEnum.GameStatus GameStatus { get; set; }
    }
}