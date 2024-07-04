using NMS.Assistant.Domain.Configuration.Interface;

namespace NMS.Assistant.Domain.Configuration
{
    public class Firebase: IFirebase
    {
        public string PathRoRefreshToken { get; set; }
        public string ApplicationId { get; set; }
        public string ProjectId { get; set; }
    }
}
