using System.Collections.Generic;

namespace LudoGame.GameEngine.Interfaces
{
    public interface IGamePlay
    {
        void SetUpTeams(IGameInfo gameInfo);
        List<IGamePlayer> GetPlayerSetUps();
        void Start();
    }
}