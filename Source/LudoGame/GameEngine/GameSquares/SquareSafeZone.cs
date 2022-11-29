namespace LudoGame.GameEngine.GameSquares
{
    public class SquareSafeZone : GameSquare
    {
        public SquareSafeZone(int boardX, int boardY, GameEnum.TeamColor? color, GameEnum.BoardDirection direction)
        {
            BoardX = boardX;
            BoardY = boardY;
            DefaultDirection = direction;
            Color = color;
        }
    }
}