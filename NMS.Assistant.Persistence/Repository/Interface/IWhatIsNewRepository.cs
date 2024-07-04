using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NMS.Assistant.Domain.Dto.Enum;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Entity;

namespace NMS.Assistant.Persistence.Repository.Interface
{
    public interface IWhatIsNewRepository
    {
        Task<ResultWithValue<List<WhatIsNew>>> GetAdminLatestWhatIsNew();
        Task<ResultWithValue<List<WhatIsNew>>> GetLatestWhatIsNew(WhatIsNewType filterType, int numberToFetch = 10);
        Task<Result> AddWhatIsNew(WhatIsNew addWhatIsNew);
        Task<Result> EditWhatIsNew(WhatIsNew editWhatIsNew);
        Task<Result> DeleteWhatIsNew(Guid guid);
    }
}