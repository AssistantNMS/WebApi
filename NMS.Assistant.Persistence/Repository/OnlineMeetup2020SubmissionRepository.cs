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
    public class OnlineMeetup2020SubmissionRepository : IOnlineMeetup2020SubmissionRepository
    {
        private readonly NmsAssistantContext _db;

        public OnlineMeetup2020SubmissionRepository(NmsAssistantContext db)
        {
            _db = db;
        }

        public async Task<ResultWithValue<List<OnlineMeetup2020Submission>>> GetAllSubmissions()
        {
            List<OnlineMeetup2020Submission> onlineMeetup2020Submissions = await _db.OnlineMeetup2020Submissions.OrderBy(f => f.SortRank).ToListAsync();
            if (onlineMeetup2020Submissions == null) return new ResultWithValue<List<OnlineMeetup2020Submission>>(false, new List<OnlineMeetup2020Submission>(), "Could not load CommunityLinks");

            return new ResultWithValue<List<OnlineMeetup2020Submission>>(true, onlineMeetup2020Submissions, string.Empty);
        }

        public async Task<Result> AddSubmission(OnlineMeetup2020Submission addOnlineMeetup2020Submission)
        {
            try
            {
                await _db.OnlineMeetup2020Submissions.AddAsync(addOnlineMeetup2020Submission);
                await _db.SaveChangesAsync();
                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

        public async Task<Result> EditOnlineMeetup2020Submission(OnlineMeetup2020Submission editOnlineMeetup2020Submission)
        {
            try
            {
                _db.OnlineMeetup2020Submissions.Update(editOnlineMeetup2020Submission);
                await _db.SaveChangesAsync();
                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

        public async Task<Result> DeleteOnlineMeetup2020Submission(Guid guid)
        {
            try
            {
                OnlineMeetup2020Submission toOnlineMeetup2020Submission = await _db.OnlineMeetup2020Submissions.FirstAsync(d => d.Guid.Equals(guid));
                if (toOnlineMeetup2020Submission == null) return new Result(false, "Could not find the specified Guid");

                _db.OnlineMeetup2020Submissions.Remove(toOnlineMeetup2020Submission);
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
