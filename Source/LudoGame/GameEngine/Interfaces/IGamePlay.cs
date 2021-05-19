using System.Collections.Generic;

namespace LudoGame.GameEngine.Interfaces
{
    public interface IGamePlay
    {
        List<IGamePlayer> ReadPlayers();
        void Start(IGameInfo gameInfo);
    }
}