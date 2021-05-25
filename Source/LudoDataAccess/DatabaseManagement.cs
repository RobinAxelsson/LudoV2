using System;
using System.Linq;
using LudoDataAccess.Authentication;
using LudoDataAccess.Database;
using LudoDataAccess.Models.Account;
using LudoTranslation;

namespace LudoDataAccess
{
    public class DatabaseManagement : IDatabaseManagement
    {
        private readonly ILudoRepository _repository;
        public DatabaseManagement(ILudoRepository repository)
        {
            _repository = repository;
        }
        public (bool success, string message) RegisterAccount(string accountName, string email, string password, string preferredLanguage)
        {
            var te = new TranslationEngine();
            if (_repository.Accounts.SingleOrDefault(a => a.PlayerName == accountName) != null)
                return (false, "Account name already exists");
            if (_repository.Accounts.SingleOrDefault(a => a.EmailAdress == email) != null)
                return (false, "Email is already registered");
            
            
            var account = new Account
            {
                EmailAdress = email,
                Password = PasswordHashing.HashPassword(password),
                PlayerName = accountName,
            };
            if (TranslationEngine.Languages.Contains(preferredLanguage))
            {
                account.Language = preferredLanguage;
                _repository.Add(account);
                _repository.SaveChanges();
                return (true, "Account has been registered");
            }
            /*
            We cannot input the list as a parameter in the message below because-
            it will be typed to a .ToString() which results in just "System.Collection.List"
            So we format a string instead
            */
            var languages = TranslationEngine.Languages.GetLanguages();
            var message = "The chosen language was not found. Please pick one of below:\n";
            languages.ForEach(l => message+= l + "\n");
            return (false, message);
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
                    return (true, tokenId);
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
                    return (true, tokenId);
                }
            }
            return
                (false, "Incorrect username or password");
        }
    }
}