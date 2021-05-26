using System.Diagnostics;
using System.Globalization;
using LudoDataAccess;
using LudoTranslation;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LudoWeb.Pages
{
    public class Login : PageModel
    {
        public Dict Dictionary { get; private set; }
        public readonly string RegionCode = RegionInfo.CurrentRegion.TwoLetterISORegionName;
        private readonly IDatabaseManagement _dbm;
        public Login(IDatabaseManagement dbm)
        {
            _dbm = dbm;
        }
        public void OnGet()
        {
            var engine = new TranslationEngine();
            Dictionary = engine.InitializeLanguage(TranslationEngine.Languages.Contains(RegionCode) ? RegionCode : "EN");
        }
    }

 
}