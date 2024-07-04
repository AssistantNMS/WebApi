using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Entity;
using NMS.Assistant.Persistence.Repository.Interface;

namespace NMS.Assistant.Persistence.Repository
{
    public class ExpeditionRepository : IExpeditionRepository
    {
        private readonly NmsAssistantContext _db;

        public ExpeditionRepository(NmsAssistantContext db)
        {
            _db = db;
        }

        public async Task<ResultWithValue<Expedition>> GetLatest()
        {
            try
            {
                Expedition latest = await _db.Expeditions.OrderByDescending(e => e.StartDate).FirstOrDefaultAsync();
                return new ResultWithValue<Expedition>(latest != null, latest, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResultWithValue<Expedition>(false, new Expedition(), ex.Message);
            }
        }

        public async Task<ResultWithValue<List<Expedition>>> GetAll()
        {
            try
            {
                List<Expedition> items = await _db.Expeditions.OrderByDescending(e => e.StartDate).ToListAsync();
                return new ResultWithValue<List<Expedition>>(true, items, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResultWithValue<List<Expedition>>(false, new List<Expedition>(), ex.Message);
            }
        }

        public async Task<Result> Add(Expedition addItem)
        {
            try
            {
                await _db.Expeditions.AddAsync(addItem);
                await _db.SaveChangesAsync();
                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

        public async Task<Result> Edit(Expedition editItem)
        {
            try
            {
                _db.Expeditions.Update(editItem);
                await _db.SaveChangesAsync();
                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

        public async Task<Result> Delete(Guid guid)
        {
            try
            {
                Expedition itemToDelete = await _db.Expeditions.FirstAsync(d => d.Guid.Equals(guid));
                if (itemToDelete == null) return new Result(false, "Could not find the specified Guid");

                _db.Expeditions.Remove(itemToDelete);
                await _db.SaveChangesAsync();
                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }
    }
}
