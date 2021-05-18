using System.Collections.Generic;
using System.Linq;
using LudoGame.GameEngine.Interfaces;

namespace LudoGame.GameEngine.Classes
{
    public class GamePlay : IGamePlay
    {
        private readonly IOptionsValidator _validator;
        private readonly IGameAction _action;
        private readonly IDice _dice;
        private PlayerRotator _playerRotator;
        private List<IGamePlayer> _players;
        public GamePlay(IOptionsValidator validator, IGameAction action, IDice dice)
        {
            _validator = validator;
            _action = action;
            _dice = dice;
        }
        public void SetUpTeams(IGameInfo gameInfo)
        {
            _players = gameInfo.Players;
            _playerRotator = new PlayerRotator(_players);
            var pawns = _players.SelectMany(p => p.Pawns).ToList();
            _action.SetUpPawns(pawns);
        }

        public List<IGamePlayer> GetPlayerSetUps()
        {
            return _players;
        }
        public void Start()
        {
            while (true)
            {
                int diceRoll = _dice.Roll();
                var activePlayer = _playerRotator.CurrentPlayer();
                var playerOption = _validator.GetPlayerOption(activePlayer.Color, diceRoll);
                var pawns = activePlayer.ChoosePlay(playerOption);
                bool valid = _validator.ValidateResponse(playerOption, pawns);
                if (valid) _action.Act(pawns, diceRoll); //else handle exception
                _playerRotator.NextPlayer();
            }
           
        }
    }
}
