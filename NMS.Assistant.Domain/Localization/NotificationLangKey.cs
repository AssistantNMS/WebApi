using System.Collections.Generic;

namespace NMS.Assistant.Domain.Localization
{
    public static class NotificationLangKey
    {
        public const string NewCommunityMissionTweet = "newCommunityMissionTweet";
        public const string NewCommunityMissionTitle = "newCommunityMissionTitle";
        public const string NewCommunityMissionMessage = "newCommunityMissionMessage";
        public const string NewReleaseMessage = "newReleaseMessage";
        public const string NewNewsItemMessage = "newNewsItemMessage";
        public const string newSteamNewsItemTitle = "newSteamNewsItemTitle";
        public const string newSteamDepotItemTitle = "newSteamDepotItemTitle";

        public static List<string> SupportedPushNotificationTranslations = new List<string>
        {
            "en",
            "de"
        };
    }
}
