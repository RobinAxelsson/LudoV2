using System;
using System.Collections.Generic;
using System.Linq;
using LudoGame.GameEngine.Interfaces;

namespace LudoGame.GameEngine.Classes
{
    public class GameFunction : IGameFunction
    {
        public GameFunction()
        {
            RollDice = rollDice;
            GetFirstPlayer = getFirstPlayer;
        }
        private readonly Random _rand = new();
        private int rollDice() => _rand.Next(100, 699) / 100;
        private IGamePlayer getFirstPlayer(List<IGamePlayer> players)
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
        public Func<int> RollDice { get; }
        public Func<List<IGamePlayer>, IGamePlayer> GetFirstPlayer { get; }
    }
}