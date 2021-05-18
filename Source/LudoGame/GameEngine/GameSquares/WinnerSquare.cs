namespace LudoGame.GameEngine.GameSquares
{
    public class WinnerSquare : GameSquare
    {
        public WinnerSquare(int boardX, int boardY)
        {
            BoardX = boardX;
            BoardY = boardY;
            DefaultDirection = 0;
            Color = null;
        }
    }
}