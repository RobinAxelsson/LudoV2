using System;
using System.Collections.Generic;
using System.Linq;
using LudoAPI.Controllers.Bodies;
using LudoAPI.DataAccess;
using LudoAPI.Models;
using LudoAPI.Models.Account;
using LudoAPI.SMTP;
using LudoGame.GameEngine;
using LudoTranslation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace LudoAPI.Controllers
{
    [Route("api/[controller]")]
    public class GameController : Controller
    {
        private readonly ILudoRepository _repository;
        private readonly IConfiguration _configuration;
        public GameController(ILudoRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }
      //  [Authorize]
        [HttpPost]
        [Route("CreateGame")]
        public IActionResult CreateGame([FromBody] InviteBody model)
        {
            //Cache account object for future usage
            var account = _repository.Accounts.SingleOrDefault(a => a.PlayerName == model.HostAccountName);
            //Make sure host exists
            if (account == null)
                return NotFound($"Couldn't find any user with the PlayerName {model.HostAccountName}");
            
            //Make sure no more than 3 people has been invited
            if (model.Recipients.Length > 3)
                return UnprocessableEntity("You cannot invite more than 3 players");
            
            //Create the game object
            var id = Guid.NewGuid().ToString("N");
            var game = new Game()
            {
                GameId = id,
                GameStatus = ModelEnum.GameStatus.WaitingForAllPlayers,
                Players = new List<Player>()
            };
            //Create player object
            //Get random index to decide color
            var rng = new Random().Next(0, 3);
            var player = new Player()
            {
                AccountId = account.Id,
                Type = ModelEnum.PlayerType.Remote,
                Color = (GameEnum.TeamColor)rng
            };
            game.Players.Add(player);
            
            //Check if recipients has been inputted. This is an optional input
            if (model.Recipients != null)
            {
                //Create a list that will fill up with each address that wasn't found.
                var notFoundAddresses = new List<string>();
                model.Recipients.ToList().ForEach( a=>
                {
                    if (!_repository.Accounts.Select(x => x.EmailAdress).ToList().Contains(a))
                    {
                        notFoundAddresses.Add(a);
                    }
                });
                if (notFoundAddresses.Count > 0)
                {
                    /*
                    We cannot input the list as a parameter in the message below because-
                    it will be typed to a .ToString() which results in just "System.Collection.List"
                    So we format a string instead
                    */
                    var message = "";
                    notFoundAddresses.ForEach(s => message += s + "\n");
                    return NotFound($"Below email address(es) has no registered user\n{message}");
                }
                //Create a placeholder account object that contains information from InviteBody.
                //This is to avoid providing unnecessary data which would be the case if we would input an account object instead
                var placeholder = new Account { PlayerName =  model.HostAccountName,};
                var client = new EmailClient(_repository, _configuration);
                //This will send invites to each recipient
                client.SendInvite(model.Recipients, game, placeholder);
            }
            return Ok(game);
        }
    }
}