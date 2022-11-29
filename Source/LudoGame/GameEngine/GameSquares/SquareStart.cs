namespace LudoGame.GameEngine.GameSquares
{
    public class SquareStart : GameSquare
    {
        public SquareStart(int boardX, int boardY, GameEnum.TeamColor? color, GameEnum.BoardDirection direction)
        {
            BoardX = boardX;
            BoardY = boardY;
            DefaultDirection = direction;
            Color = color;
        }
    }
}