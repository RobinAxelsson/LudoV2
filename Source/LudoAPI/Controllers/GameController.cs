using System;
using System.Collections.Generic;
using System.Linq;
using LudoAPI.Controllers.Bodies;
using LudoAPI.DataAccess;
using LudoAPI.Models;
using LudoAPI.Models.Account;
using LudoAPI.SMTP;
using LudoGame.GameEngine;
using LudoGame.GameEngine.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [Authorize]
        [HttpPost]
        [Route("CreateGame")]
        public IActionResult CreateGame(string[] recipients)
        {
            var token = Request.Headers.FirstOrDefault(p => p.Key == "Authorization")
                .Value.FirstOrDefault()
                ?.Replace("Bearer ", "");
            var account = _repository.AccountTokens.Include(a => a.Account)
                .SingleOrDefault(t => t.Token == token)
                ?.Account;
            if (account == null) return NotFound($"Couldn't find account");

            //Make sure no more than 3 people has been invited
            if (recipients.Length > 3) return UnprocessableEntity("You cannot invite more than 3 players");

            //Create the game object
            var id = Guid.NewGuid().ToString("N");
            var game = new Game()
            {
                GameId = id, GameStatus = ModelEnum.GameStatus.WaitingForAllPlayers
            };
            //Create player object
            //Get random index to decide color
            var rng = new Random().Next(0, 3);
            var player = _repository.Players.FirstOrDefault(p => p.AccountId == account.Id) ?? new Player()
            {
                AccountId = account.Id, Type = ModelEnum.PlayerType.Remote, Color = (GameEnum.TeamColor) rng
            };
            player.Game = game;
            _repository.Update(player);
            //Check if recipients has been inputted. This is an optional input
            if (recipients.Length > 0)
            {
                //Create a list that will fill up with each address that wasn't found.
                var notFoundAddresses = new List<string>();
                recipients.ToList()
                    .ForEach(a =>
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
                var placeholder = new Account {PlayerName = account.PlayerName,};
                var client = new EmailClient(_repository, _configuration);
                //This will send invites to each recipient
                client.SendInvite(recipients, game, placeholder);
            }
            _repository.Add(game);
            _repository.SaveChanges();
            return Ok(game);
        }
        
        [Authorize]
        [HttpPost]
        [Route("InvitePlayer")]
        public IActionResult InvitePlayer(string gameId, string[] recipients)
        {
            var token = Request.Headers.FirstOrDefault(p => p.Key == "Authorization")
                .Value.FirstOrDefault()
                ?.Replace("Bearer ", "");
            var account = _repository.AccountTokens.Include(a => a.Account)
                .SingleOrDefault(t => t.Token == token)
                ?.Account;
            if (account == null) return NotFound($"Couldn't find account");
            var game = _repository.Games.SingleOrDefault(g => g.GameId == gameId);
            if (game == null) return NotFound($"Could not find a game with the ID: {gameId}");
            //Make sure no more than 3 people has been invited
            if (recipients.Length > 3) return UnprocessableEntity("You cannot invite more than 3 players");
            //Check if recipients has been inputted.
            if (recipients.Length == 0)
                return Conflict("Recipients cannot be empty");
            
                //Create a list that will fill up with each address that wasn't found.
            var notFoundAddresses = new List<string>();
            recipients.ToList()
                .ForEach(a =>
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
            var placeholder = new Account {PlayerName = account.PlayerName,};
            var client = new EmailClient(_repository, _configuration);
            //This will send invites to each recipient
            client.SendInvite(recipients, game, placeholder);
            return Ok(game);
        }


        [Authorize]
        [HttpPost]
        [Route("JoinGame")]
        public IActionResult JoinGame(string gameId)
        {
            var token = Request.Headers.FirstOrDefault(p => p.Key == "Authorization")
                .Value.FirstOrDefault()
                ?.Replace("Bearer ", "");
            var account = _repository.AccountTokens.Include(a => a.Account)
                .SingleOrDefault(t => t.Token == token)
                ?.Account;
            if (account == null) return NotFound($"Couldn't find account");
            var game = _repository.Games.SingleOrDefault(g => g.GameId == gameId);
            if (game == null) return NotFound($"Could not find a game with the ID: {gameId}");
            var gamePlayers = _repository.Players.Include(g => g.Game).Where(p => p.Game.GameId == gameId).ToList();
            if (gamePlayers.Count == 4) return Conflict("Lobby is full");
            var takenColors = gamePlayers.Select(p => (int) p.Color).ToList();
            //Select random not taken color
            var rng = -1; //placeholder value
            do
            { 
                rng = new Random().Next(0, 3);
            } while (takenColors.Contains(rng));
            var player = _repository.Players.FirstOrDefault(p => p.AccountId == account.Id) ?? new Player()
            {
                AccountId = account.Id,
                Color = (GameEnum.TeamColor) rng,
                Type = ModelEnum.PlayerType.Remote
            };
            player.Game = game;
            _repository.Update(player);
            _repository.SaveChanges();
            return Ok(game);
        }
        [Authorize]
        [HttpPost]
        [Route("AddAI")]
        public IActionResult AddAI(string gameId)
        {
            var token = Request.Headers.FirstOrDefault(p => p.Key == "Authorization")
                .Value.FirstOrDefault()
                ?.Replace("Bearer ", "");
            var account = _repository.AccountTokens.Include(a => a.Account)
                .SingleOrDefault(t => t.Token == token)
                ?.Account;
            if (account == null) return NotFound($"Couldn't find account");
            var game = _repository.Games.SingleOrDefault(g => g.GameId == gameId);
            if (game == null) return NotFound($"Could not find a game with the ID: {gameId}");
            var gamePlayers = _repository.Players.Include(g => g.Game).Where(p => p.Game.GameId == gameId).ToList();
            if (gamePlayers.Count == 4) return Conflict("Lobby is full");
            var takenColors = gamePlayers.Select(p => (int) p.Color).ToList();
            //Select random not taken color
            var rng = -1; //placeholder value
       
            do
            { 
                rng = new Random().Next(0, 3);
            } while (takenColors.Contains(rng));

            var player = new Player()
            {
                Color = (GameEnum.TeamColor) rng,
                Game = game,
                Type = ModelEnum.PlayerType.Stephan
            };
                _repository.Add(player);
                _repository.SaveChanges();
                return Ok(game);
        }
    }
}