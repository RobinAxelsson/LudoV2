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
            //    if (gameSquare.Color == GameEnum.TeamColor.Blue)
            //    {
            //        baseX = 0;
            //        baseY = 0;
            //    }

            //    if (gameSquare.Color == GameEnum.TeamColor.Red)
            //    {
            //        baseX = xCount - 1;
            //        baseY = 0;
            //    }

            //    if (gameSquare.Color == GameEnum.TeamColor.Yellow)
            //    {
            //        baseX = 0;
            //        baseY = yCount - 1;
            //    }

            //    if (gameSquare.Color == GameEnum.TeamColor.Green)
            //    {
            //        baseX = xCount - 1;
            //        baseY = yCount - 1;
            //    }
            //    Y = baseY;
            //    X = baseX;
            //}
            //else
            //{
                Y = gameSquare.BoardY;
                X = gameSquare.BoardX;
            //}
            
            Id = $"X{gameSquare.BoardX}Y{gameSquare.BoardY}";
        }
    }
}