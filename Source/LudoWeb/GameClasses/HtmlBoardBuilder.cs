using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using LudoGame.GameEngine;
using LudoGame.GameEngine.GameSquares;
using LudoGame.GameEngine.Interfaces;
using LudoWeb.GameInterfaces;
using LudoWeb.ViewModel;

namespace LudoWeb.GameClasses
{
    public class HtmlBoardBuilder : IHtmlBoardBuilder
    {
        public int XCount { get; }
        public int YCount { get; }
        private List<GameSquareViewModel> _squareViews;
        public HtmlBoardBuilder(IBoardOrm boardOrm)
        {
            var gameSquares = boardOrm.Map();
            XCount = gameSquares.Select(s => s.BoardX).Max() + 1;
            YCount = gameSquares.Select(s => s.BoardY).Max() + 1;
            var win = gameSquares.Find(s =>
            s.GetType() == typeof(SquareWinner));
            gameSquares.Remove(win);
            var squareViews = new List<GameSquareViewModel>();
            foreach (var gs in gameSquares)
            {
                squareViews.Add(new GameSquareViewModel(gs, XCount, YCount));
            }
            _squareViews = squareViews;

        }

        public GameSquareViewModel GetGameSquare(int x, int y)
        {
            return _squareViews.Find(s => s.X == x && s.Y == y);
        }
    }
}
