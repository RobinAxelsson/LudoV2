using System;
using LudoAPI.Controllers.Bodies;
using LudoAPI.DataAccess;
using LudoAPI.Models;
using LudoAPI.Models.Account;
using LudoAPI.SMTP;
using LudoTranslation;
using Microsoft.AspNetCore.Mvc;

namespace LudoAPI.Controllers
{
    [Route("api/[controller]")]
    public class GameController : Controller
    {
        
      //  [Authorize]
        [HttpPost]
        [Route("CreateGame")]
        public Game InvitePlayers([FromBody] InviteBody model)
        {
            var id = Guid.NewGuid().ToString("N");
            var game = new Game()
            {
                GameId = id,
                GameStatus = ModelEnum.GameStatus.WaitingForAllPlayers
            };
            if (model.Recipients != null)
            {
                var placeholder = new Account
                {
                    PlayerName =  model.HostAccountName,
                    Language = "sv_SE"
                };
                TranslationEngine.InitializeLanguage(placeholder.Language);
                EmailClient.SendInvite(model.Recipients, game, placeholder);
            }
            return game;
        }
    }
}