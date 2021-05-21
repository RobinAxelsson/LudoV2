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
        public Func<IGamePlay, bool> ContinuePlay { get; }
        public Func<int, bool> EndGame { get; }
        private int rollDice()
        {
            return 1;
        }

        private bool continuePlay(IGamePlay gamePlay)
        {
            if (gamePlay.RoundCount > 0) return false;
            return true;
        }
    }
}
