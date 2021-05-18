using System;

namespace LudoAPI.GameEngine.Frontend
{
    public interface IController
    {
        public void Activate();

        public event Action SelectionUpEvent;
        public event Action SelectionDownEvent;
        public event Action TakeOutTwoPressEvent;
        public event Action OnConfirmEvent;
    }
}
