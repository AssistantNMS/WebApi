using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NMS.Assistant.Domain.Dto.Enum;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Entity;
using NMS.Assistant.Persistence.Repository.Interface;

namespace NMS.Assistant.Persistence.Repository
{
    public class HelloGamesHistoryRepository : IHelloGamesHistoryRepository
    {
        private readonly NmsAssistantContext _db;

        public HelloGamesHistoryRepository(NmsAssistantContext db)
        {
            _db = db;
        }

        public async Task<ResultWithValue<bool>> Exists(HelloGamesHistoryType type, string identifier)
        {
            try
            {
                List<HelloGamesHistory> history = await _db.HelloGamesHistories
                    .Where(hh => hh.Type == type && hh.Identifier.Equals(identifier))
                    .ToListAsync();
                if (history == null || history.Count == 0)
                {
                    return new ResultWithValue<bool>(true, false, "Not found");
                }
                return new ResultWithValue<bool>(true, true, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResultWithValue<bool>(false, true, ex.Message);
            }
        }

        public async Task<Result> AddHistoryItem(HelloGamesHistory history)
        {
            try
            {
                _db.HelloGamesHistories.Add(history);

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
