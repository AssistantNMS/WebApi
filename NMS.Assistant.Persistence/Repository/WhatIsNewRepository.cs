using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NMS.Assistant.Domain.Dto.Enum;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Entity;
using NMS.Assistant.Persistence.Helper;
using NMS.Assistant.Persistence.Repository.Interface;

namespace NMS.Assistant.Persistence.Repository
{
    public class WhatIsNewRepository : IWhatIsNewRepository
    {
        private readonly NmsAssistantContext _db;

        public WhatIsNewRepository(NmsAssistantContext db)
        {
            _db = db;
        }

        public async Task<ResultWithValue<List<WhatIsNew>>> GetAdminLatestWhatIsNew()
        {
            List<WhatIsNew> whatIsNews = await _db.WhatIsNews
                .OrderByDescending(win => win.ActiveDate)
                .ToListAsync();
            if (whatIsNews == null) return new ResultWithValue<List<WhatIsNew>>(false, new List<WhatIsNew>(), "Could not load WhatIsNews");

            return new ResultWithValue<List<WhatIsNew>>(true, whatIsNews, string.Empty);
        }

        public async Task<ResultWithValue<List<WhatIsNew>>> GetLatestWhatIsNew(WhatIsNewType filterType, int numberToFetch = 10)
        {
            Expression<Func<WhatIsNew, bool>> predicate = l => true;
            bool predicateIsEnabled = true;
            switch (filterType)
            {
                case WhatIsNewType.OnlyAndroid:
                    predicate = l => l.IsAndroid;
                    break;
                case WhatIsNewType.OnlyIos:
                    predicate = l => l.IsIos;
                    break;
                case WhatIsNewType.OnlyWebApp:
                    predicate = l => l.IsWebApp;
                    break;
                case WhatIsNewType.OnlyDiscord:
                    predicate = l => l.IsDiscord;
                    break;

                default:
                    predicateIsEnabled = false;
                    break;
            }

            List<WhatIsNew> whatIsNews = await _db.WhatIsNews
                .Where(win => DateTime.Now > win.ActiveDate)
                .AddFilterIfValue(predicateIsEnabled, predicate)
                .OrderByDescending(win => win.ActiveDate)
                .Take(numberToFetch).ToListAsync();
            if (whatIsNews == null) return new ResultWithValue<List<WhatIsNew>>(false, new List<WhatIsNew>(), "Could not load WhatIsNews");

            return new ResultWithValue<List<WhatIsNew>>(true, whatIsNews, string.Empty);
        }

        public async Task<Result> AddWhatIsNew(WhatIsNew addWhatIsNew)
        {
            try
            {
                await _db.WhatIsNews.AddAsync(addWhatIsNew);
                await _db.SaveChangesAsync();
                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

        public async Task<Result> EditWhatIsNew(WhatIsNew editWhatIsNew)
        {
            try
            {
                _db.WhatIsNews.Update(editWhatIsNew);
                await _db.SaveChangesAsync();
                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

        public async Task<Result> DeleteWhatIsNew(Guid guid)
        {
            try
            {
                WhatIsNew toDeleteWhatIsNew = await _db.WhatIsNews.FirstAsync(d => d.Guid.Equals(guid));
                if (toDeleteWhatIsNew == null) return new Result(false, "Could not find the specified Guid");

                _db.WhatIsNews.Remove(toDeleteWhatIsNew);
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
