using NMS.Assistant.Domain.Configuration.Interface;

namespace NMS.Assistant.Domain.Configuration
{
    public class SharedDirectory : ISharedDirectory
    {
        public string GuideBasePath { get; set; }
        public string LangBasePath { get; set; }
    }
}
