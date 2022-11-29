namespace LudoGame.GameEngine.GameSquares
{
    public class SquareTeamBase : GameSquare
    {
        public SquareTeamBase() { }
        public SquareTeamBase(int boardX, int boardY, GameEnum.TeamColor? color, GameEnum.BoardDirection direction)
        {
            BoardX = boardX;
            BoardY = boardY;
            DefaultDirection = direction;
            Color = color;
        }
    }
}