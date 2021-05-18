namespace LudoGame.GameEngine.GameSquares
{
    public class StartSquare : GameSquare
    {
        public StartSquare(int boardX, int boardY, GameEnum.TeamColor? color, GameEnum.BoardDirection direction)
        {
            BoardX = boardX;
            BoardY = boardY;
            DefaultDirection = direction;
            Color = color;
        }
    }
}