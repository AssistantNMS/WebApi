namespace NMS.Assistant.Domain.Configuration.Interface
{
    public interface IDatabase
    {
        string ConnectionString { get; set; }
        int CommandTimeout { get; set; }
        int PageSize { get; set; }
    }
}
