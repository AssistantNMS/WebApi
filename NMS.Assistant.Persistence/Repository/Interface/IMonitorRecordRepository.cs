using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Contract;
using NMS.Assistant.Persistence.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NMS.Assistant.Persistence.Repository.Interface
{
    public interface IMonitorRecordRepository
    {
        Task<ResultWithValue<List<MonitorRecord>>> GetAll();
        Task<ResultWithValue<List<MonitorStatusRecord>>> Search(DateTime startDate, DateTime endDate);
        Task<Result> Add(MonitorRecord addItem);
        Task<Result> Edit(MonitorRecord editItem);
        Task<Result> Delete(Guid guid);
    }
}
