using NMS.Assistant.Domain.Configuration.Interface;

namespace NMS.Assistant.Domain.Configuration
{
    public class SendGrid: ISendGrid
    {
        public string BaseApiUrl { get; set; }
        public string ApiKey { get; set; }
    }
}
