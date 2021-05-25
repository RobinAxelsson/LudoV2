using System.Diagnostics;
using System.Globalization;
using LudoDataAccess;
using LudoTranslation;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LudoWeb.Pages
{
    public class Login : PageModel
    {
        public Dict Dictionary { get; set; }
        private readonly IDatabaseManagement _dbm;
        public Login(IDatabaseManagement dbm)
        {
            _dbm = dbm;
        }
        public void OnGet()
        {
            var engine = new TranslationEngine();
            var languageIso2 = RegionInfo.CurrentRegion.TwoLetterISORegionName;
            Dictionary = engine.InitializeLanguage(TranslationEngine.Languages.Contains(languageIso2) ? languageIso2 : "EN");
        }

        public void LoginAccount(string username, string password)
        {
            var result = _dbm.Login(username, password);
            Debug.Write($"{result.success}\n{result.message}");
        }
    }

 
}