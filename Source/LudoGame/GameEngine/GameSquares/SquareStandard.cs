namespace LudoGame.GameEngine.GameSquares
{
    public class SquareStandard : GameSquare
    {
        public SquareStandard(int boardX, int boardY, GameEnum.BoardDirection direction)
        {
            BoardX = boardX;
            BoardY = boardY;
            DefaultDirection = direction;
            Color = null;
        }
    }
}