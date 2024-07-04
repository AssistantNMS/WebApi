using NMS.Assistant.Domain.Configuration.Interface;

namespace NMS.Assistant.Domain.Configuration
{
    public class Database: IDatabase
    {
        public string ConnectionString { get; set; }
        public int CommandTimeout { get; set; }
        public int PageSize { get; set; }
    }
}
