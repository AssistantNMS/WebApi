using NMS.Assistant.Domain.Configuration.Interface;

namespace NMS.Assistant.Domain.Configuration
{
    public class Caching : ICaching
    {
        public int ExpiryInSeconds { get; set; }
        public int PermissionsExpiryInSeconds { get; set; }
    }
}
