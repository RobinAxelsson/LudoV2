using LudoAPI.DataAccess;

namespace LudoAPI.GameEngine.GameSquares
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
        public override ModelEnum.BoardDirection DirectionNext(ModelEnum.TeamColor color)
        {
            return
               color == ModelEnum.TeamColor.Yellow ? ModelEnum.BoardDirection.Up :
               color == ModelEnum.TeamColor.Blue ? ModelEnum.BoardDirection.Right :
               color == ModelEnum.TeamColor.Red ? ModelEnum.BoardDirection.Down : ModelEnum.BoardDirection.Left;
        }
    }
}