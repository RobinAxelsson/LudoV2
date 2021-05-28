using System;
using System.Collections.Generic;
using System.Linq;
using LudoGame.GameEngine.Interfaces;

namespace LudoGame.GameEngine.Classes
{
    public class OptionsValidator : IOptionsValidator
    {
        private readonly IBoardCollection _boardCollection;
        private readonly IEqualityComparer<Pawn> _pawnComparer;
        public OptionsValidator(IBoardCollection collection, IEqualityComparer<Pawn> pawnComparer)
        {
            _boardCollection = collection;
            _pawnComparer = pawnComparer;
        }

        public PlayerOption GetPlayerOption(GameEnum.TeamColor color, int diceRoll)
        {
            var pawns = SelectablePawns(color, diceRoll);
            bool CanTakeOutTwo = this.CanTakeOutTwo(color, diceRoll);
            return new PlayerOption(pawns, CanTakeOutTwo, diceRoll);
        }
        public bool ValidateResponse(PlayerOption option, Pawn[] pawnResponse)
        {
            if (pawnResponse.Length > 2) return false;
            if (option.CanTakeOutTwo == false && pawnResponse.Length == 2) return false;
            if (option.CanTakeOutTwo == true && pawnResponse.Length == 2) return true;
            if(pawnResponse.Length > 0)
            {
                var pawn = pawnResponse[0];
                if (option.PawnsToMove.Contains(pawn, _pawnComparer)) return true;
                if (pawnResponse.Length == 0 && option.PawnsToMove.Length > 0)
                    throw new Exception("Player have to move if they can");
            } 
            return false;
        }
        private Pawn[] SelectablePawns(GameEnum.TeamColor color, int diceRoll)
        {
            var pawnsInBase = _boardCollection.PawnsInBase(color);
            var freeTeamPawns = _boardCollection.GetFreeTeamPawns(color);

            if (diceRoll == 1 || diceRoll == 6)
                return freeTeamPawns.Concat(pawnsInBase).ToArray();
            else
                return freeTeamPawns.ToArray();
        }
        private bool CanTakeOutTwo(GameEnum.TeamColor color, int diceRoll) => _boardCollection.PawnsInBase(color).Count > 1 && diceRoll == 6;
    }
}
