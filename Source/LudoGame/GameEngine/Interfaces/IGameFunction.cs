using System;
using System.Collections.Generic;

namespace LudoGame.GameEngine.Interfaces
{
    public interface IGameFunction
    {
        public Func<int> RollDice { get; }
        public Func<List<IGamePlayer>, IGamePlayer> GetFirstPlayer { get; }
        public Func<int, bool> GameKeep { get; }
    }
}