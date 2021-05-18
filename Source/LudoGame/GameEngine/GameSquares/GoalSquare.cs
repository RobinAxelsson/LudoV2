namespace LudoGame.GameEngine.GameSquares
{
    public class GoalSquare : GameSquare
    {
        public GoalSquare(int boardX, int boardY)
        {
            BoardX = boardX;
            BoardY = boardY;
            DefaultDirection = 0;
            Color = null;
        }
        public override GameEnum.BoardDirection DirectionNext(GameEnum.TeamColor color)
        {
            return
               color == GameEnum.TeamColor.Yellow ? GameEnum.BoardDirection.Up :
               color == GameEnum.TeamColor.Blue ? GameEnum.BoardDirection.Right :
               color == GameEnum.TeamColor.Red ? GameEnum.BoardDirection.Down : GameEnum.BoardDirection.Left;
        }
    }
}