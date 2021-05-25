using System.Collections.Generic;
using LudoGame.GameEngine;
using LudoGame.GameEngine.Classes;
using LudoGame.GameEngine.Interfaces;

namespace LudoGameTest.Stubs
{
    public class SimplePlayer : IGamePlayer
    {
        public static List<IGamePlayer> GetFourPlayers()
        {
            return new()
            {
                new SimplePlayer(GameEnum.TeamColor.Blue),
                new SimplePlayer(GameEnum.TeamColor.Yellow),
                new SimplePlayer(GameEnum.TeamColor.Green),
                new SimplePlayer(GameEnum.TeamColor.Red)
            };
        }
        public SimplePlayer(GameEnum.TeamColor color)
        {
            Color = color;
            Pawns = new List<Pawn>();
        }
        public ICollection<Pawn> Pawns { get; set; }
        public int Result { get; set; }
        public bool NextToThrow { get; set; }
        public GameEnum.TeamColor Color { get; set; }
        public Pawn[] ChoosePlay(PlayerOption playerOption)
        {
            var returnArray = new Pawn[1];
            if (playerOption.PawnsToMove.Length != 0)
            {
                returnArray[0] = playerOption.PawnsToMove[0];
            }

            return returnArray;
        }
    }
}
