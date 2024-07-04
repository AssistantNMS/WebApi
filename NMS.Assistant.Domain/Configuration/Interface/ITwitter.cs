namespace NMS.Assistant.Domain.Configuration.Interface
{
    public interface ITwitter
    {
        string ConsumerApiKey { get; set; }
        string ConsumerApiSecretKey { get; set; }
        string AccessToken { get; set; }
        string AccessTokenSecret { get; set; }
    }
}
