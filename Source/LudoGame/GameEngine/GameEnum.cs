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
            Blue = 0,
            Red = 1,
            Yellow = 2,
            Green = 3
        }

        public enum GameStatus
        {
            Playing,
            Waiting,
            Ended
        }
        public static TeamColor ParseTeamColor(string inputColor)
        {
            var success = Enum.TryParse(inputColor, true, out TeamColor color);
            return success ? color : TeamColor.Yellow;
        }
        public static bool TeamColorExists(string inputColor)
        {
            var success = Enum.TryParse(inputColor, true, out TeamColor color);
            return success;
        }
    }
}
