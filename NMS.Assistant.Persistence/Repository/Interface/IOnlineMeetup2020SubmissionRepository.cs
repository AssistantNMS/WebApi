using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Entity;

namespace NMS.Assistant.Persistence.Repository.Interface
{
    public interface IOnlineMeetup2020SubmissionRepository
    {
        Task<ResultWithValue<List<OnlineMeetup2020Submission>>> GetAllSubmissions();
        Task<Result> AddSubmission(OnlineMeetup2020Submission addOnlineMeetup2020Submission);
        Task<Result> EditOnlineMeetup2020Submission(OnlineMeetup2020Submission editOnlineMeetup2020Submission);
        Task<Result> DeleteOnlineMeetup2020Submission(Guid guid);
    }
}