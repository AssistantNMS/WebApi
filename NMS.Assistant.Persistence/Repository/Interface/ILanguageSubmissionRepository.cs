using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Entity;

namespace NMS.Assistant.Persistence.Repository.Interface
{
    public interface ILanguageSubmissionRepository
    {
        Task<ResultWithValue<List<LanguageSubmission>>> GetAllSubmittedLanguages();
        Task<ResultWithValue<LanguageSubmission>> GetSubmittedLanguage(Guid guid);
        Task<Result> AddLanguageSubmission(LanguageSubmission addLanguageSubmission);
        Task<Result> DeleteLanguageSubmission(Guid guid);
    }
}