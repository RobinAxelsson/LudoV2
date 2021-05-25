namespace LudoDataAccess
{
    public interface IDatabaseManagement
    {
        public (bool success, string message) RegisterAccount(string accountName, string email, string password,
            string preferredLanguage);

        public (bool success, string message) Login(string username, string password);
    }
}