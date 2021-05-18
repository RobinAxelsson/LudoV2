using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using LudoAPI.DataAccess;
using LudoAPI.Models;
using LudoAPI.Models.Account;
using LudoTranslation;
using Microsoft.Extensions.Configuration;

namespace LudoAPI.SMTP
{
    public class EmailClient
    {
        private readonly ILudoRepository _repository;
        private readonly IConfiguration _configuration;
        public EmailClient(ILudoRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }
        public void SendInvite(string[] recipients, Game game, Account host)
        {
            //Standard SMTP server for gmail
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                //Standard SMTP port
                Port = 587, 
                Credentials = new NetworkCredential("ludoinvites.pgbsnh20@gmail.com", "kanelbulle123"),
                EnableSsl = true,
            };
            var accounts = _repository.Accounts;
            //Send each email separate so we can use the target's native language
            //This is also to avoid each user to see every recipient and exposing their addresses.
            foreach (var account in accounts)
            {
                if (recipients.Contains(account.EmailAdress))
                {
                    var te = new TranslationEngine();
                    te.InitializeLanguage(account.Language);
                    var message = new MailMessage
                    {
                        From = new MailAddress("ludoinvites.pgbsnh20@gmail.com"),
                        Subject = $"{host.PlayerName} " + Dict.Email_Subject,
                        IsBodyHtml = true,
                        Body = GenerateBody(game.GameId, host.PlayerName, game.Url)
             
                    };
                    message.To.Add(account.EmailAdress);
                    smtpClient.Send(message);
                }
            }
        }
        private string GenerateBody(string gameId, string accountId, string gameUrl)
        {
            var doc = File.ReadAllText(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) +
                             "/SMTP/Resources/emailbody_raw.html");
            doc = doc
                .Replace("GAMEID", gameId)
                .Replace("ACCOUNTID", accountId)
                .Replace("INVITEDTITLE", Dict.Email_Title)
                .Replace("SUBTITLE", Dict.Email_Subtitle)
                .Replace("JOINGAMEURI", gameUrl);
            //Above path will need to be updated.
            File.WriteAllText(@"C:\Users\Albin\Desktop\test.html", doc);
            return doc;
        }
    }
}