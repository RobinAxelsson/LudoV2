using System.Globalization;
using LudoTranslation;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace LudoWeb.Pages
{
    public class Register : PageModel
    {
        public Dict Dictionary { get; set; }
        public void OnGet()
        {
            var engine = new TranslationEngine();
            var languageIso2 = RegionInfo.CurrentRegion.TwoLetterISORegionName;
            Dictionary = engine.InitializeLanguage(TranslationEngine.Languages.Contains(languageIso2) ? languageIso2 : "EN");
        }
    }
}