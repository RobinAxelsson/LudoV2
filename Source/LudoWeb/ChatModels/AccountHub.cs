using System;
using System.Diagnostics;
using System.Threading.Tasks;
using LudoDataAccess;
using LudoTranslation;
using Microsoft.AspNetCore.SignalR;

namespace LudoWeb.ChatModels
{
    public class AccountHub : Hub
    {
        private readonly IDatabaseManagement _dbm;
        private readonly TranslationEngine _engine;
        
        public AccountHub(IDatabaseManagement dbm, TranslationEngine engine)
        {
            _dbm = dbm;
            _engine = engine;
            
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
        public async Task SendCookie(string cookie)
        {
            if (cookie.Contains("="))
                cookie = cookie.Split("=")[1];
            var result = _dbm.ValidateToken(cookie);
            await Clients.Caller.SendAsync("CookieResult", result.success, result.message);
        }

        public async Task RequestTranslation(string[] properties, string language)
        {
            var dict = _engine.InitializeLanguage(language);
            for (var i = 0; i <= properties.Length - 1; i++)
            {
                properties[i] = dict.GetPropertyValue(properties[i]);
            }
            await Clients.Caller.SendAsync("TranslationDelivery", properties);
        }
    }
}