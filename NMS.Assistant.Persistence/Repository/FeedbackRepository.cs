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
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly NmsAssistantContext _db;

        public FeedbackRepository(NmsAssistantContext db)
        {
            _db = db;
        }

        public async Task<ResultWithValue<Feedback>> GetLatestFeedbackForm()
        {
            Feedback latestFeedback = await _db.Feedbacks.OrderByDescending(f => f.Created).FirstOrDefaultAsync();
            if (latestFeedback == null) return new ResultWithValue<Feedback>(false, new Feedback(), "Could not load Feedback");

            return new ResultWithValue<Feedback>(true, latestFeedback, string.Empty);
        }

        public async Task<ResultWithValue<Feedback>> GetFeedbackFormFromId(Guid guid)
        {
            Feedback specificFeedback = await _db.Feedbacks.Where(f => f.Guid.Equals(guid)).FirstOrDefaultAsync();
            if (specificFeedback == null) return new ResultWithValue<Feedback>(false, new Feedback(), "Could not load specific Feedback");

            return new ResultWithValue<Feedback>(true, specificFeedback, string.Empty);
        }

        public async Task<ResultWithValue<List<Feedback>>> GetAllFeedbackForm()
        {
            List<Feedback> feedback = await _db.Feedbacks.OrderByDescending(f => f.Created).ToListAsync();
            if (feedback == null) return new ResultWithValue<List<Feedback>>(false, new List<Feedback>(), "Could not load Feedback");

            return new ResultWithValue<List<Feedback>>(true, feedback, string.Empty);
        }

        public async Task<Result> ReceiveFeedback(List<FeedbackAnswer> answers)
        {
            try
            {
                await _db.FeedbackAnswers.AddRangeAsync(answers);
                await _db.SaveChangesAsync();
                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

        public async Task<Result> CreateFeedback(Feedback feedback)
        {
            try
            {
                await _db.Feedbacks.AddRangeAsync(feedback);
                await _db.SaveChangesAsync();
                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }



        public async Task<ResultWithValue<List<FeedbackAnswer>>> GetAnswersForFeedback(Guid feedbackGuid)
        {
            List<FeedbackAnswer> feedbackAnswers = await _db.FeedbackAnswers
                .Where(fa => fa.FeedbackGuid.Equals(feedbackGuid))
                .OrderByDescending(f => f.DateAnswered)
                .ToListAsync();
            if (feedbackAnswers == null || feedbackAnswers.Count <= 0) return new ResultWithValue<List<FeedbackAnswer>>(false, new List<FeedbackAnswer>(), "Could not load Feedback answers");

            return new ResultWithValue<List<FeedbackAnswer>>(true, feedbackAnswers, string.Empty);
        }

        public async Task<ResultWithValue<List<FeedbackQuestion>>> GetQuestionsFromFeedback(Guid feedbackGuid)
        {
            List<FeedbackQuestion> feedbackQuestions = await _db.FeedbackQuestions
                .Where(fa => fa.FeedbackGuid.Equals(feedbackGuid))
                .ToListAsync();
            if (feedbackQuestions == null || feedbackQuestions.Count <= 0) return new ResultWithValue<List<FeedbackQuestion>>(false, new List<FeedbackQuestion>(), "Could not load Feedback answers");

            return new ResultWithValue<List<FeedbackQuestion>>(true, feedbackQuestions, string.Empty);
        }
    }
}
