using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace LudoTranslation
{
    public class TranslationEngine
    {
        private readonly Dictionary<string, string> Dictionary;

         public TranslationEngine()
        {
            Dictionary = new Dictionary<string, string>();
        }

        public Dict InitializeLanguage(string lang)
        {
            var dict = new Dict();
            var line = "";
            var assembly = Assembly.GetExecutingAssembly();
            // var resourceName = "MyCompany.MyProduct.MyFile.txt";
            using (var stream = assembly.GetManifestResourceStream("LudoTranslation.Translations." + lang + ".lang"))
            using (var reader = new StreamReader(stream))
            {
               
                while ((line = reader.ReadLine()) != null)
                {
                    //Don't do anything if line is just blank or doesn't contain double equals
                    if (string.IsNullOrWhiteSpace(line) || !line.Contains("==")) continue; 
                    var lineSplit = line.Split("==");
                    Dictionary.Add(lineSplit[0], lineSplit[1]);
                    foreach (var prop in dict.GetType().GetProperties())
                        prop.SetValue(dict, Dictionary.SingleOrDefault(k => k.Key == prop.Name).Value);
                }
            }

            return dict;

        }
        public static class Languages
        {
            
            public const string EN = "EN";
            public const string SE = "SE";
            public static bool Contains(string input)
            {
                
                return typeof(Languages).GetFields().Select(f => f.Name.ToLower()).ToList().Contains(input.ToLower());
            }
            public static List<string> GetLanguages()
            {
                return typeof(Languages).GetFields().Select(f => f.Name).ToList();
            }

            public static string Parse(string input)
            {
                return typeof(Languages).GetFields().Select(f => f.Name).SingleOrDefault(l => string.Equals(l, input, StringComparison.InvariantCultureIgnoreCase));
            }
        }
        /*
         * Archived code.
           Language used to be an enum
           
        public Language ParseEnum(string language)
        {
            var success = Enum.TryParse(language, true, out Language lang);
            return success ? lang : Language.en_US;
        }
        public bool EnumExists(string language)
        {
            var success = Enum.TryParse(language, true, out Language lang);
            return success;
        }
        */
    }
}