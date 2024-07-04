using NMS.Assistant.Domain.Configuration.Interface;

namespace NMS.Assistant.Domain.Configuration
{
    public class Redis : IRedis
    {
        public string ConnectionString { get; set; }
        public string Password { get; set; }
        public string InstanceName { get; set; }
        public int ConnectRetry { get; set; }
    }
}
