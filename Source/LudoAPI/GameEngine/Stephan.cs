using System;
using System.Collections.Generic;
using LudoAPI.GameEngine.GameSquares;
using LudoAPI.GameEngine.Interfaces;
using LudoAPI.Models;
using ModelEnum = LudoAPI.DataAccess.ModelEnum;

namespace LudoAPI.GameEngine
{
    public record Stephan : Player
    {
        public static event Action<Stephan, int> StephanThrowEvent;
        private Action<string> WriteLogging { get; set; }
        private string LoggerMessage { get; set; } = "";

        private IBoardCollection _boardCollection;

        public Stephan(ModelEnum.TeamColor color, ILog log = null)
        {
            Color = color;

            if (log != null)
                WriteLogging = log.Log;
            else
                WriteLogging = x => x = "";

            LoggerMessage = $"{DateTime.Now.ToShortTimeString()}: Initializing Stephan. Color: {Color}";
            WriteLogging(LoggerMessage);
            LoggerMessage = "";
        }

        public override Pawn[] ChoosePlay(PlayerOption playerOption)
        {
            int diceRoll = playerOption.DiceRoll;
            LoggerMessage = $"\n\n[Method: ChoosePlay] New instance\n\n{DateTime.Now.ToShortTimeString()}: [Method: ChoosePlay] Rolled: {diceRoll}";
            LoggerMessage += $"\n{DateTime.Now.ToShortTimeString()}: [Method: ChoosePlay] Calculating play...";
            StephanThrowEvent?.Invoke(this, diceRoll);
            var result = CalculatePlay(playerOption);
            if (result.takeout)
            {
                if (result.takeoutCount == 2)
                {
                    LoggerMessage += $"\n{DateTime.Now.ToShortTimeString()}: [Method: ChoosePlay] Calculations finished. Committing move";
                    WriteLogging(LoggerMessage);
                    var basePawns = _boardCollection.PawnsInBase(Color);
                    return new Pawn[]{basePawns[0], basePawns[1]};
                }

                if (result.takeoutCount == 1)
                {


                    LoggerMessage += $"\n{DateTime.Now.ToShortTimeString()}: [Method: ChoosePlay] Calculations finished. Committing move";
                    WriteLogging(LoggerMessage);
                    return new[] {_boardCollection.PawnsInBase(Color)[0]};
                }
            }
            LoggerMessage += $"\n{DateTime.Now.ToShortTimeString()}: [Method: ChoosePlay] Calculations finished. Committing move";
                WriteLogging(LoggerMessage);
                return new []{result.pawnToMove};
        }
        private (Pawn pawnToMove, bool pass, bool takeout, int takeoutCount) CalculatePlay(PlayerOption playerOption)
        {
            int dice = playerOption.DiceRoll;
            var pawns = _boardCollection.GetFreeTeamPawns(Color);
            LoggerMessage += $"\n{DateTime.Now.ToShortTimeString()}: [Method: CalculatePlay] Checking how many friendly pawns is on board. Result: {pawns.Count.ToString()}";

            //Declaring deconstructed variable for CheckIfPawnCanBeTakenOut which will be called upon later
            (int Count, bool IsPossible) takeOut;
            LoggerMessage += $"\n{DateTime.Now.ToShortTimeString()}: [Method: CalculatePlay] Count is equal to {pawns.Count}";
            if (pawns.Count > 0)
            {

                //Check if I can reach goal
                LoggerMessage += $"\n{DateTime.Now.ToShortTimeString()}: [Method: CalculatePlay] Checking to see if I can reach goal with a pawn";
                var checkPawn = CheckIfPawnCanReachGoal(pawns, dice);
                if (checkPawn != null) return (checkPawn, false, false, 0);
                LoggerMessage += $"\n{DateTime.Now.ToShortTimeString()}: [Method: CheckIfPawnCanReachGoal] returned a null pawn. Continuing";


                //Check if I can reach safezone
                LoggerMessage += $"\n{DateTime.Now.ToShortTimeString()}: [Method: CalculatePlay] Checking to see if I can reach Safezone with a pawn";
                checkPawn = CheckIfPawnCanReachSafezone(pawns, dice);
                if (checkPawn != null) return (checkPawn, false, false, 0);
                LoggerMessage += $"\n{DateTime.Now.ToShortTimeString()}: [Method: CheckIfPawnCanReachSafezone] returned a null pawn. Continuing";


                //Check for possible eradication
                LoggerMessage += $"\n{DateTime.Now.ToShortTimeString()}: [Method: CalculatePlay] Checking if a pawn has the possibility to eradicate another pawn";
                var pawnToEradicateWith = CheckForPossibleEradication(dice).PawnToEradicateWith;
                if (pawnToEradicateWith != null)
                    return (pawnToEradicateWith, false, false, 0); //Returning eradication move. Method will break here
                LoggerMessage += $"\n{DateTime.Now.ToShortTimeString()}: [Method: CalculatePlay] CheckForPossibleEradication returned a null pawn. Continuing";


                //Check if pawn(s) can be taken out
                LoggerMessage += $"\n{DateTime.Now.ToShortTimeString()}: [Method: CalculatePlay] Checking if pawn can be taken out";
                takeOut = CheckIfPawnCanBeTakenOut(dice);
                if (takeOut.IsPossible)
                {
                    LoggerMessage += $"\n{DateTime.Now.ToShortTimeString()}: [Method: CalculatePlay] Can take out {takeOut.Count} pawns. Returning move";
                    return (null, false, true, takeOut.Count); //Taking out pieces. Method will break here
                }


                //No pawns could be taken out
                LoggerMessage += $"\n{DateTime.Now.ToShortTimeString()}: [Method: CalculatePlay] CheckIfPawnCanBeTakenOut returned false. Moving piece instead";
                return (ReturnWhatPieceToMove(pawns, dice), false, false, 0); //Returning piece from Method which calculates best piece to move.
            }



            //Check if pawn(s) can be taken out
            LoggerMessage += $"\n{DateTime.Now.ToShortTimeString()}: [Method: CalculatePlay] Checking if pawn can be taken out";
            takeOut = CheckIfPawnCanBeTakenOut(dice);
            if (takeOut.IsPossible)
            {
                LoggerMessage += $"\n{DateTime.Now.ToShortTimeString()}: [Method: CalculatePlay] Can take out {takeOut.Count} pawns. Returning move";
                return (null, false, true, takeOut.Count); //Taking out pieces. Method will break here
            }
            LoggerMessage += $"\n{DateTime.Now.ToShortTimeString()}: [Method: CalculatePlay] CheckIfPawnCanBeTakenOut returned false. Cannot play this turn";
            return (null, true, false, 0); //Pass turn

        }

