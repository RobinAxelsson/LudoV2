using System;
using LudoGame.GameEngine.Interfaces;

namespace LudoGame.GameEngine.Classes
{
    public class Dice : IDice
    {
        private Random _rand = new();
        public int Roll() => _rand.Next(100, 699) / 100;
    }
}