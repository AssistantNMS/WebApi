using System.Collections.Generic;
using System.Threading.Tasks;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Integration.Contract;

namespace NMS.Assistant.Integration.Repository.Interface
{
    public interface INmsfmRepository
    {
        public Task<ResultWithValue<List<NmsfmSheet>>> ReadTrackDataFromExcel();
    }
}