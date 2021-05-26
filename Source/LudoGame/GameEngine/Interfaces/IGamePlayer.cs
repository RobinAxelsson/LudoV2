using System.Collections.Generic;
using LudoGame.GameEngine.Classes;

namespace LudoGame.GameEngine.Interfaces
{
    public interface IGamePlayer
    {
        public ICollection<Pawn> Pawns { get; set; }
        public int Result { get; set; }
        public bool NextToThrow { get; set; }
        public GameEnum.TeamColor Color { get; set; }
        public Pawn[] ChoosePlay(PlayerOption playerOption);
    }
}