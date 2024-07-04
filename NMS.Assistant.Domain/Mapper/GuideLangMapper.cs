using System.Collections.Generic;

namespace NMS.Assistant.Domain.Mapper
{
    public static class GuideLangMapper
    {
        public static List<string> RenameGuideNames(this List<string> guides)
        {
            List<string> result = new List<string>();
            foreach (string guide in guides)
            {
                if (guide.Equals("guide.af")) result.Add("Afrikaans");
                if (guide.Equals("guide.ar")) result.Add("Arabic");
                if (guide.Equals("guide.de")) result.Add("German");
                if (guide.Equals("guide.en")) result.Add("English");
                if (guide.Equals("guide.es")) result.Add("Spanish");
                if (guide.Equals("guide.fr")) result.Add("French");
                if (guide.Equals("guide.hu")) result.Add("Hungarian");
                if (guide.Equals("guide.it")) result.Add("Italian");
                if (guide.Equals("guide.nl")) result.Add("Dutch");
                if (guide.Equals("guide.pl")) result.Add("Polish");
                if (guide.Equals("guide.pt")) result.Add("Portuguese");
                if (guide.Equals("guide.pt-br")) result.Add("Brazilian Portuguese");
                if (guide.Equals("guide.ro")) result.Add("Romanian");
                if (guide.Equals("guide.ru")) result.Add("Russian");
                if (guide.Equals("guide.zh-hans")) result.Add("Chinese Simplified");
                if (guide.Equals("guide.zh-hant")) result.Add("Chinese Traditional");
            }

            return result;
        }
    }
}
