namespace LudoGame.GameEngine.GameSquares
{
    public class SquareWinner : GameSquare
    {
        public SquareWinner(int boardX, int boardY)
        {
            BoardX = boardX;
            BoardY = boardY;
            DefaultDirection = 0;
            Color = null;
        }
    }
}