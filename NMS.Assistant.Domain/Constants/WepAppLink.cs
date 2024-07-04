namespace NMS.Assistant.Domain.Constants
{
    public class WepAppLink
    {
        public const string LanguageKey = "{lang}";
        public const string AppIdKey = "{appId}";
        public const string CatalogueRoute = "https://app.nmsassistant.com/link/" + LanguageKey + "/" + AppIdKey + ".html";
        public const string ProcessorRoute = "https://app.nmsassistant.com/processor/" + AppIdKey;
        public const string ImagesRoute = "https://app.nmsassistant.com/assets/images/";
    }
}
