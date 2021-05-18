using LudoAPI.GameEngine;
using LudoAPI.GameEngine.Interfaces;
namespace LudoAPI.Models
{
    public abstract record Player : IGamePlayer
    {
        public int AccountId { get; set; }
        public bool CanThrow { get; set; }
        public int Result { get; set; }
        
        public DataAccess.ModelEnum.TeamColor Color { get; set; }
        public abstract Pawn[] ChoosePlay(PlayerOption playerOption);
    }
}