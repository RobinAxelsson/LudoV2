using System;
using System.Collections.Generic;

namespace LudoGame.GameEngine.Interfaces
{
    public interface IFunctionRegistrar
    {
        void RegRollDice(Func<int> rollDice);
        void RegFirstPlayer(Func<List<IGamePlayer>, IGamePlayer> getFirstPlayer);
    }
}