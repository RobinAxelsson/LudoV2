using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using LudoAPI.Authentication;
using LudoAPI.DataAccess;
using LudoAPI.Models;
using LudoAPI.Models.Account;
using LudoAPI.Translation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Account = LudoAPI.Models.Account.Account;

namespace LudoAPI.Controllers 
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
     private readonly ILudoRepository _repository;
       private readonly IConfiguration _configuration;
       public AccountController(ILudoRepository repository, IConfiguration configuration)
        {
            _repository = repository;
           _configuration = configuration;
        }
        
        
        [Authorize]
        [HttpGet]
        [Route("GetAccounts")]
        public List<Account> GetAccounts()
        {
       
            return new List<Account>();
        }   
        [HttpPost]
        [Route("Register")]
        public IActionResult Register([FromBody] RegisterModel model)
        {
            if (_repository.Accounts.SingleOrDefault(a => a.PlayerName == model.AccountName) != null)
                return Conflict();
            var account = new Account
            {
                EmailAdress = model.Email,
                Password = PasswordHashing.HashPassword(model.Password),
                PlayerName = model.AccountName,
                Role = Role.User
            };
            var lang = TranslationEngine.ParseEnum(model.PreferredLanguage);
            if (TranslationEngine.EnumExists(model.PreferredLanguage))
            {
                account.Language = TranslationEngine.ParseEnum(model.PreferredLanguage);
                _repository.Add(account);
                _repository.SaveChanges();
                return Ok(account.Language.ToString());
            }
            var enums = Enum.GetValues<TranslationEngine.Language>()
                    .Select(language => language.ToString()).ToList();
                var message = "The chosen language was not found. Please pick one of below:\n";
                enums.ForEach(l => message+= l + "\n");
                return NotFound(message);
        }
        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            var account = _repository.Accounts.FirstOrDefault(x => x.PlayerName == model.Username);
            if (account == null) return NotFound();
            if (account.Password != PasswordHashing.HashPassword(model.Password) || account.PlayerName != model.Username) return Unauthorized();
            var identity = GetClaimsIdentity(account);
            var token = GetJwtToken(identity);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            var userToken = new AccountToken()
            {
                Account = account,
                ExpiryDate = token.ValidTo,
                Token = tokenString
            };
            _repository.Add(userToken);
            _repository.SaveChanges();
            return Ok(new
            {
                token = tokenString,
                expiration = token.ValidTo
            });
        }
        private JwtSecurityToken GetJwtToken(ClaimsIdentity identity)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                notBefore: DateTime.UtcNow,
                claims: identity.Claims,
                // our token will live 1 hour, but you can change you token lifetime here
                expires: DateTime.Now.Add(TimeSpan.FromHours(3)),
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));
            return jwtSecurityToken;
        }
        private ClaimsIdentity GetClaimsIdentity(Account account)
        {
            // Here we can save some values to token.
            // For example we are storing here user id and email
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, account.PlayerName)
            };
            var claimsIdentity = new ClaimsIdentity(claims, "Token");

            // Adding roles code
            // Roles property is string collection but you can modify Select code if it it's not
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, account.Role));
            return claimsIdentity;
        }
    }
}