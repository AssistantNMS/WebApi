namespace NMS.Assistant.Domain.Configuration.Interface
{
    public interface IDirectory
    {
        DirectoryItem CommunitySpotlight { get; set; }
        DirectoryItem TempDownload { get; set; }
    }
}
