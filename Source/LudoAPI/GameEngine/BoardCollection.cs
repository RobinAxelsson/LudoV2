using System;
using System.Collections.Generic;
using System.Linq;
using LudoAPI.GameEngine.GameSquares;
using LudoAPI.GameEngine.Interfaces;
using LudoAPI.Models;
using ModelEnum = LudoAPI.DataAccess.ModelEnum;

namespace LudoAPI.GameEngine
{
    public class BoardCollection : IBoardCollection
    {
        private List<GameSquare> _boardSquares { get; }
        public BoardCollection(IBoardOrm boardOrm)
        {
            _boardSquares = boardOrm.Map();
        }
        private List<Pawn> _allPawn
        {
            get { return _allPawn.FindAll(p => p.X != 0 && p.Y != 0); }
            set => _allPawn = value;
        }
        public void SetAllPawn(List<Pawn> pawns)
        {
            if (_allPawn != null) _allPawn = pawns;
        }
        public List<ModelEnum.TeamColor> TeamsLeft() => _allPawn.Select(p => p.Color).ToList();
        public List<Pawn> GetTeamPawns(ModelEnum.TeamColor color) => _allPawn.FindAll(p => p.Color == color);
        public Pawn FurthestPawn(ModelEnum.TeamColor color)
        {
            var path = TeamPath(color);
            var teamPawns = GetFreeTeamPawns(color);
            Pawn furthestPawn = null;
            foreach (var square in path)
            {
                var pawn = teamPawns.FirstOrDefault(p => square == CurrentSquare(p));
                if (pawn != null) furthestPawn = pawn;
            }
            return furthestPawn;
        }
        public bool IsOccupiedByTeam(ModelEnum.TeamColor color, GameSquare square)
        {
            var pawns = PawnsOnSquare(square);
            if (pawns.Count == 0) return false;
            if (pawns[0].Color == color) return true;
            return false;
        }

        public List<Pawn> PawnsInBase(ModelEnum.TeamColor color) => _allPawn.FindAll(p => CurrentSquare(p) == BaseSquare(color));
        public List<Pawn> PawnsOnSquare(GameSquare square) => _allPawn.FindAll(p => CurrentSquare(p) == square);
        public List<Pawn> GetFreeTeamPawns(ModelEnum.TeamColor color) =>
            GetTeamPawns(color).FindAll(p => CurrentSquare(p).GetType() != typeof(BaseSquare));
        public GameSquare PastSquare(GameSquare square, ModelEnum.TeamColor color)
        {
            var defaultDirection = square.DirectionNext(color);
            var backDirection = ReverseDirection(defaultDirection);
            var diff = NextDiff(backDirection);
            var nextSquare = _boardSquares.Find(x => x.BoardX == square.BoardX + diff.X && x.BoardY == square.BoardY + diff.Y) ?? throw new NullReferenceException();
            return nextSquare;
        }
        public List<Pawn> EnemiesOnSquare(GameSquare targetSquare, ModelEnum.TeamColor color)
        {
            return _allPawn.FindAll(p => p.X == targetSquare.BoardX && p.Y == targetSquare.BoardY && p.Color != color);
        }
        public GameSquare CurrentSquare(Pawn pawn)
        {
            return _boardSquares.Find(x => x.BoardX == pawn.X && x.BoardY == pawn.Y);
        }
        public List<GameSquare> EnemySquares(ModelEnum.TeamColor color)
        {
            var enemyPawns = _allPawn.FindAll(s => s.Color != color);
            var enemySquares = new List<GameSquare>();
            foreach (var p in enemyPawns)
            {
                enemySquares.Add(CurrentSquare(p));
            }
            return enemySquares;
        }
        public GameSquare GoalSquare()
        {
            return _boardSquares.Find(x => x.GetType() == typeof(GoalSquare));
        }
        public List<GameSquare> SafeZoneSquares(ModelEnum.TeamColor color)
        {
            return _boardSquares.FindAll(s => s.GetType() == typeof(SafezoneSquare) && s.Color == color);
        }
        public GameSquare StartSquare(ModelEnum.TeamColor color)
        {
            return _boardSquares.Find(x => x.GetType() == typeof(StartSquare) && x.Color == color);
        }
        public GameSquare BaseSquare(ModelEnum.TeamColor color)
        {
            return _boardSquares.Find(x => x.GetType() == typeof(BaseSquare) && x.Color == color);
        }
        public List<GameSquare> TeamPath(ModelEnum.TeamColor color)
        {
            var teamSquares = new List<GameSquare>();
            var baseSquare = BaseSquare(color);
            teamSquares.Add(baseSquare);
            GameSquare temp = baseSquare;
            while (temp.GetType() != typeof(GoalSquare))
            {
                temp = GetNext(temp, color);
                teamSquares.Add(temp);
            }
            return teamSquares;
        }
        private (int X, int Y) NextDiff(ModelEnum.BoardDirection direction)
        {
            return direction == ModelEnum.BoardDirection.Up ? (0, -1) :
                direction == ModelEnum.BoardDirection.Right ? (1, 0) :
                direction == ModelEnum.BoardDirection.Down ? (0, 1) :
                direction == ModelEnum.BoardDirection.Left ? (-1, 0) : (0, 0);
        }
        public GameSquare GetNext(GameSquare square, ModelEnum.TeamColor color)
        {
            var diff = NextDiff(square.DirectionNext(color));
            var nextSquare = _boardSquares.Find(x => x.BoardX == square.BoardX + diff.X && x.BoardY == square.BoardY + diff.Y) ?? throw new NullReferenceException();
            return nextSquare;
        }
        public GameSquare GetBack(GameSquare square, ModelEnum.TeamColor color)
        {
            var defaultDirection = square.DirectionNext(color);
            var backDirection = ReverseDirection(defaultDirection);
            var diff = NextDiff(backDirection);
            var nextSquare = _boardSquares.Find(x => x.BoardX == square.BoardX + diff.X && x.BoardY == square.BoardY + diff.Y) ?? throw new NullReferenceException();
            return nextSquare;
        }
        private ModelEnum.BoardDirection ReverseDirection(ModelEnum.BoardDirection direction)
        {
            return direction == ModelEnum.BoardDirection.Down ? ModelEnum.BoardDirection.Up :
                direction == ModelEnum.BoardDirection.Up ? ModelEnum.BoardDirection.Down :
                direction == ModelEnum.BoardDirection.Left ? ModelEnum.BoardDirection.Right :
                ModelEnum.BoardDirection.Left;
        }
    }
}
