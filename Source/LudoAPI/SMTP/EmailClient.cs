using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using LudoAPI.Models;
using LudoAPI.Models.Account;
using LudoTranslation;

namespace LudoAPI.SMTP
{
    public static class EmailClient
    {
        public static void SendInvite(string recipient, Game game, Account host)
        {
            //Standard SMTP server for gmail
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                //Standard SMTP port
                Port = 587, 
                Credentials = new NetworkCredential("ludoinvites.pgbsnh20@gmail.com", "kanelbulle123"),
                EnableSsl = true,
            };
            var message = new MailMessage
            {
                From = new MailAddress("ludoinvites.pgbsnh20@gmail.com"),
                Subject = $"{host.PlayerName} " + Dict.Email_Subject,
                IsBodyHtml = true,
                Body = GenerateBody(game.GameId, host.PlayerName, game.Url)
            };
            message.To.Add(recipient);
            smtpClient.Send(message);
        }
        public static void SendInvite(string[] recipients, Game game, Account host)
        {
            //Standard SMTP server for gmail
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                //Standard SMTP port
                Port = 587, 
                Credentials = new NetworkCredential("ludoinvites.pgbsnh20@gmail.com", "kanelbulle123"),
                EnableSsl = true,
            };
            var message = new MailMessage
            {
                From = new MailAddress("ludoinvites.pgbsnh20@gmail.com"),
                Subject = $"{host.PlayerName} " + Dict.Email_Subject,
                IsBodyHtml = true,
                Body = GenerateBody(game.GameId, host.PlayerName, game.Url)
            };
            recipients.ToList().ForEach(s => message.To.Add(s));
            smtpClient.Send(message);
        }
        private static string GenerateBody(string gameId, string accountId, string gameUrl)
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