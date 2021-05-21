using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LudoGame.GameEngine.Interfaces;

namespace LudoGame.GameEngine.Classes
{
    public class OrderedPlayerCollection : IEnumerable<IGamePlayer>
    {
        private List<IGamePlayer> _players;
        public OrderedPlayerCollection(List<IGamePlayer> players, Func<List<IGamePlayer>, IGamePlayer> getFirstPlayer)
        {
            if (players.Count() != 4) throw new Exception("Player count is not 4");
            if (players.Select(c => c.Color).Distinct().Count() != 4) throw new Exception("All colors does not exist");
            var firstToThrow = getFirstPlayer(players);
            var orderedPlayers = new List<IGamePlayer>();

            foreach (var color in _orderOfTeams) //Sorts players according to given color sequence.
            {
                orderedPlayers.Add(players.Find(p => p.Color == color));
            }

            while (orderedPlayers[0].Color != firstToThrow.Color) //Puts first player first in rotation.
            {
                var pRotate = orderedPlayers[0];
                orderedPlayers.RemoveAt(0);
                orderedPlayers.Add(pRotate);
            }
            _players = orderedPlayers;
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
            foreach (var player in _players)
            {
                yield return player;
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
