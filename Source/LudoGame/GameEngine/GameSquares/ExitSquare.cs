namespace LudoGame.GameEngine.GameSquares
{
    public class ExitSquare : GameSquare
    {
        public ExitSquare(int boardX, int boardY, GameEnum.TeamColor? color, GameEnum.BoardDirection direction)
        {
            BoardX = boardX;
            BoardY = boardY;
            DefaultDirection = direction;
            Color = color;
        }
        public override GameEnum.BoardDirection DirectionNext(GameEnum.TeamColor color)
        {
            if (this.Color == color)
            {
                return
                    color == GameEnum.TeamColor.Yellow ? GameEnum.BoardDirection.Up :
                    color == GameEnum.TeamColor.Blue ? GameEnum.BoardDirection.Right :
                    color == GameEnum.TeamColor.Red ? GameEnum.BoardDirection.Down : GameEnum.BoardDirection.Left;
            }
            else
                return DefaultDirection;
        }
    }
}