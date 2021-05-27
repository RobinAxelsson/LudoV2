using System;
using System.Diagnostics;
using System.IO;
using LudoDataAccess.Models.Account;
using LudoGame.GameEngine;
using LudoGame.GameEngine.Classes;
using LudoTranslation;
using Newtonsoft.Json;
using Xunit;

namespace LudoWebTests
{
    public class UnitTests
    {
        [Fact]
        public void SendOption()
        {
            var account = new Account()
            {
                PlayerName = "Alban",
                EmailAdress = "aasdf@aef.se",
                Language = TranslationEngine.Languages.EN,
                Id = 0,
                Password = "aafEhrok123456ERTW!"
            };

            
            string json = JsonConvert.SerializeObject(account);

          

            File.WriteAllText(@"C:\Users\axels\Google Drive\Code\VS Code\code-webbutveckling-backend\ludov2-renegades\ludo-v2-group-g5_albin-robin\Source\LudoWebTests\GameClasses\account.json", json);
        }
    }
}
