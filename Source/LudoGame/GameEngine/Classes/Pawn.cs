namespace LudoGame.GameEngine.Classes
{
    public class Pawn
    {
        public int Id { get; set; }
        public GameEnum.TeamColor Color { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public Pawn(GameEnum.TeamColor color, int id)
        {
            Color = color;
            Id = id;
        }
    }
}