using System.Collections.Generic;
using Newtonsoft.Json;

namespace NMS.Assistant.Integration.Contract
{
    public class SteamNewsHub
    {
        //public bool forwardComplete { get; set; }
        //public bool backwardComplete { get; set; }
        //public Document[] documents { get; set; }
        //public App[] apps { get; set; }

        [JsonProperty("events")]
        public List<Event> Events { get; set; }
    }

    public class Event
    {
        [JsonProperty("gid")]
        public string Id { get; set; }
        //public string clan_steamid { get; set; }

        [JsonProperty("event_name")]
        public string Name { get; set; }

        //public int event_type { get; set; }
        //public int appid { get; set; }
        //public string server_address { get; set; }
        //public string server_password { get; set; }

        [JsonProperty("rtime32_start_time")]
        public int PostTime { get; set; }

        //public int rtime32_end_time { get; set; }
        //public int comment_count { get; set; }
        //public string creator_steamid { get; set; }
        //public string last_update_steamid { get; set; }
        //public string event_notes { get; set; }
        //public string jsondata { get; set; }

        [JsonProperty("announcement_body")]
        public AnnouncementBody AnnouncementBody { get; set; }

        //public int published { get; set; }
        //public int hidden { get; set; }
        //public int rtime32_visibility_start { get; set; }
        //public int rtime32_visibility_end { get; set; }
        //public int broadcaster_accountid { get; set; }
        //public int follower_count { get; set; }
        //public int ignore_count { get; set; }
        //public string forum_topic_id { get; set; }
        //public int rtime32_last_modified { get; set; }
        //public string news_post_gid { get; set; }
        //public int rtime_mod_reviewed { get; set; }
        //public int featured_app_tagid { get; set; }

        [JsonProperty("votes_up")]
        public int UpVotes { get; set; }

        [JsonProperty("votes_down")]
        public int DownVotes { get; set; }

        //public string comment_type { get; set; }
        //public string gidfeature { get; set; }
        //public string gidfeature2 { get; set; }
        //public string clan_steamid_original { get; set; }

        [JsonProperty("video_preview_type")]
        public string VideoType { get; set; }

        [JsonProperty("video_preview_id")]
        public string VideoPreviewId { get; set; }
    }

    public class AnnouncementBody
    {
        //public string gid { get; set; }
        //public string clanid { get; set; }
        //public string posterid { get; set; }

        [JsonProperty("headline")]
        public string Headline { get; set; }

        //public int posttime { get; set; }
        //public int updatetime { get; set; }

        [JsonProperty("body")]
        public string Markdown { get; set; }

        [JsonProperty("commentcount")]
        public int CommentCount { get; set; }

        //public string[] tags { get; set; }
        //public int language { get; set; }
        //public int hidden { get; set; }
        //public string forum_topic_id { get; set; }
        //public string event_gid { get; set; }
        //public int voteupcount { get; set; }
        //public int votedowncount { get; set; }
    }
}

