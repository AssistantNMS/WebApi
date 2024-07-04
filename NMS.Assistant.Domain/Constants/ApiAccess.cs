using System.Collections.Generic;

namespace NMS.Assistant.Domain.Constants
{
    public static class ApiAccess
    {
        public static string PublicBasic = "public-basic";
        public static string Public = "public-advanced";
        public static string Auth = "authenticated";
        public static string All = "all";

        public static List<string> HiddenControllers = new List<string>
        {
            "Donation",
            "Github",
            "Language",
            "PendingGuide",
            "Version",
            "WhatIsNew",
        };

        public static List<string> PublicBasicControllers = new List<string>
        {
            "About",
            "CommunityLink",
            "CommunityMission",
            "CommunityMissionProgress",
            "HelloGames",
            "ItemInfo",
            "Nmsfm",
        };
    }
}
