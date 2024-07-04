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
    public class LanguageSubmissionRepository : ILanguageSubmissionRepository
    {
        private readonly NmsAssistantContext _db;

        public LanguageSubmissionRepository(NmsAssistantContext db)
        {
            _db = db;
        }

        public async Task<ResultWithValue<List<LanguageSubmission>>> GetAllSubmittedLanguages()
        {
            List<LanguageSubmission> languageSubmissions = await _db.LanguageSubmissions.OrderBy(f => f.DateSubmitted).ToListAsync();
            if (languageSubmissions == null) return new ResultWithValue<List<LanguageSubmission>>(false, new List<LanguageSubmission>(), "Could not load LanguageSubmissions");

            return new ResultWithValue<List<LanguageSubmission>>(true, languageSubmissions, string.Empty);
        }

        public async Task<ResultWithValue<LanguageSubmission>> GetSubmittedLanguage(Guid guid)
        {
            try
            {
                LanguageSubmission user = await _db.LanguageSubmissions.FirstAsync(u => u.Guid.Equals(guid));
                return new ResultWithValue<LanguageSubmission>(true, user, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResultWithValue<LanguageSubmission>(false, new LanguageSubmission(), ex.Message);
            }
        }
        
        public async Task<Result> AddLanguageSubmission(LanguageSubmission addLanguageSubmission)
        {
            try
            {
                await _db.LanguageSubmissions.AddAsync(addLanguageSubmission);
                await _db.SaveChangesAsync();
                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

        public async Task<Result> DeleteLanguageSubmission(Guid guid)
        {
            try
            {
                LanguageSubmission toDelete = await _db.LanguageSubmissions.FirstAsync(d => d.Guid.Equals(guid));
                if (toDelete == null) return new Result(false, "Could not find the specified LanguageSubmission");

                _db.LanguageSubmissions.Remove(toDelete);
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