        #region "Return" Methods
        private Pawn ReturnWhatPieceToMove(List<Pawn> piecesOut, int dice)
        {
            //Making sure piecesOut is not null
            if (piecesOut == null) throw new ArgumentNullException(nameof(piecesOut));


            //Check if pawn has a possibility to end up in goal
            LoggerMessage += $"\n{DateTime.Now.ToShortTimeString()}: [Method: ReturnWhatPieceToMove] Checking if a pawn has the possibility to end up in goal";
            var checkPawn = CheckIfPawnCanReachGoal(piecesOut, dice);
            if (checkPawn != null) return checkPawn;
            LoggerMessage += $"\n{DateTime.Now.ToShortTimeString()}: [Method: ReturnWhatPieceToMove] CheckIfPawnCanReachGoal returned null. Continuing";


            //Check if pawn has a possibility to end up in Safezone
            LoggerMessage += $"\n{DateTime.Now.ToShortTimeString()}: [Method: ReturnWhatPieceToMove] Checking if a pawn has the possibility to end up in safezone";
            checkPawn = CheckIfPawnCanReachSafezone(piecesOut, dice);
            if (checkPawn != null) return checkPawn;
            LoggerMessage += $"\n{DateTime.Now.ToShortTimeString()}: [Method: ReturnWhatPieceToMove] CheckIfPawnCanReachSafezone returned null. Continuing";


            //Check if a pawn in spawn is blocking
            LoggerMessage += $"\n{DateTime.Now.ToShortTimeString()}: [Method: ReturnWhatPieceToMove] Checking if spawn point is blocked";
            checkPawn = CheckIfPawnIsBlockingSpawn();
            if (checkPawn != null) return checkPawn;
            LoggerMessage += $"\n{DateTime.Now.ToShortTimeString()}: [Method: ReturnWhatPieceToMove] CheckIfPawnIsBlockingSpawn returned null. Continuing";


            //Check if any pawn has a possibility to end up in enemy spawn
            LoggerMessage += $"\n{DateTime.Now.ToShortTimeString()}: [Method: ReturnWhatPieceToMove] Checking if a pawn has the possibility to end up in enemy start square";
            var (pawnEndUpInEnemySpawn, pawnsNotToMove) = CheckIfPawnWillEndUpInEnemySpawn(dice);
            LoggerMessage += $"\n{DateTime.Now.ToShortTimeString()}: [Method: ReturnWhatPieceToMove] Result: {pawnEndUpInEnemySpawn}";


            //Check distance between pawns
            checkPawn = CheckIfPawnDistanceIsTooGreat();
            if (checkPawn != null && !pawnsNotToMove.Contains(checkPawn)) return checkPawn;
            LoggerMessage += $"\n{DateTime.Now.ToShortTimeString()}: [Method: ReturnWhatPieceToMove] CheckIfPawnDistanceIsTooGreat returned null or the farthest pawn will end up in enemy start square.\n{DateTime.Now.ToShortTimeString()}: [Method: ReturnWhatPieceToMove] Will move the farthest pawn.";
            return _boardCollection.FurthestPawn(Color);
        }
        #endregion

