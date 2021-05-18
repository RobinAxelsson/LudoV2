namespace LudoGame.GameEngine.GameSquares
{
    public class BaseSquare : GameSquare
    {
        public BaseSquare(int boardX, int boardY, GameEnum.TeamColor? color, GameEnum.BoardDirection direction)
        {
            BoardX = boardX;
            BoardY = boardY;
            DefaultDirection = direction;
            Color = color;
        }
    }
}