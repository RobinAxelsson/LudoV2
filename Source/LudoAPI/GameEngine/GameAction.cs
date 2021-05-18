using System;
using System.Collections.Generic;
using LudoAPI.GameEngine.GameSquares;
using LudoAPI.GameEngine.Interfaces;
using LudoAPI.Models;
using ModelEnum = LudoAPI.DataAccess.ModelEnum;

namespace LudoAPI.GameEngine
{
    public class GameAction : IGameAction
    {
        private readonly IBoardCollection _boardCollection;

        public GameAction(IBoardCollection collection)
        {
            _boardCollection = collection;
        }
        public event Action<DataAccess.ModelEnum.TeamColor, int> OnMoveEvent;
        public event Action<DataAccess.ModelEnum.TeamColor> OnTakeOutTwoEvent;

        public event Action<Pawn> OnBounceEvent;
        public event Action<Pawn, int> OnGoalEvent;
        public event Action<Pawn> OnAllTeamPawnsOutEvent;
        public event Action<Pawn, ModelEnum.TeamColor, int> OnEradicationEvent;
        public event Action<ModelEnum.TeamColor> GameLoserEvent;
        public event Action GameOverEvent;
        public event Action<Pawn> OnSafeZoneEvent;

        public void Act(Pawn[] pawns, int diceRoll)
        {
            if (pawns.Length == 0) return;
            if (pawns.Length == 2)
            {
                OnTakeOutTwoEvent?.Invoke(pawns[0].Color);
                foreach (var p in pawns)
                {
                    Move(p, 1);
                }
                return;
            }
            if (pawns.Length == 1)
            {
                OnMoveEvent?.Invoke(pawns[0].Color, diceRoll);
                Move(pawns[0], diceRoll);
            }
            throw new Exception("Invalid pawn count");
        }
        private void Move(Pawn pawn, int dice)
        {
            var tempSquare = _boardCollection.CurrentSquare(pawn);
            bool startingSquareIsSafeZoneSquare = tempSquare is SafezoneSquare;

            bool bounced = false;

            for (var i = 0; i < dice; i++)
            {
                var lastIteration = i == dice - 1;

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
                        OnAllTeamPawnsOutEvent?.Invoke(pawn);
                    else
                        OnGoalEvent?.Invoke(pawn, _boardCollection.GetTeamPawns(pawn.Color).Count);

                    bool onlyOneTeamLeft = _boardCollection.TeamsLeft().Count == 1;
                    if (onlyOneTeamLeft)
                    {
                        GameLoserEvent?.Invoke(_boardCollection.TeamsLeft()[0]);
                        GameOverEvent?.Invoke();
                    }
                    return;
                }
            }

            DataAccess.ModelEnum.TeamColor? enemyColor = null;
            int pawnsToEradicate = 0;
            var enemies = _boardCollection.EnemiesOnSquare(tempSquare, pawn.Color);
            if (enemies.Count > 0)
            {
                enemyColor = enemies[0].Color;
                pawnsToEradicate = enemies.Count;
                var eradicateBase = _boardCollection.BaseSquare((ModelEnum.TeamColor)enemyColor);
                ChangeCoordinates(enemies, eradicateBase);
            }

            if (pawnsToEradicate != 0) OnEradicationEvent?.Invoke(pawn, (ModelEnum.TeamColor)enemyColor, pawnsToEradicate);
            ChangeCoordinates(pawn, tempSquare);

            if (bounced == true) OnBounceEvent?.Invoke(pawn);
            if (tempSquare is SafezoneSquare && startingSquareIsSafeZoneSquare == false) OnSafeZoneEvent?.Invoke(pawn);
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
