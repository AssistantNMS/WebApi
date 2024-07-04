namespace NMS.Assistant.Domain.Configuration.Interface
{
    public interface IApplicationInsights
    {
        bool Enabled { get; set; }
        string ConnectionString { get; set; }
    }
}
