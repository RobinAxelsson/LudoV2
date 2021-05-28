using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using LudoDataAccess.Database;
using LudoDataAccess.Models;
using LudoDataAccess.Models.Account;
using LudoTranslation;

namespace LudoDataAccess.SMTP
{
    public class EmailClient
    {
        private Dict _dict;
        private readonly ILudoRepository _repository;
        public EmailClient(ILudoRepository repository)
        {
            _repository = repository;
        }
        public void SendInvite(string[] recipients, string gameId, string gameUrl, Account host)
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
                    _dict = te.InitializeLanguage(account.Language);
                   
                    var message = new MailMessage
                    {
                        From = new MailAddress("ludoinvites.pgbsnh20@gmail.com"),
                        Subject = $"{host.PlayerName} " + _dict.Email_Subject,
                        IsBodyHtml = true,
                        Body = GenerateBody(gameId, host.PlayerName, gameUrl)
             
                    };
                    message.To.Add(account.EmailAdress);
                    smtpClient.Send(message);
                }
            }
        }
        private string GetEmaiLBodyFromResoruce(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            // var resourceName = "MyCompany.MyProduct.MyFile.txt";
            var result = "";
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }

        private string GenerateBody(string gameId, string accountId, string gameUrl)
        {
            /*
            var doc = File.ReadAllText(Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) +
                             "/SMTP/Resources/emailbody_raw.html");
                             */
            var doc = GetEmaiLBodyFromResoruce("LudoDataAccess.SMTP.Resources.emailbody_raw.html");
            doc = doc
                .Replace("GAMEID", gameId)
                .Replace("ACCOUNTID", accountId)
                .Replace("INVITEDTITLE", _dict.Email_Title)
                .Replace("SUBTITLE", _dict.Email_Subtitle)
                .Replace("JOINGAMEURI", gameUrl);
            //Above path will need to be updated.
            File.WriteAllText(@"C:\Users\Albin\Desktop\test.html", doc);
            return doc;
        }
    }
}