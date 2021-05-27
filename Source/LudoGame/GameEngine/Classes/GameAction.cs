using System;
using System.Collections.Generic;
using LudoGame.GameEngine.GameSquares;
using LudoGame.GameEngine.Interfaces;
using static LudoGame.GameEngine.GameEnum;

namespace LudoGame.GameEngine.Classes
{
    public class GameAction : IGameAction
    {
        private readonly IBoardCollection _boardCollection;
        private readonly IGameEvent _gameEvent;
        public GameAction(IBoardCollection collection, IGameEvent gameEvent)
        {
            _boardCollection = collection;
            _gameEvent = gameEvent;
        }
        public void Act(Pawn[] pawns, int diceRoll)
        {
            if (pawns.Length == 0) return;
            if (pawns.Length == 2)
            {
                if (diceRoll != 6) throw new Exception("Pawns can not be two if dice is not 6.");
                foreach (var p in pawns)
                {
                    Move(p, 1);
                }
                return;
            }
            if (pawns.Length == 1)
            {
                Move(pawns[0], diceRoll);
            }
            else
            {
                throw new Exception("Invalid pawn count");
            }
        }
        public void SetUpPawns(List<Pawn> allPawns)
        {
            if (allPawns.Count == 0)
            {
                var colors = Enum.GetValues<TeamColor>();
                int pawnId = 0;
                for (int t = 0; t < 4; t++)
                {
                    var baseSquare = _boardCollection.BaseSquare(colors[t]);

                    for (int p = 0; p < 4; p++)
                    {
                        var pawn = new Pawn(colors[t], pawnId);
                        pawnId++;
                        ChangeCoordinates(pawn, baseSquare);
                        allPawns.Add(pawn);
                    }
                }
                _boardCollection.SetPawns(allPawns);
                return;
            }

            if (allPawns.TrueForAll(p => p.X == 0 && p.Y == 0))
            {
                allPawns.ForEach(p => ChangeCoordinates(
                    p, _boardCollection.BaseSquare(p.Color))
                );
                return;
            }

            _boardCollection.SetPawns(allPawns);
        }
        private void Move(Pawn pawn, int dice)
        {
            var tempSquare = _boardCollection.CurrentSquare(pawn);
            bool startingSquareIsSafeZoneSquare = tempSquare is SafezoneSquare;

            bool bounced = false;

            for (var i = 0; i < dice; i++)
            {
                bool lastIteration = i == dice - 1;

                if (tempSquare is GoalSquare || bounced == true)
                {
                    tempSquare = _boardCollection.PastSquare(tempSquare, pawn.Color);
                    bounced = true;
                }
                else
                {
                    tempSquare = _boardCollection.GetNext(tempSquare, pawn.Color);
                }
                if (lastIteration == true && tempSquare is GoalSquare)
                {
                    ChangeCoordinates(pawn, _boardCollection.GoalSquare());

                    if (_boardCollection.GetTeamPawns(pawn.Color).Count == 0)
                    {
                        _gameEvent.InvokeOnAllTeamPawnsOutEvent(pawn.Color);
                    }
                    else
                    {
                        _gameEvent.InvokeOnGoalEvent(pawn.Color, _boardCollection.GetTeamPawns(pawn.Color).Count);
                    }

                    bool onlyOneTeamLeft = _boardCollection.TeamsLeft().Count == 1;
                    if (onlyOneTeamLeft)
                    {
                        _gameEvent.InvokeOnGameOverEvent();
                    }
                    ChangeCoordinates(pawn, _boardCollection.WinnerSquare());
                    _boardCollection.RemovePawn(pawn);
                    return;
                }
            }

            TeamColor? enemyColor = null;
            int pawnsToEradicate = 0;
            var enemies = _boardCollection.EnemiesOnSquare(tempSquare, pawn.Color);
            if (enemies.Count > 0)
            {
                enemyColor = enemies[0].Color;
                pawnsToEradicate = enemies.Count;
                var eradicateBase = _boardCollection.BaseSquare((TeamColor)enemyColor);
                ChangeCoordinates(enemies, eradicateBase);
            }

            if (pawnsToEradicate != 0) _gameEvent.InvokeOnEradicationEvent(pawn.Color, (TeamColor)enemyColor, pawnsToEradicate);
            ChangeCoordinates(pawn, tempSquare);

            if (bounced == true) _gameEvent.InvokeOnBounceEvent(pawn.Color);
            if (tempSquare is SafezoneSquare && startingSquareIsSafeZoneSquare == false) _gameEvent.InvokeOnSafeZoneEvent(pawn.Color);
            enemies.Add(pawn);
            _gameEvent.InvokeOnMoveEvent(enemies.ToArray()); //enemies plus own.
        }
        private void ChangeCoordinates(List<Pawn> pawns, GameSquare targetSquare)
        {
            foreach (var p in pawns)
            {
                ChangeCoordinates(p, targetSquare);
            }
        }
        private void ChangeCoordinates(Pawn pawn, GameSquare targetSquare)
        {
            if (targetSquare is GoalSquare)
            {
                pawn.X = 0;
                pawn.Y = 0;
                return;
            }
            pawn.X = targetSquare.BoardX;
            pawn.Y = targetSquare.BoardY;
        }
    }
}
