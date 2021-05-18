namespace LudoGame.GameEngine.GameSquares
{
    public class StandardSquare : GameSquare
    {
        public StandardSquare(int boardX, int boardY, GameEnum.BoardDirection direction)
        {
            BoardX = boardX;
            BoardY = boardY;
            DefaultDirection = direction;
            Color = null;
        }
    }
}