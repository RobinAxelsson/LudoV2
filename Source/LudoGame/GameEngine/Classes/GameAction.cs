using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LudoGame.GameEngine.GameSquares;
using LudoGame.GameEngine.Interfaces;
using static LudoGame.GameEngine.GameEnum;

namespace LudoGame.GameEngine.Classes
{
    public class GameAction : IGameAction
    {
        private readonly IBoardCollection _boardCollection;
        private readonly IGameEvent _gameEvent;
        public Func<Pawn[], Task> refreshPawns { get; set; }
        public GameAction(IBoardCollection collection, IGameEvent gameEvent)
        {
            _boardCollection = collection;
            _gameEvent = gameEvent;
        }
        public async void Act(Pawn[] pawns, int diceRoll)
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
            
            await refreshPawns(_boardCollection.GetAllPawns().ToArray());
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

            if (allPawns.TrueForAll(p => p.x == 0 && p.y == 0))
            {
                allPawns.ForEach(p => ChangeCoordinates(
                    p, _boardCollection.BaseSquare(p.color))
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
                    tempSquare = _boardCollection.PastSquare(tempSquare, pawn.color);
                    bounced = true;
                }
                else
                {
                    tempSquare = _boardCollection.GetNext(tempSquare, pawn.color);
                }
                if (lastIteration == true && tempSquare is GoalSquare)
                {
                    ChangeCoordinates(pawn, _boardCollection.GoalSquare());

                    if (_boardCollection.GetTeamPawns(pawn.color).Count == 0)
                    {
                        _gameEvent.InvokeOnAllTeamPawnsOutEvent(pawn.color);
                    }
                    else
                    {
                        _gameEvent.InvokeOnGoalEvent(pawn.color, _boardCollection.GetTeamPawns(pawn.color).Count);
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
            var enemies = _boardCollection.EnemiesOnSquare(tempSquare, pawn.color);
            if (enemies.Count > 0)
            {
                enemyColor = enemies[0].color;
                pawnsToEradicate = enemies.Count;
                var eradicateBase = _boardCollection.BaseSquare((TeamColor)enemyColor);
                ChangeCoordinates(enemies, eradicateBase);
            }

            if (pawnsToEradicate != 0) _gameEvent.InvokeOnEradicationEvent(pawn.color, (TeamColor)enemyColor, pawnsToEradicate);
            ChangeCoordinates(pawn, tempSquare);

            if (bounced == true) _gameEvent.InvokeOnBounceEvent(pawn.color);
            if (tempSquare is SafezoneSquare && startingSquareIsSafeZoneSquare == false) _gameEvent.InvokeOnSafeZoneEvent(pawn.color);
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
                pawn.x = 0;
                pawn.y = 0;
                return;
            }
            pawn.x = targetSquare.BoardX;
            pawn.y = targetSquare.BoardY;
        }
    }
}
