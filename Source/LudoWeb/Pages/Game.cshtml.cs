using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LudoGame.GameEngine;
using LudoGame.GameEngine.GameSquares;
using LudoGame.GameEngine.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LudoWeb.Pages
{
    public class Board : PageModel
    {
        private List<GameSquare> _gameSquares;
        public int XCount { get; }
        public int YCount { get; }
        public GameSquareViewModel GetGameSquare(int x, int y)
        {
            var square = _gameSquares.Find(s => s.BoardX == x && s.BoardY == y);
            if (square == null) return null;
            else
            {
                return new GameSquareViewModel(square);
            }
        }
        private GameSquare GetBase(GameEnum.TeamColor color) => _gameSquares.Find(s => s.GetType() == typeof(BaseSquare) && s.Color == color);
        private GameSquare CopyTo(GameSquare square, int x, int y)
        {
            var type = square.GetType();
            var newSquare = (GameSquare)Activator.CreateInstance(type);
            newSquare.BoardX = x;
            newSquare.BoardY = y;
            newSquare.Color = square.Color;
            return newSquare;
        }
        private void PasteFourSquares(GameSquare original, int upperLeftX, int upperLeftY)
        {
            var coords = new List<(int X, int Y)>
            {
                (upperLeftX, upperLeftY),
                (upperLeftX + 1, upperLeftY),
                (upperLeftX, upperLeftY + 1),
                (upperLeftX + 1, upperLeftY + 1)
            };
            foreach (var coord in coords)
            {
                _gameSquares.Add(CopyTo(original, coord.X, coord.Y));
            }
            _gameSquares.Remove(original);
        }
        public Board(IBoardOrm boardOrm)
        {
            _gameSquares = boardOrm.Map(@"Pages\Board\BoardMap.txt");
            XCount = _gameSquares.Select(s => s.BoardX).Max() + 1;
            YCount = _gameSquares.Select(s => s.BoardY).Max() + 1;
            var win = _gameSquares.Find(s => s.GetType() == typeof(WinnerSquare));
            _gameSquares.Remove(win);
            var blueBase = GetBase(GameEnum.TeamColor.Blue);
            PasteFourSquares(blueBase, 0, 0);
            var redBase = GetBase(GameEnum.TeamColor.Red);
            PasteFourSquares(redBase, 0, YCount-2);
            var greenBase = GetBase(GameEnum.TeamColor.Green);
            PasteFourSquares(greenBase, XCount - 2, YCount - 2);
            var yellowBase = GetBase(GameEnum.TeamColor.Yellow);
            PasteFourSquares(yellowBase, XCount - 2, 0);
        }
        public void OnGet()
        {
        }
    }
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
            Id = X + "," + Y;
        }
    }
}
