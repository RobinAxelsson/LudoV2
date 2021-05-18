using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace LudoAPI.Translation
{
    public static class TranslationEngine
    {
        private static readonly Dictionary<string, string> Dictionary;

        static TranslationEngine()
        {
            Dictionary = new Dictionary<string, string>();
        }

        public static void InitializeLanguage(string lang)
        {
            var line = "";
            var reader = new StreamReader(Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) + "/Translation/Translations/" + lang + ".lang");
            while ((line = reader.ReadLine()) != null && !string.IsNullOrWhiteSpace(line) && line.Contains("=="))
            {
                var lineSplit = line.Split("==");
                Dictionary.Add(lineSplit[0], lineSplit[1]);
                foreach (var prop in typeof(Dict).GetProperties())
                    prop.SetValue(null, Dictionary.SingleOrDefault(k => k.Key == prop.Name).Value);
            }
        }

        public enum Language
        {
            en_US,
            sv_SE
        }

        public static Language ParseEnum(string language)
        {
            var success = Enum.TryParse(language, true, out Language lang);
            return success ? lang : Language.en_US;
        }
        public static bool EnumExists(string language)
        {
            var success = Enum.TryParse(language, true, out Language lang);
            return success;
        }

    }
}