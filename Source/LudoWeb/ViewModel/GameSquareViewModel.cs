using LudoGame.GameEngine.GameSquares;

namespace LudoWeb.ViewModel
{
    public class GameSquareViewModel
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Id { get; set; }
        public string Color { get; set; }
        public GameSquareViewModel(GameSquare gameSquare)
        {
            Color = gameSquare.Color.ToString().ToLower();
            X = gameSquare.BoardX;
            Y = gameSquare.BoardY;
            Id = $"X{X}Y{Y}";
        }
    }
}