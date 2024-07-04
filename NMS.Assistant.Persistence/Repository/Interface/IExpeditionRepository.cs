using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Entity;

namespace NMS.Assistant.Persistence.Repository.Interface
{
    public interface IExpeditionRepository
    {
        Task<ResultWithValue<Expedition>> GetLatest();

        Task<ResultWithValue<List<Expedition>>> GetAll();

        Task<Result> Add(Expedition addItem);

        Task<Result> Edit(Expedition editItem);

        Task<Result> Delete(Guid guid);
    }
}
