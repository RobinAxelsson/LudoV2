using System.Diagnostics;
using System.Threading.Tasks;
using LudoDataAccess;
using Microsoft.AspNetCore.SignalR;

namespace LudoWeb.ChatModels
{
    public class AccountHub : Hub
    {
        private readonly IDatabaseManagement _dbm;
        public AccountHub(IDatabaseManagement dbm)
        {
            _dbm = dbm;
        }
        public async Task SendRegistrationData(string accountName, string email, string password, string preferredLanguage)
        {
            var result = _dbm.RegisterAccount(accountName, email, password, preferredLanguage);
            await Clients.Caller.SendAsync("RegistrationResult", result.success, result.message);
        }
        public async Task SendLoginData(string username, string password)
        {
            var result = _dbm.Login(username, password);
            await Clients.Caller.SendAsync("LoginResult", result.success, result.message);
        }
    }
}