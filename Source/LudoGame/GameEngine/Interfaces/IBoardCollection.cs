using System.Collections.Generic;
using LudoGame.GameEngine.Classes;
using LudoGame.GameEngine.GameSquares;

namespace LudoGame.GameEngine.Interfaces
{
    public interface IBoardCollection
    {
        List<GameEnum.TeamColor> TeamsLeft();
        List<Pawn> EnemiesOnSquare(GameSquare targetSquare, GameEnum.TeamColor color);
        bool IsOccupiedByTeam(GameEnum.TeamColor color, GameSquare square);
        List<Pawn> PawnsInBase(GameEnum.TeamColor color);
        List<Pawn> PawnsOnSquare(GameSquare square);
        List<Pawn> GetFreeTeamPawns(GameEnum.TeamColor color);
        GameSquare CurrentSquare(Pawn pawn);
        GameSquare PastSquare(GameSquare square, GameEnum.TeamColor color);
        List<Pawn> GetTeamPawns(GameEnum.TeamColor color);
        Pawn FurthestPawn(GameEnum.TeamColor color);
        List<GameSquare> EnemySquares(GameEnum.TeamColor color);
        GameSquare GoalSquare();
        GameSquare WinnerSquare();
        List<GameSquare> SafeZoneSquares(GameEnum.TeamColor color);
        GameSquare StartSquare(GameEnum.TeamColor color);
        GameSquare BaseSquare(GameEnum.TeamColor color);
        List<GameSquare> TeamPath(GameEnum.TeamColor color);
        GameSquare GetNext(GameSquare square, GameEnum.TeamColor color);
        GameSquare GetBack(GameSquare square, GameEnum.TeamColor color);
        bool RemovePawn(Pawn pawn);
        void SetPawns(List<Pawn> allPawns);
    }
}