using System;

namespace LudoGame.GameEngine
{
    public static class GameEnum
    {
        public enum BoardDirection
        {
            Up,
            Right,
            Down,
            Left
        }

        public enum TeamColor
        {
            Blue,
            Red,
            Yellow,
            Green
        }

        public enum GameStatus
        {
            Playing,
            Waiting,
            Ended
        }
    }
}
