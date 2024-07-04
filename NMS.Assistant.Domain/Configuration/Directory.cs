using NMS.Assistant.Domain.Configuration.Interface;

namespace NMS.Assistant.Domain.Configuration
{
    public class Directory: IDirectory
    {
        public DirectoryItem CommunitySpotlight { get; set; }
        public DirectoryItem TempDownload { get; set; }
    }

    public class DirectoryItem
    {
        public string DiskPath { get; set; }
        public string WebPath { get; set; }
    }
}
