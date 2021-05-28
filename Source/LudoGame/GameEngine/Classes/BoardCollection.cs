using System;
using System.Collections.Generic;
using System.Linq;
using LudoGame.GameEngine.GameSquares;
using LudoGame.GameEngine.Interfaces;

namespace LudoGame.GameEngine.Classes
{
    public class BoardCollection : IBoardCollection
    {
        private List<GameSquare> _boardSquares { get; }
        public BoardCollection(IBoardOrm boardOrm)
        {
            _boardSquares = boardOrm.Map();
        }
        private List<Pawn> _allPawn;
        public void SetPawns(List<Pawn> allPawn) => _allPawn = allPawn;
        public bool RemovePawn(Pawn pawn)
        {
            var result = _allPawn.Exists(p => p.Equals(pawn));
            if (result == true)
            {
                pawn.x = 0;
                pawn.y = 0;
                _allPawn.Remove(pawn);
            }
            return result;
        }

        public Pawn GetTruePawn(Pawn pawn)
        {
            var pawns = _allPawn;
            return _allPawn.SingleOrDefault(p => p.id == pawn.id);
        }
        public List<GameEnum.TeamColor> TeamsLeft() => _allPawn.Select(p => p.color).ToList();
        public List<Pawn> GetTeamPawns(GameEnum.TeamColor color) => _allPawn.FindAll(p => p.color == color);
        public Pawn FurthestPawn(GameEnum.TeamColor color)
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
        public bool IsOccupiedByTeam(GameEnum.TeamColor color, GameSquare square)
        {
            var pawns = PawnsOnSquare(square);
            if (pawns.Count == 0) return false;
            if (pawns[0].color == color) return true;
            return false;
        }

        public List<Pawn> PawnsInBase(GameEnum.TeamColor color) => _allPawn.FindAll(p => CurrentSquare(p) == BaseSquare(color));
        public List<Pawn> PawnsOnSquare(GameSquare square) => _allPawn.FindAll(p => CurrentSquare(p) == square);
        public List<Pawn> GetFreeTeamPawns(GameEnum.TeamColor color) =>
            GetTeamPawns(color).FindAll(p => CurrentSquare(p).GetType() != typeof(BaseSquare));
        public GameSquare PastSquare(GameSquare square, GameEnum.TeamColor color)
        {
            var defaultDirection = square.DirectionNext(color);
            var backDirection = ReverseDirection(defaultDirection);
            var diff = NextDiff(backDirection);
            var nextSquare = _boardSquares.Find(x => x.BoardX == square.BoardX + diff.X && x.BoardY == square.BoardY + diff.Y) ?? throw new NullReferenceException();
            return nextSquare;
        }
        public List<Pawn> EnemiesOnSquare(GameSquare targetSquare, GameEnum.TeamColor color)
        {
            return _allPawn.FindAll(p => p.x == targetSquare.BoardX && p.y == targetSquare.BoardY && p.color != color);
        }
        public List<Pawn> GetAllPawns()
        {
            return _allPawn;
        }
        public GameSquare CurrentSquare(Pawn pawn)
        {
            return _boardSquares.Find(x => x.BoardX == pawn.x && x.BoardY == pawn.y);
        }
        public List<GameSquare> EnemySquares(GameEnum.TeamColor color)
        {
            var enemyPawns = _allPawn.FindAll(s => s.color != color);
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
        public GameSquare WinnerSquare()
        {
            return _boardSquares.Find(x => x.GetType() == typeof(WinnerSquare));
        }
        public List<GameSquare> SafeZoneSquares(GameEnum.TeamColor color)
        {
            return _boardSquares.FindAll(s => s.GetType() == typeof(SafezoneSquare) && s.Color == color);
        }
        public GameSquare StartSquare(GameEnum.TeamColor color)
        {
            return _boardSquares.Find(x => x.GetType() == typeof(StartSquare) && x.Color == color);
        }
        public GameSquare BaseSquare(GameEnum.TeamColor color)
        {
            return _boardSquares.Find(x => x.GetType() == typeof(BaseSquare) && x.Color == color);
        }
        public List<GameSquare> TeamPath(GameEnum.TeamColor color)
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
        private (int X, int Y) NextDiff(GameEnum.BoardDirection direction)
        {
            return direction == GameEnum.BoardDirection.Up ? (0, -1) :
                direction == GameEnum.BoardDirection.Right ? (1, 0) :
                direction == GameEnum.BoardDirection.Down ? (0, 1) :
                direction == GameEnum.BoardDirection.Left ? (-1, 0) : (0, 0);
        }
        public GameSquare GetNext(GameSquare square, GameEnum.TeamColor color)
        {
            var diff = NextDiff(square.DirectionNext(color));
            var nextSquare = _boardSquares.Find(x => x.BoardX == square.BoardX + diff.X && x.BoardY == square.BoardY + diff.Y) ?? throw new NullReferenceException();
            return nextSquare;
        }
        public GameSquare GetBack(GameSquare square, GameEnum.TeamColor color)
        {
            var defaultDirection = square.DirectionNext(color);
            var backDirection = ReverseDirection(defaultDirection);
            var diff = NextDiff(backDirection);
            var nextSquare = _boardSquares.Find(x => x.BoardX == square.BoardX + diff.X && x.BoardY == square.BoardY + diff.Y) ?? throw new NullReferenceException();
            return nextSquare;
        }
        private GameEnum.BoardDirection ReverseDirection(GameEnum.BoardDirection direction)
        {
            return direction == GameEnum.BoardDirection.Down ? GameEnum.BoardDirection.Up :
                direction == GameEnum.BoardDirection.Up ? GameEnum.BoardDirection.Down :
                direction == GameEnum.BoardDirection.Left ? GameEnum.BoardDirection.Right :
                GameEnum.BoardDirection.Left;
        }
    }
}
