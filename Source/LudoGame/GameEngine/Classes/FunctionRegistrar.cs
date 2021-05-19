using System;
using System.Collections.Generic;
using System.Linq;
using LudoGame.GameEngine.Interfaces;

namespace LudoGame.GameEngine.Classes
{
    public class FunctionRegistrar : IFunctionRegistrar
    {
        private readonly Random _rand = new();
        public void RegRollDice(Func<int> rollDice) => rollDice = RollDice;
        private int RollDice() => _rand.Next(100, 699) / 100;
        public void RegFirstPlayer(Func<List<IGamePlayer>, IGamePlayer> getFirstPlayer) =>
        getFirstPlayer = GetFirstPlayer;
        private IGamePlayer GetFirstPlayer(List<IGamePlayer> players)
        {
            var firsts = players.Where(p => p.NextToThrow == true).ToList();
            if (firsts.Any() == false || firsts.Count() > 1)
            {
                var startIndex = new Random().Next(100, 499) / 100;
                players.ForEach(p => p.NextToThrow = false);
                var firstPlayer = players[startIndex];
                firstPlayer.NextToThrow = true;
                return firstPlayer;
            }
            return players.Find(p => p.NextToThrow == true) ?? throw new NullReferenceException();
        }
    }
}