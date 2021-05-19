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
        public GamePlay(IOptionsValidator validator, IGameAction action, IFunctionRegistrar registrar, IGameEvent gameEvent)
        {
            _validator = validator;
            _action = action;
            _gameEvent = gameEvent;
            registrar.RegFirstPlayer(_getFirstPlayer);
            registrar.RegRollDice(_rollDice);
        }

        private Func<List<IGamePlayer>, IGamePlayer> _getFirstPlayer;
        private Func<int> _rollDice;
        public void SetUpTeams(IGameInfo gameInfo)
        {
            var players = gameInfo.Players;
            _orderedPlayers = new OrderedPlayerCollection(players, _getFirstPlayer);
            var pawns = _orderedPlayers.SelectMany(p => p.Pawns).ToList();
            _action.SetUpPawns(pawns);
        }

        public List<IGamePlayer> GetPlayers()
        {
            return _orderedPlayers.Select(p => p).ToList();
        }
        public void Start()
        {
            while (true)
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
                _gameEvent.InvokeOnRoundCompletedEvent();
            }
        }
    }
}
