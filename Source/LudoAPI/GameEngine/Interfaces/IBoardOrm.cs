using System.Collections.Generic;
using LudoAPI.GameEngine.GameSquares;

namespace LudoAPI.GameEngine.Interfaces
{
    public interface IBoardOrm
    {
        public List<GameSquare> Map();
    }
}