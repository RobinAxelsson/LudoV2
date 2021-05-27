using System;
using System.Collections.Generic;
using System.Linq;
using LudoGame.GameEngine;
using LudoGame.GameEngine.GameSquares;
using LudoGame.GameEngine.Interfaces;
using LudoWeb.GameInterfaces;
using LudoWeb.ViewModel;

namespace LudoWeb.GameClasses
{
    public class HtmlBoardBuilder : IHtmlBoardBuilder
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
            var newSquare = (GameSquare) Activator.CreateInstance(type);
            newSquare.BoardX = x;
            newSquare.BoardY = y;
            newSquare.Color = square.Color;
            return newSquare;
        }
        private void CopyPaste(GameSquare original, int X, int Y)
        {
            _gameSquares.Add(CopyTo(original, X, Y));
            _gameSquares.Remove(original);
        }

        public HtmlBoardBuilder(IBoardOrm boardOrm)
        {
            _gameSquares = boardOrm.Map();
            XCount = _gameSquares.Select(s => s.BoardX).Max() + 1;
            YCount = _gameSquares.Select(s => s.BoardY).Max() + 1;
            var win = _gameSquares.Find(s => s.GetType() == typeof(WinnerSquare));
            _gameSquares.Remove(win);
            var blueBase = GetBase(GameEnum.TeamColor.Blue);
            CopyPaste(blueBase, 0, 0);
            var redBase = GetBase(GameEnum.TeamColor.Red);
            CopyPaste(redBase, 0, YCount - 1);
            var greenBase = GetBase(GameEnum.TeamColor.Green);
            CopyPaste(greenBase, XCount - 1, YCount - 1);
            var yellowBase = GetBase(GameEnum.TeamColor.Yellow);
            CopyPaste(yellowBase, XCount - 1, 0);
        }
    }
}
