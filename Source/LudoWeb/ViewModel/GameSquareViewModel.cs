using LudoGame.GameEngine;
using LudoGame.GameEngine.GameSquares;

namespace LudoWeb.ViewModel
{
    public class GameSquareViewModel
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Id { get; set; }
        public string Color { get; set; }
        public GameSquareViewModel(GameSquare gameSquare, int xCount, int yCount)
        {
            Color = gameSquare.Color.ToString().ToLower();
            //if (gameSquare.GetType() == typeof(BaseSquare))
            //{
            //    int baseX = 0;
            //    int baseY = 0;
            //    if (gameSquare.color == GameEnum.TeamColor.Blue)
            //    {
            //        baseX = 0;
            //        baseY = 0;
            //    }

            //    if (gameSquare.color == GameEnum.TeamColor.Red)
            //    {
            //        baseX = xCount - 1;
            //        baseY = 0;
            //    }

            //    if (gameSquare.color == GameEnum.TeamColor.Yellow)
            //    {
            //        baseX = 0;
            //        baseY = yCount - 1;
            //    }

            //    if (gameSquare.color == GameEnum.TeamColor.Green)
            //    {
            //        baseX = xCount - 1;
            //        baseY = yCount - 1;
            //    }
            //    y = baseY;
            //    x = baseX;
            //}
            //else
            //{
                Y = gameSquare.BoardY;
                X = gameSquare.BoardX;
            //}
            
            Id = $"x{gameSquare.BoardX}y{gameSquare.BoardY}";
        }
    }
}