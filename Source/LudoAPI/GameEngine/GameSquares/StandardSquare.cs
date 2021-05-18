using LudoAPI.DataAccess;

namespace LudoAPI.GameEngine.GameSquares
{
    public class StandardSquare : GameSquare
    {
        public StandardSquare(int boardX, int boardY, ModelEnum.BoardDirection direction)
        {
            BoardX = boardX;
            BoardY = boardY;
            DefaultDirection = direction;
            Color = null;
        }
    }
}