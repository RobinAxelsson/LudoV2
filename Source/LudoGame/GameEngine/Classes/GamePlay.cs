using System;
using System.Collections.Generic;
using System.Linq;
using LudoGame.GameEngine.Interfaces;

namespace LudoGame.GameEngine.Classes
{
    public class GamePlay : IGamePlay
    {
        private readonly IOptionsValidator _validator;
        private readonly IGameAction _action;
        private OrderedPlayerCollection _orderedPlayers;
        private readonly IGameEvent _gameEvent;
        public GamePlay(IOptionsValidator validator, IGameAction action, IGameFunction gameFunctions, IGameEvent gameEvent)
        {
            _validator = validator;
            _action = action;
            _gameEvent = gameEvent;
            _getFirstPlayer = gameFunctions.GetFirstPlayer;
            _rollDice = gameFunctions.RollDice;
            _continuePlay = gameFunctions.ContinuePlay;
        }

        private readonly Func<IGamePlay, bool> _continuePlay;
        private readonly Func<List<IGamePlayer>, IGamePlayer> _getFirstPlayer;
        private readonly Func<int> _rollDice;

        private void SetUpTeams(List<IGamePlayer> players)
        {
            _orderedPlayers = new OrderedPlayerCollection(players, _getFirstPlayer);
            var pawns = _orderedPlayers.SelectMany(p => p.Pawns).ToList();
            _action.SetUpPawns(pawns);
        }
        public GameEnum.GameStatus GameStatus { get; set; }
        public int RoundCount { get; set; }
        public List<IGamePlayer> ReadPlayers()
        {
            return _orderedPlayers.Select(p => p).ToList();
        }
        public void Start(List<IGamePlayer> players)
        {
            SetUpTeams(players);

            //_gameEvent.InvokeOnNewGameEvent();
            while (_continuePlay(this))
            {
                foreach (var player in _orderedPlayers)
                {
                    int diceRoll = _rollDice();
                    var playerOption = _validator.GetPlayerOption(player.Color, diceRoll);
                    var pawns = player.ChoosePlay(playerOption);
                    bool valid = _validator.ValidateResponse(playerOption, pawns);
                    if (valid) _action.Act(pawns, diceRoll);
                    else
                    {
                        player.ChoosePlay(playerOption);
                        _gameEvent.InvokeOnInvalidResponseEvent(player);
                    }
                }

                RoundCount++;
                _gameEvent.InvokeOnRoundCompletedEvent();
            }
        }
    }
}
