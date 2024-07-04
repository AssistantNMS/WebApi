using System.Collections.Generic;
using System.Linq;

namespace NMS.Assistant.Data.Helper
{
    public static class TwitterHelper
    {
        public static List<string> CommonHashTags = new List<string> {
            "NoMansSky", "NoMansSkyEchoes", "AssistantNMS", "AssistantApps"
        };

        public static string CommonHashTagsString = string.Join(" ", CommonHashTags.Select(text => $"#{text}").ToList());
    }
}
