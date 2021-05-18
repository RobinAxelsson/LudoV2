namespace LudoGame.GameEngine.GameSquares
{
    public class SafezoneSquare : GameSquare
    {
        public SafezoneSquare(int boardX, int boardY, GameEnum.TeamColor? color, GameEnum.BoardDirection direction)
        {
            BoardX = boardX;
            BoardY = boardY;
            DefaultDirection = direction;
            Color = color;
        }
    }
}