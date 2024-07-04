using NMS.Assistant.Domain.Configuration.Interface;

namespace NMS.Assistant.Domain.Configuration
{
    public class ApplicationInsights: IApplicationInsights
    {
        public bool Enabled { get; set; }
        public string ConnectionString { get; set; }
    }
}