        #region Checking methods
        private (bool result, List<Pawn> pawnsToNotMove) CheckIfPawnWillEndUpInEnemySpawn(int dice)
        {
            var pawns = _boardCollection.GetFreeTeamPawns(Color);
            LoggerMessage += $"\n{DateTime.Now.ToShortTimeString()}: [Method: CheckIfPawnWillEndUpInEnemySpawn] Doing calculations to see what friendly pawns can end up in enemy spawn square";
            var result = false;
            var pawnsNotToMove = new List<Pawn>();
            var squarePosition = _boardCollection.StartSquare(Color); //Start at spawn
            foreach (var pawn in pawns)
            {
                squarePosition = _boardCollection.CurrentSquare(pawn);
                for (var i = 0; i <= dice - 1; i++)
                {
                    squarePosition = _boardCollection.GetNext(squarePosition, Color);
                }
                if (squarePosition is StartSquare && squarePosition.Color != Color)
                {
                    result = true;
                    pawnsNotToMove.Add(pawn);
                }
            }
            return (result, pawnsNotToMove);
        }
        private (int Count, bool IsPossible) CheckIfPawnCanBeTakenOut(int dice)
        {
            var pawns = _boardCollection.GetFreeTeamPawns(Color);
            if (dice == 6)
            {
                LoggerMessage += $"\n{DateTime.Now.ToShortTimeString()}: [Method: CheckIfPawnCanBeTakenOut] Dice resulted in a 6\n{DateTime.Now.ToShortTimeString()}: [Method: CheckIfPawnCanBeTakenOut] Checking how many friendly pawns is on board. Result: {pawns.Count}";
                if (pawns.Count < 3)
                {
                    LoggerMessage += $"\n{DateTime.Now.ToShortTimeString()}: [Method: CheckIfPawnCanBeTakenOut] Count is less than three. Will attempt to return double pawns";
                    if (CheckIfPawnIsBlockingSpawn() == null) return (2, true);
                    LoggerMessage += $"\n{DateTime.Now.ToShortTimeString()}: [Method: CheckIfPawnCanBeTakenOut] Spawn is blocked. Continuing.";
                }
                if (pawns.Count == 3)
                {
                    LoggerMessage += $"\n{DateTime.Now.ToShortTimeString()}: [Method: CheckIfPawnCanBeTakenOut] Count is equal to three. Will attempt to return single takeout";
                    if (CheckIfPawnIsBlockingSpawn() == null) return (1, true);
                    LoggerMessage += $"\n{DateTime.Now.ToShortTimeString()}: [Method: CheckIfPawnCanBeTakenOut] Spawn is blocked. Continuing.";

                }
                LoggerMessage += $"\n{DateTime.Now.ToShortTimeString()}: [Method: CheckIfPawnCanBeTakenOut] All pawns is on board. Will return zero";
                return (0, false);
            }
            if (dice == 1)
            {
                LoggerMessage += $"\n{DateTime.Now.ToShortTimeString()}: [Method: CheckIfPawnCanBeTakenOut] Dice resulted in a 1\n{DateTime.Now.ToShortTimeString()}: [Method: CheckIfPawnCanBeTakenOut] Checking how many friendly pawns is on board. Result: {pawns.Count}";
                if (pawns.Count < 4)
                {
                    LoggerMessage += $"\n{DateTime.Now.ToShortTimeString()}: [Method: CheckIfPawnCanBeTakenOut] Count is less than four. Will attempt to return single takeout";
                    if (CheckIfPawnIsBlockingSpawn() == null) return (1, true);
                    LoggerMessage += $"\n{DateTime.Now.ToShortTimeString()}: [Method: CheckIfPawnCanBeTakenOut] Spawn is blocked. Continuing.";
                }
                LoggerMessage += $"\n{DateTime.Now.ToShortTimeString()}: [Method: CheckIfPawnCanBeTakenOut] All pawns is on board. Will return zero";
                return (0, false);
            }
            LoggerMessage +=
                    $"\n{DateTime.Now.ToShortTimeString()}: [Method: CheckIfPawnCanBeTakenOut] Dice resulted in a 1\n{DateTime.Now.ToShortTimeString()}: [Method: CheckIfPawnCanBeTakenOut] Not eligible to pull out pawn. Will return zero.";
            return (0, false);
        }
        private Pawn CheckIfPawnCanReachSafezone(IEnumerable<Pawn> piecesOut, int dice)
        {
            LoggerMessage += $"\n{DateTime.Now.ToShortTimeString()}: [Method: CheckIfPawnCanReachSafezone] Looping through each friendly pawn on board";
            foreach (var piece in piecesOut)
            {
                var squarePosition = _boardCollection.CurrentSquare(piece);
                var squarePositionCalc = squarePosition;
                for (var i = 0; i <= dice; i++)
                {
                    squarePositionCalc = _boardCollection.GetNext(squarePositionCalc, Color);
                    if (squarePositionCalc.GetType() != typeof(SafezoneSquare)) continue;
                    LoggerMessage += $"\n{DateTime.Now.ToShortTimeString()}: [Method: CheckIfPawnCanReachSafezone] Can reach a Safezone-square. Returning move";
                    if (squarePosition != null) return _boardCollection.PawnsOnSquare(squarePosition)[0];
                }
            }
            return null;
        }
        private Pawn CheckIfPawnCanReachGoal(IEnumerable<Pawn> piecesOut, int dice)
        {
            LoggerMessage += $"\n{DateTime.Now.ToShortTimeString()}: [Method: CheckIfPawnCanReachGoal] Looping through each friendly pawn on board";
            foreach (var piece in piecesOut)
            {
                var squarePosition = _boardCollection.CurrentSquare(piece);
                var squarePositionCalc = squarePosition;
                for (var i = 0; i <= dice; i++)
                {
                    squarePositionCalc = _boardCollection.GetNext(squarePositionCalc, Color);
                    if (squarePositionCalc.GetType() != typeof(GoalSquare)) continue;
                    LoggerMessage += $"\n{DateTime.Now.ToShortTimeString()}: [Method: CheckIfPawnCanReachGoal] Can reach a Goal-square. Returning move";
                    if (squarePosition != null) return _boardCollection.PawnsOnSquare(squarePosition)[0];
                }
            }
            return null;
        }
        private Pawn CheckIfPawnIsBlockingSpawn()
        {
            LoggerMessage += $"\n{DateTime.Now.ToShortTimeString()}: [Method: CheckIfPawnIsBlockingSpawn] Checking if there is pawns in base";
            if (_boardCollection.PawnsOnSquare(_boardCollection.BaseSquare(Color)).Count > 0)
            {
                LoggerMessage +=
                    $"\n{DateTime.Now.ToShortTimeString()}: [Method: CheckIfPawnIsBlockingSpawn] No pawns where found in base, returning null";
                return null;
            }

            LoggerMessage += $"\n{DateTime.Now.ToShortTimeString()}: [Method: CheckIfPawnIsBlockingSpawn] Checking if a friendly pawn is blocking Spawn-point";
            var pawnInTakeOut = _boardCollection.PawnsOnSquare(_boardCollection.StartSquare(Color));
            return pawnInTakeOut[0];
        }
        private Pawn CheckIfPawnDistanceIsTooGreat()
        {
            LoggerMessage += $"\n{DateTime.Now.ToShortTimeString()}: [Method: CheckIfPawnDistanceIsTooGreat] Calculating distance between min and max pawn";
            var distance = 0;
            Pawn closestPawn = null;
            var farthestPawn = _boardCollection.FurthestPawn(Color);
            var squarePosition = _boardCollection.StartSquare(Color);
            var furthestIndex = 0;
            for (var i = 0; i <= _boardCollection.TeamPath(Color).Count; i++)
            {
                squarePosition = _boardCollection.GetNext(squarePosition, Color);
                if (squarePosition == _boardCollection.CurrentSquare(farthestPawn))
                {
                    furthestIndex = i;
                }
            }
            squarePosition = _boardCollection.StartSquare(Color); //Start at spawn
            for (var i = 0; i <= furthestIndex; i++)
            {
                squarePosition = _boardCollection.GetNext(squarePosition, Color);
                if (closestPawn != null)
                {
                    distance++;
                }
                if (_boardCollection.IsOccupiedByTeam(Color, squarePosition) && closestPawn == null)
                {
                    closestPawn = _boardCollection.PawnsOnSquare(squarePosition)[0];
                }
            }
            LoggerMessage += $"\n{DateTime.Now.ToShortTimeString()}: [Method: CheckIfPawnDistanceIsTooGreat] Distance is: {distance.ToString()} squares";
            if (distance < 10) return null;
            LoggerMessage += $"\n{DateTime.Now.ToShortTimeString()}: [Method: CheckIfPawnDistanceIsTooGreat] Result is 10 or above. Returning pawn";
            return closestPawn;
        }
        private (bool CanEradicate, Pawn PawnToEradicateWith) CheckForPossibleEradication(int dice)
        {
            var pawns = _boardCollection.GetFreeTeamPawns(Color);
            LoggerMessage += $"\n{DateTime.Now.ToShortTimeString()}: [Method: CheckForPossibleEradication] Calculating possible eradication";
            var eradication = false;
            Pawn eradicationPawn = null;
            foreach (var pawn in pawns)
            {
                var squarePosition = _boardCollection.CurrentSquare(pawn);
                for (var i = 0; i <= dice - 1; i++)
                {
                    squarePosition = _boardCollection.GetNext(squarePosition, Color);
                }
                
                if (_boardCollection.IsOccupiedByTeam(Color, squarePosition)) continue;
                eradication = true;
                eradicationPawn = pawn;
            }
            LoggerMessage += $"\n{DateTime.Now.ToShortTimeString()}: [Method: CheckForPossibleEradication] Eradication status: {eradication}";
            if (eradication)
            {
                LoggerMessage += $"\n{DateTime.Now.ToShortTimeString()}: [Method: CheckForPossibleEradication] Can eradicate. Checking if eradication will result in enemy spawn position";
                var pawnsToNotMove = CheckIfPawnWillEndUpInEnemySpawn(dice).pawnsToNotMove;
                if (!pawnsToNotMove.Contains(eradicationPawn) && eradicationPawn != null)
                {
                    LoggerMessage += $"\n{DateTime.Now.ToShortTimeString()}: [Method: CheckForPossibleEradication] Eradication will not result in enemy spawn position. Returning move";
                    return (eradication, eradicationPawn);
                }
                LoggerMessage += $"\n{DateTime.Now.ToShortTimeString()}: [Method: CheckForPossibleEradication] Eradication will result in enemy spawn position. Returning Pawn as null";
                return (true, null);
            }
            LoggerMessage += $"\n{DateTime.Now.ToShortTimeString()}: [Method: CheckForPossibleEradication] No eradication possible. Returning Pawn as null";
            return (true, null);
        }
        #endregion


    }
}