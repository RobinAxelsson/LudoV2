namespace LudoGame.GameEngine.GameSquares
{
    public abstract class GameSquare
    {
        public GameEnum.TeamColor? Color { get; set; }
        public int BoardX { get; set; }
        public int BoardY { get; set; }
        public GameEnum.BoardDirection DefaultDirection { get; set; }
        public virtual GameEnum.BoardDirection DirectionNext(GameEnum.TeamColor color) => DefaultDirection;
    }
}