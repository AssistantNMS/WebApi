namespace NMS.Assistant.Domain.Configuration.Interface
{
    public interface ICaching
    {
        int ExpiryInSeconds { get; set; }
        int PermissionsExpiryInSeconds { get; set; }
    }
}
