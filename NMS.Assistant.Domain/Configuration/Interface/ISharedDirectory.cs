namespace NMS.Assistant.Domain.Configuration.Interface
{
    public interface ISharedDirectory
    {
        string GuideBasePath { get; set; }
        string LangBasePath { get; set; }
    }
}
