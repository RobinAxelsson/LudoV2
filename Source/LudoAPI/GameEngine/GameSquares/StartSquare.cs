using LudoAPI.DataAccess;

namespace LudoAPI.GameEngine.GameSquares
{
    public class StartSquare : GameSquare
    {
        public StartSquare(int boardX, int boardY, ModelEnum.TeamColor? color, ModelEnum.BoardDirection direction)
        {
            BoardX = boardX;
            BoardY = boardY;
            DefaultDirection = direction;
            Color = color;
        }
    }
}