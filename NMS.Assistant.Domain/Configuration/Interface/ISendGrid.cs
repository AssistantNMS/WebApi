namespace NMS.Assistant.Domain.Configuration.Interface
{
    public interface ISendGrid
    {
        string BaseApiUrl { get; set; }
        string ApiKey { get; set; }
    }
}
