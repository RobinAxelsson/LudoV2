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
