using NMS.Assistant.Domain.Configuration.Interface;

namespace NMS.Assistant.Domain.Configuration
{
    public class Twitter: ITwitter
    {
        public string ConsumerApiKey { get; set; }
        public string ConsumerApiSecretKey { get; set; }
        public string AccessToken { get; set; }
        public string AccessTokenSecret { get; set; }
    }
}
