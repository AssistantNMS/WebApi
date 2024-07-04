using NMS.Assistant.Domain.Result;
using NMS.Assistant.Integration.Contract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NMS.Assistant.Integration.Repository.Interface
{
    public interface IUptimeRobotRepository
    {
        Task<ResultWithValue<List<Monitor>>> GetNMSCDMonitors();
    }
}
