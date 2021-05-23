using System.Collections.Generic;
using LudoGame.GameEngine.GameSquares;

namespace LudoGame.GameEngine.Interfaces
{
    public interface IBoardOrm
    {
        public List<GameSquare> Map();
        public List<GameSquare> Map(string filePath);
    }
}