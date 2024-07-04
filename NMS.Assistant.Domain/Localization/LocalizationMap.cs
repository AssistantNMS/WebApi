using System.Collections.Generic;
using NMS.Assistant.Domain.Dto.Enum;

namespace NMS.Assistant.Domain.Localization
{
    public class LocalizationMap
    {
        public string Code { get; set; }
        public LanguageType Type { get; set; }

        public static List<LocalizationMap> PredefinedLocalizationMaps = new List<LocalizationMap>
        {
            new LocalizationMap { Code = "en", Type = LanguageType.English },
            new LocalizationMap { Code = "nl", Type = LanguageType.Dutch },
            new LocalizationMap { Code = "fr", Type = LanguageType.French },
            new LocalizationMap { Code = "de", Type = LanguageType.German },
            new LocalizationMap { Code = "id", Type = LanguageType.Indonesian },
            new LocalizationMap { Code = "it", Type = LanguageType.Italian },
            new LocalizationMap { Code = "pt", Type = LanguageType.Portuguese },
            new LocalizationMap { Code = "pl", Type = LanguageType.Polish },
            new LocalizationMap { Code = "pt-br", Type = LanguageType.BrazilianPortuguese },
            new LocalizationMap { Code = "ro", Type = LanguageType.Romanian },
            new LocalizationMap { Code = "ru", Type = LanguageType.Russian },
            new LocalizationMap { Code = "es", Type = LanguageType.Spanish },
            new LocalizationMap { Code = "cs", Type = LanguageType.Czech },
            new LocalizationMap { Code = "tr", Type = LanguageType.Turkish },
            new LocalizationMap { Code = "ar", Type = LanguageType.Arabic },
            new LocalizationMap { Code = "hu", Type = LanguageType.Hungarian },
            new LocalizationMap { Code = "zh-hans", Type = LanguageType.SimplifiedChinese },
            new LocalizationMap { Code = "zh-hant", Type = LanguageType.TraditionalChinese },
            new LocalizationMap { Code = "af", Type = LanguageType.Afrikaans },
        };

        public static string GetLangCode(LanguageType type)
        {
            foreach (LocalizationMap localizationMap in PredefinedLocalizationMaps)
            {
                if (localizationMap.Type == type) return localizationMap.Code;
            }

            return "en";
        }
    }
}
