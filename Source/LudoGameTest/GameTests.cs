using System.Collections.Generic;
using System.Linq;
using LudoGame.GameEngine;
using LudoGame.GameEngine.Classes;
using LudoGame.GameEngine.Configuration;
using LudoGame.GameEngine.GameSquares;
using LudoGame.GameEngine.Interfaces;
using LudoGameTest.Stubs;
using Xunit;

namespace LudoGameTest
{
    public class GameTests
    {
        private LudoProvider _provider;
        public GameTests()
        {
            var provider = new LudoProvider();
            _provider = provider;
        }

        [Fact]
        public void OrderedPlayerCollection_IsOrdering()
        {
            var players = SimplePlayer.GetFourPlayers();

            var playerCollection = new OrderedPlayerCollection(players, x => x.Find(p => p.Color == GameEnum.TeamColor.Red));

            Assert.True(playerCollection[0].Color == GameEnum.TeamColor.Red);
            Assert.True(playerCollection[1].Color == GameEnum.TeamColor.Green);
            Assert.True(playerCollection[2].Color == GameEnum.TeamColor.Yellow);
            Assert.True(playerCollection[3].Color == GameEnum.TeamColor.Blue);
        }
        [Fact]
        public void OrderedPlayerCollection_IsIterating()
        {
            var players = SimplePlayer.GetFourPlayers();

            var playerCollection = new OrderedPlayerCollection(players, x => x.Find(p => p.Color == GameEnum.TeamColor.Red));

            foreach (var p in playerCollection)
            {
                p.NextToThrow = true;
            }
            var next = playerCollection.Select(p => p.NextToThrow).ToList();
            Assert.False(next.Exists(b => b == false));
        }
        [Fact]
        public void GameAction_SetUpPawns()
        {
            var gameAction = _provider.GetGameService<IGameAction>();
            gameAction.SetUpPawns(new List<Pawn>());
            var boardCollection = _provider.GetGameService<IBoardCollection>();
            var bluePawns = boardCollection.PawnsInBase(GameEnum.TeamColor.Blue);
            var redPawns = boardCollection.PawnsInBase(GameEnum.TeamColor.Red);
            var greenPawns = boardCollection.PawnsInBase(GameEnum.TeamColor.Green);
            var yellowPawns = boardCollection.PawnsInBase(GameEnum.TeamColor.Yellow);
            Assert.True(yellowPawns.Count() == 4);
            Assert.True(bluePawns.Count() == 4);
            Assert.True(redPawns.Count() == 4);
            Assert.True(greenPawns.Count() == 4);
        }
        [Fact]
        public void BoardCollection_GetNext()
        {
            //Arrange
            var boardCollection = _provider.GetGameService<IBoardCollection>();
            var blueStart = boardCollection.StartSquare(GameEnum.TeamColor.Blue);

            //Act
            GameSquare actual = blueStart;
            for (int i = 0; i < 12; i++)
            {
                actual = boardCollection.GetNext(actual, GameEnum.TeamColor.Blue);
            }

            //assert
            var redStart = boardCollection.StartSquare(GameEnum.TeamColor.Red);
            Assert.Equal(redStart, actual);
        }
        [Fact]
        public void GameAction_SetUpOldPawn_AssertBlueStart()
        {
            //Arrange
            var gameAction = _provider.GetGameService<IGameAction>();
            var boardCollection = _provider.GetGameService<IBoardCollection>();
            var blueStart = boardCollection.StartSquare(GameEnum.TeamColor.Blue);
            var pawn = new Pawn(GameEnum.TeamColor.Blue)
            {
                x = blueStart.BoardX,
                y = blueStart.BoardY
            };
            //Act
            gameAction.SetUpPawns(new List<Pawn>()
            {
                pawn
            });

            //Assert
            var actualSquare = boardCollection.CurrentSquare(pawn);
            Assert.Equal(blueStart, actualSquare);
        }
        [Fact]
        public void GameAction_MovePawn_AssertRedStart()
        {
            //Arrange
            var gameAction = _provider.GetGameService<IGameAction>();
            var boardCollection = _provider.GetGameService<IBoardCollection>();
            var blueStart = boardCollection.StartSquare(GameEnum.TeamColor.Blue);
            var pawn = new Pawn(GameEnum.TeamColor.Blue)
            {
                x = blueStart.BoardX,
                y = blueStart.BoardY
            };
            gameAction.SetUpPawns(new List<Pawn>()
            {
                pawn
            });
            //Act
            gameAction.Act(new Pawn[]{pawn}, 12);
            //assert

            var actualSquare = boardCollection.CurrentSquare(pawn);
            
            var expectedRedStart = boardCollection.StartSquare(GameEnum.TeamColor.Red);
            Assert.Equal(expectedRedStart, actualSquare);
        }
        [Fact]
        public void GameAction_MoveTwoPawn_AssertStandard()
        {
            //Arrange
            var gameAction = _provider.GetGameService<IGameAction>();
            var boardCollection = _provider.GetGameService<IBoardCollection>();
            var blueStart = boardCollection.StartSquare(GameEnum.TeamColor.Blue);
            var pawn1 = new Pawn(GameEnum.TeamColor.Blue)
            {
                x = blueStart.BoardX,
                y = blueStart.BoardY
            };
            var pawn2 = new Pawn(GameEnum.TeamColor.Blue)
            {
                x = blueStart.BoardX,
                y = blueStart.BoardY
            };
            gameAction.SetUpPawns(new List<Pawn>()
            {
                pawn1, pawn2
            });
            //Act
            gameAction.Act(new Pawn[] { pawn1, pawn2 }, 6);
            //assert

            var actualSquare1 = boardCollection.CurrentSquare(pawn1);
            var actualSquare2 = boardCollection.CurrentSquare(pawn2);
            var squareType = actualSquare1.GetType();


            Assert.Equal(actualSquare1, actualSquare2);
            Assert.Equal(typeof(StandardSquare), squareType);
            Assert.Equal(actualSquare1, actualSquare2);
        }
        [Theory]
        [InlineData(1, 4, false)]
        [InlineData(6, 4, true)]
        [InlineData(5, 0, false)]
        public void OptionsValidator_GetPlayerOption_CountPawns(int diceroll, int expectedCount, bool expectTakeout)
        {
            //Arrange
            var gameAction = _provider.GetGameService<IGameAction>();
            gameAction.SetUpPawns(new List<Pawn>());
            var validator = _provider.GetGameService<IOptionsValidator>();

            //Act
            var playerOption = validator.GetPlayerOption(GameEnum.TeamColor.Blue, diceroll);

            //Assert
            int pawnsToMoveActual = playerOption.PawnsToMove.Length;
            Assert.Equal(expectTakeout, playerOption.CanTakeOutTwo);
            Assert.Equal(expectedCount, pawnsToMoveActual);
        }
        [Fact]
        public void OptionsValidator_Validate_ToManyInResponse()
        {
            //Arrange
            var gameAction = _provider.GetGameService<IGameAction>();
            gameAction.SetUpPawns(new List<Pawn>());
            var validator = _provider.GetGameService<IOptionsValidator>();
            var playerOption = validator.GetPlayerOption(GameEnum.TeamColor.Blue, 6);

            //Act - Returns all four pawns
            var actualIsValid = validator.ValidateResponse(playerOption, playerOption.PawnsToMove);

            Assert.False(actualIsValid);
        }
        [Fact]
        public void OptionsValidator_Validate_WithNewArray()
        {
            //Arrange
            var gameAction = _provider.GetGameService<IGameAction>();
            gameAction.SetUpPawns(new List<Pawn>());
            var validator = _provider.GetGameService<IOptionsValidator>();
            var playerOption = validator.GetPlayerOption(GameEnum.TeamColor.Blue, 6);
            var pawn = playerOption.PawnsToMove[0];
            var incomingPawn = new Pawn(pawn.color)
            {
                x = pawn.x,
                y = pawn.y
            };
            //Act - Returns all four pawns
            var actualIsValid = validator.ValidateResponse(playerOption, new []{incomingPawn});

            Assert.True(actualIsValid);
        }
    }

}
