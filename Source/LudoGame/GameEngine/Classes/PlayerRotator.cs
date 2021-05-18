using System;
using System.Collections.Generic;
using System.Linq;
using LudoGame.GameEngine.Interfaces;

namespace LudoGame.GameEngine.Classes
{
    public class PlayerRotator
    {
        public PlayerRotator(List<IGamePlayer> players)
        {
            _players = players;
            _colorOrder = new ColorOrder();
            var next = players.Where(p => p.NextToThrow == true);

            if (next.Any() == false || next.Count() > 1)
            {
                var startIndex = new Random().Next(100, 499) / 100;
                players.ForEach(p => p.NextToThrow = false);
                players[startIndex].NextToThrow = true;
                _colorIndex = _colorOrder.ColorIndex(players[startIndex].Color);
            }
            else
            {
                var startPlayer = _players.Find(p => p.NextToThrow == true) ?? throw new NullReferenceException();
                _colorIndex = _colorOrder.ColorIndex(startPlayer.Color);
            }
        }

        public IGamePlayer CurrentPlayer()
        {
            var color = _colorOrder[_colorIndex];
            return _players.Find(p => p.Color == color);
        }
        public void NextPlayer()
        {
            _colorIndex++;
        }
        private ColorOrder _colorOrder;
        private int _colorIndex;
        private List<IGamePlayer> _players;
        private class ColorOrder
        {
            private List<GameEnum.TeamColor> _orderOfTeams = new List<GameEnum.TeamColor>()
            {
                GameEnum.TeamColor.Red,
                GameEnum.TeamColor.Green,
                GameEnum.TeamColor.Yellow,
                GameEnum.TeamColor.Blue
            };

            public GameEnum.TeamColor this[int idx]
            {
                get
                {
                    var top = idx % (_orderOfTeams.Count - 1);
                    if (top != 0) idx = top;
                    return _orderOfTeams[idx];
                }
            }
            public int ColorIndex(GameEnum.TeamColor color)
            {
                return _orderOfTeams.FindIndex(c => c == color);
            }
        }
    }
    
}
