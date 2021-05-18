using LudoAPI.GameEngine.Interfaces;
using LudoAPI.Models;

namespace LudoAPI.GameEngine
{
    public record RemotePlayer : Player, IGamePlayer
    {
        public override Pawn[] ChoosePlay(PlayerOption playerOption)
        {
            //await frontend result
            return new Pawn[0];
        }
    }
}
