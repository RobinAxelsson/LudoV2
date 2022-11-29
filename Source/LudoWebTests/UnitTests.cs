using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using LudoDataAccess.Models.Account;
using LudoGame.GameEngine;
using LudoGame.GameEngine.Classes;
using LudoGame.GameEngine.GameSquares;
using LudoTranslation;
using Newtonsoft.Json;
using Xunit;

namespace LudoWebTests
{
    public class UnitTests
    {
        [Fact]
        public void SendOption()
        {
            var squares = new BoardOrm().Map();

            var allPawns = new List<Pawn>();
            var colors = Enum.GetValues<GameEnum.TeamColor>();
            int pawnId = 0;
            for (int t = 0; t < 4; t++)
            {
                var baseSquare = squares.Find(x => x.GetType() == typeof(SquareTeamBase) && x.Color == colors[t]);

                for (int p = 0; p < 4; p++)
                {
                    var pawn = new Pawn(colors[t], pawnId);
                    pawnId++;
                    ChangeCoordinates(pawn, baseSquare);
                    allPawns.Add(pawn);
                }
            }

            string json = JsonConvert.SerializeObject(allPawns);

            File.WriteAllText(@"C:\Users\axels\Google Drive\Code\VS Code\code-webbutveckling-backend\ludov2-renegades\ludo-v2-group-g5_albin-robin\Source\LudoWeb\wwwroot\data\set-up-pawns.json", json);
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
            if (targetSquare is SquareGoal)
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
