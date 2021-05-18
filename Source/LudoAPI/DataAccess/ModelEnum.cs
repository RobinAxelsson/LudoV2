namespace LudoAPI.DataAccess
{
    //https://docs.microsoft.com/en-us/ef/core/modeling/value-conversions?tabs=data-annotations //ValueConversion
    //https://www.entityframeworktutorial.net/EntityFramework5/enum-in-entity-framework5.aspx //Enumsettings in SSM
    public static class ModelEnum
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
        public enum EndResult
        {
            First = 1,
            Second = 2,
            Third = 3,
            Fourth = 4
        }
        public enum GameStatus
        {
            Created,
            WaitingForAllPlayers,
            Playing,
            Paused,
            Aborted,
            Exception,
            Ended
        }
    
    }
}
