namespace NMS.Assistant.Domain.Configuration.Interface
{
    public interface IFirebase
    {
        string PathRoRefreshToken { get; set; }
        string ApplicationId { get; set; }
        string ProjectId { get; set; }
    }
}
