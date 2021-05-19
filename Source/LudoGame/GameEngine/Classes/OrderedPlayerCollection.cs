using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LudoGame.GameEngine.Interfaces;

namespace LudoGame.GameEngine.Classes
{
    public class OrderedPlayerCollection : IEnumerable<IGamePlayer>
    {
        private readonly List<IGamePlayer> _players;
        public OrderedPlayerCollection(List<IGamePlayer> players, Func<List<IGamePlayer>, IGamePlayer> getFirstPlayer)
        {
            if (players.Count() != 4) throw new Exception("Player count is not 4");
            if (players.Select(c => c.Color).Distinct().Count() != 4) throw new Exception("All colors does not exist");
            var firstToThrow = getFirstPlayer(players);
            var newList = new List<IGamePlayer>();

            foreach (var color in _orderOfTeams) //Sorts players according to given color sequence.
            {
                newList.Add(players.Find(p => p.Color == color));
            }

            while (newList[0].Color != firstToThrow.Color) //Puts first player first in rotation.
            {
                var pRotate = newList[0];
                newList.RemoveAt(0);
                newList.Add(pRotate);
            }
            _players = players;
        }
        public IGamePlayer this[int idx] => _players[idx];
        private readonly List<GameEnum.TeamColor> _orderOfTeams = new()
        {
            GameEnum.TeamColor.Red,
            GameEnum.TeamColor.Green,
            GameEnum.TeamColor.Yellow,
            GameEnum.TeamColor.Blue
        };
        public IEnumerator<IGamePlayer> GetEnumerator()
        {
            return new GamePlayerEnumerator(this);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class GamePlayerEnumerator : IEnumerator<IGamePlayer>
    {
        private readonly OrderedPlayerCollection _collection;
        private int _idx;
        public GamePlayerEnumerator(OrderedPlayerCollection collection)
        {
            _collection = collection;
            _idx = -1;
        }
        public bool MoveNext() => ++_idx < _collection.Count();
        public void Reset() => _idx = -1;
        public IGamePlayer Current => _collection[_idx];
        object IEnumerator.Current => Current;
        public void Dispose() { }
    }

}
