using LudoDataAccess.Models.Account;

namespace LudoDataAccess
{
    public interface IDatabaseManagement
    {
        public (bool success, string[] messages) RegisterAccount(string accountName, string email, string password,
            string preferredLanguage);

        public (bool success, string message) Login(string username, string password);
        public (bool success, string message) ValidateToken(string token);
        public Account GetAccountFromToken(string token);
    }
}