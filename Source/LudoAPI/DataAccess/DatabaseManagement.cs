//using System.IO;
//using Microsoft.EntityFrameworkCore;
//using Ludo_Web.MVC.Models; using static Ludo_Web.MVC.Models.ModelEnum;
//using Ludo_Web.MVC.GameEngine;
//using System.Linq;
//using System;
//using System.Collections.Generic;
//using LudoEngine.BoardUnits.Main;
//using System.Threading;
//using Ludo_Web.MVC.GameEngine;
//using Ludo_Web.MVC.Models; using static Ludo_Web.MVC.Models.ModelEnum;

//namespace Ludo_Web.DataAccess
//{
//    public static class DatabaseManagement
//    {
//        public static string ConnectionString { get; private set; }
//        public static Thread SaveThread;
//        private static GamePlay _gamePlay { get; set; }
//        public static void SaveInit(GamePlay gamePlay)
//        {
//            _gamePlay = gamePlay;
//            SaveThread = new Thread(new ThreadStart(() => save(gamePlay)));
//            SaveThread.IsBackground = true;
//            GamePlay.OnPlayerEndsRoundEvent += Save;
//        }
//        public static void Save(GamePlay gamePlay)
//        {
//            if (!SaveThread.IsAlive)
//            {
//                SaveThread = new Thread(new ThreadStart(() => save(gamePlay)));
//                SaveThread.Start();
//            }
//        }

//        public static void ReadConnectionString(string filepath)
//        {
//            ConnectionString = File.ReadAllText(filepath);
//        }

//        public static void SavePlayer(string Name, int gameId)
//        {
//            using var db = new LudoContext();

//            var player = new Player { PlayerName = Name };
//            db.Players.Add(player);

//            db.SaveChanges();


//            var playerGame = new GameStat { GameId = gameId, PlayerId = player.Id };
//            db.Players.Add(playerGame);

//            db.SaveChanges();
//        }

//        public static void SaveAndGetGame(TeamColor currentTurn)
//        {
//            using var db = new LudoContext();

//            var saveGame = new Game { CurrentTurn = currentTurn};
//            db.Games.Update(saveGame);
//            db.SaveChanges();

//            var getGame = db.Games
//                .Where(x => x.Id ==  saveGame.Id)
//                .First();

//            StageSaving.Game = getGame;
//        }

//        public static List<Game> GetGames()
//        {
//            using var db = new LudoContext();

//            var game = db.Games
//                .Select(x => x)
//                .OrderByDescending(x => x.LastSaved);

//            List<Game> games = new();
//            if (game != null)
//            {
//                games.AddRange(game);
//            }

//            return games;
//        }

//        public static void GetPlayersInGame(int gameId)
//        {
//            using var db = new LudoContext();

//            var player = db.Players;
//            var Players = db.Players;

//            var players = player
//                .Join(
//                Players,
//                player => player.Id,
//                Players => Players.PlayerId,
//                (player, Players) => new
//                {
//                    player.Id,
//                    player.PlayerName,
//                    Players.GameId
//                })
//                .Where(x => x.GameId == gameId);

//            foreach (var item in players)
//            {
//                Console.WriteLine(item.PlayerName);
//            }
//        }

//        public static List<PawnSavePoint> GetPawnPositionsInGame(Game game)
//        {
//            using var db = new LudoContext();

//            var savePoints = db.PawnSavePoints
//                .Where(x => x.Game == game);

//            List<PawnSavePoint> savepointList = new();

//            foreach (var item in savePoints)
//            {
//                PawnSavePoint savepoint = new PawnSavePoint()
//                {
//                    Id = item.Id,
//                    Game = item.Game,
//                    Color = item.Color,
//                    PlayerType = item.PlayerType,
//                    XPosition = item.XPosition,
//                    YPosition = item.YPosition
//                };
//                savepointList.Add(savepoint);
//            }
//            return savepointList;
//        }

//        public static List<Game> GetGameId(int playerId)
//        {
//            using var db = new LudoContext();

//            var gameId = db.Players
//                .Where(x => x.PlayerId == playerId)
//                .Select(x => x.GameId).ToList();

//            return (from item in gameId
//                    from game in db.Games
//                    where game.Id == item
//                    select game
//                     ).ToList();
//        }

//        private static void save(GamePlay gamePlay)
//        {
//            TeamColor currentTeam = _gamePlay.CurrentPlayer().Color;
//            StageSaving.Pawns = Board.GetTeamPawns(currentTeam);

//            List<Pawn> pawns = StageSaving.Pawns;
//            Game game = new();
//            if (StageSaving.Game != null)
//            {
//                game = StageSaving.Game;
//            }
//            else
//            {
//                SaveAndGetGame(currentTeam);
//                game = StageSaving.Game;
//            }
//            using var db = new LudoContext();

            
//            if (db.PawnSavePoints.Any(x => x.Color == currentTeam && x.Game == game))
//            {
//                var querry = db.PawnSavePoints
//                .Where(x => x.Color == currentTeam && x.Game == game);

//                Pawn[] pawnsArray = pawns.ToArray();
//                int i = 0;
//                foreach (var dbData in querry)
//                {
//                    dbData.Color = pawnsArray[i].Color;
//                    dbData.XPosition = pawnsArray[i].CurrentSquare().BoardX;
//                    dbData.YPosition = pawnsArray[i].CurrentSquare().BoardY;
//                    dbData.Game = game;
//                    i++;
//                }
//            }
//            else
//            {
//                foreach (var pawn in pawns)
//                {
//                    Type playerType = gamePlay.Players.Find(x => x.Color == pawn.Color).GetType();
//                    int iPlayerType = -1;
//                    if (playerType == typeof(HumanPlayer)) iPlayerType = 0;
//                    if(playerType == typeof(Stephan)) iPlayerType = 1;
//                    if (iPlayerType == -1) throw new Exception("Playertype is not available");

//                    var pawnPosition = new PawnSavePoint { Color = pawn.Color, XPosition = pawn.CurrentSquare().BoardX, YPosition = pawn.CurrentSquare().BoardY, Game = game, PlayerType = iPlayerType};

//                    db.Update(pawnPosition);
//                }
//            }
//            db.SaveChanges();

//            var result = db.Games
//                .Where(x => x.Id == game.Id)
//                .SingleOrDefault();
//            if (result != null)
//            {
//                result.CurrentTurn = gamePlay.NextPlayerForSave();
//                result.LastSaved = DateTime.Now;
//                db.Games.Attach(result);
//                db.Entry(result).State = EntityState.Modified;
//                db.SaveChanges();
//            }
//        }
//    }

//}