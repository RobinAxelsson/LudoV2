using System;
using System.Collections.Generic;
using System.Linq;
using LudoDataAccess.Authentication;
using LudoDataAccess.Database;
using LudoDataAccess.Models.Account;
using LudoTranslation;
using Microsoft.EntityFrameworkCore;

namespace LudoDataAccess
{
    public class DatabaseManagement : IDatabaseManagement
    {
        private readonly ILudoRepository _repository;
        private readonly TranslationEngine _engine;
        public DatabaseManagement(ILudoRepository repository, TranslationEngine engine)
        {
            _repository = repository;
            _engine = engine;
        }
        public (bool success, string[] messages) RegisterAccount(string accountName, string email, string password, string preferredLanguage)
        {
            var errorList = new List<string>();
            var failed = false;
            if (_repository.Accounts.SingleOrDefault(a => a.PlayerName == accountName) != null)
            {
                failed = true;
                errorList.Add("accountnametaken");
            }
            if (_repository.Accounts.SingleOrDefault(a => a.EmailAdress == email) != null)
                {
                    failed = true;
                    errorList.Add("emailtaken");
                }

        
            var account = new Account
            {
                EmailAdress = email,
                Password = PasswordHashing.HashPassword(password),
                PlayerName = accountName,
            };
            if (TranslationEngine.Languages.Contains(preferredLanguage) && !failed)
            {
                account.Language = preferredLanguage;
                _repository.Add(account);
                _repository.SaveChanges();
                return (true, null);
            }

            if (failed)
            {
                /*
           We cannot input the list as a parameter in the message below because-
           it will be typed to a .ToString() which results in just "System.Collection.List"
           So we format a string instead
           */
                if (!TranslationEngine.Languages.Contains(preferredLanguage))
                {
                    var languages = TranslationEngine.Languages.GetLanguages();
                    var message = "The chosen language was not found. Please pick one of below:\n";
                    languages.ForEach(l => message+= l + "\n");
                    errorList.Add(message);
                }
                return (false, errorList.ToArray());
            }

            return (false, errorList.ToArray());
        }
        public (bool success, string message) Login(string username, string password)
        {
            var usernameIsEmail = false;
            //Set account where username is PlayerName
            var account = _repository.Accounts.FirstOrDefault(x => x.PlayerName == username);
            if (account == null)
            {
                //If it fails, set try set username as email
                account = _repository.Accounts.FirstOrDefault(x => x.EmailAdress == username);
                usernameIsEmail = true;
            }
            if (!usernameIsEmail)
            {
                if(account.Password == PasswordHashing.HashPassword(password) && account.PlayerName == username)
                {
                    var tokenId = Guid.NewGuid().ToString("N");
                    var token = new AccountToken()
                    {
                        Account = account,
                        Token = tokenId,
                        ExpiryDate = DateTime.UtcNow.AddHours(3)
                    };
                    _repository.Add(token);
                    _repository.SaveChanges();
                 //   var expiry = token.ExpiryDate.ToString("R").Replace("GMT", "UTC");
                 var expiry = token.ExpiryDate.ToString("R");
                    return (true, $"token={tokenId};expiry={expiry};sameSite=Lax;path=/");
                }
            }
            else
            {
                if(account != null && account.Password == PasswordHashing.HashPassword(password) && account.EmailAdress == username)
                {
                    var tokenId = Guid.NewGuid().ToString("N");
                    var token = new AccountToken()
                    {
                        Account = account,
                        Token = tokenId,
                        ExpiryDate = DateTime.UtcNow.AddHours(3)
                    };
                    _repository.Add(token);
                    _repository.SaveChanges();
                    var expiry = token.ExpiryDate.ToString("R").Replace("GMT", "UTC");
                    return (true, $"token={tokenId};expiry={expiry};sameSite=Lax;path=/");
                }
            }
            return
                (false, "Incorrect username or password");
        }

        public (bool success, string message) ValidateToken(string token)
        {
            var accountToken = _repository.AccountTokens.Include(t => t.Account).SingleOrDefault(t => t.Token == token);
            if (accountToken != null)
            {
                if (accountToken.ExpiryDate > DateTime.UtcNow)
                {
                    return (true, "Token is valid");
                }
            }
            return (false, "Invalid token");
        }

        public Account GetAccountFromToken(string token)
        {
            var result = ValidateToken(token);
          
            if (result.success)
            {
                var account = _repository.AccountTokens.Include(at => at.Account).SingleOrDefault(a => a.Token == token)
                    ?.Account;
                return account;
            }
            return null;
        }
    }
}