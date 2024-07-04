namespace NMS.Assistant.Domain.Configuration.Interface
{
    public interface IRedis
    {
        string ConnectionString { get; set; }
        string Password { get; set; }
        string InstanceName { get; set; }
        int ConnectRetry { get; set; }
    }
}
