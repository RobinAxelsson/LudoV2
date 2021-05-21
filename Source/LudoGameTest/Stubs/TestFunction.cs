using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LudoGame.GameEngine.Interfaces;

namespace LudoGameTest.Stubs
{
    public class TestFunction : IGameFunction
    {
        public TestFunction()
        {
            RollDice = rollDice;
        }
        public Func<int> RollDice { get; }
        public Func<List<IGamePlayer>, IGamePlayer> GetFirstPlayer { get; }
        public Func<int, bool> GameKeep { get; }
        private int rollDice()
        {
            return 1;
        }
    }
}
