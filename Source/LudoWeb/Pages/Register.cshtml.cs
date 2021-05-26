using System.Globalization;
using LudoTranslation;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace LudoWeb.Pages
{
    public class Register : PageModel
    {
        public Dict Dictionary { get; private set; }
        public readonly string RegionCode = RegionInfo.CurrentRegion.TwoLetterISORegionName;
        public void OnGet()
        {
            var engine = new TranslationEngine();
            Dictionary = engine.InitializeLanguage(TranslationEngine.Languages.Contains(RegionCode) ? RegionCode : "EN");
        }


    }
}