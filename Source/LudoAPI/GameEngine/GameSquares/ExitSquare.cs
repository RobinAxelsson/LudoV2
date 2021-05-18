using LudoAPI.DataAccess;

namespace LudoAPI.GameEngine.GameSquares
{
    public class ExitSquare : GameSquare
    {
        public ExitSquare(int boardX, int boardY, ModelEnum.TeamColor? color, ModelEnum.BoardDirection direction)
        {
            BoardX = boardX;
            BoardY = boardY;
            DefaultDirection = direction;
            Color = color;
        }
        public override ModelEnum.BoardDirection DirectionNext(ModelEnum.TeamColor color)
        {
            if (this.Color == color)
            {
                return
                    color == ModelEnum.TeamColor.Yellow ? ModelEnum.BoardDirection.Up :
                    color == ModelEnum.TeamColor.Blue ? ModelEnum.BoardDirection.Right :
                    color == ModelEnum.TeamColor.Red ? ModelEnum.BoardDirection.Down : ModelEnum.BoardDirection.Left;
            }
            else
                return DefaultDirection;
        }
    }
}