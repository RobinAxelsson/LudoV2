using System.IO;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using LudoAPI.Translation;

namespace LudoAPI.SMTP
{
    public static class EmailClient
    {
        public static void SendInvite(string recipient)
        {
            //Standard SMTP server for gmail
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                //Standard SMTP port
                Port = 587, 
                Credentials = new NetworkCredential("Insert our ludo email", "insert our ludo password"),
                EnableSsl = true,
            };
            
            var body = GenerateBody("Insert game ID", "Insert account ID", "Insert game URL");
            var subject = "Insert account id " + Dict.Email_Subject;
            smtpClient.Send("Insert our ludo email", recipient, subject, body);
        }
        public static string GenerateBody(string gameId, string accountId, string gameUrl)
        {
            var doc = File.ReadAllText(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) +
                             "/SMTP/Resources/emailbody_raw.html");
            doc = doc
                .Replace("GAMEID", gameId)
                .Replace("ACCOUNTID", accountId)
                .Replace("INVITEDTITLE", Dict.Email_Title)
                .Replace("SUBTITLE", Dict.Email_Subtitle)
                .Replace("JOINGAMEURI", gameUrl)
                .Replace("IMGURL", Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) +
                                   "/SMTP/Resources/email_ludo_game_grey.jpg");
            //Above path will need to be updated.
            File.WriteAllText(@"C:\Users\Albin\Desktop\test.html", doc);
            return doc;
        }
    }
}