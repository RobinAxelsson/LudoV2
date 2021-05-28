namespace LudoGame.GameEngine.Classes
{
    public class Pawn
    {
        public int id { get; set; }
        public GameEnum.TeamColor color { get; set; }
        public int x { get; set; }
        public int y { get; set; }

        public Pawn(GameEnum.TeamColor color, int id)
        {
            this.color = color;
            this.id = id;
        }
    }
}