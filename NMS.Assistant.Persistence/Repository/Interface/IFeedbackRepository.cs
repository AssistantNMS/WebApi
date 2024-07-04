using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Entity;

namespace NMS.Assistant.Persistence.Repository.Interface
{
    public interface IFeedbackRepository
    {
        Task<ResultWithValue<Feedback>> GetLatestFeedbackForm();
        Task<ResultWithValue<Feedback>> GetFeedbackFormFromId(Guid guid);
        Task<ResultWithValue<List<Feedback>>> GetAllFeedbackForm();
        Task<Result> ReceiveFeedback(List<FeedbackAnswer> answers);
        Task<Result> CreateFeedback(Feedback feedback);

        Task<ResultWithValue<List<FeedbackAnswer>>> GetAnswersForFeedback(Guid feedbackGuid);
        Task<ResultWithValue<List<FeedbackQuestion>>> GetQuestionsFromFeedback(Guid feedbackGuid);
    }
}