using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NMS.Assistant.Api.Filter;
using NMS.Assistant.Domain.Dto.Enum;
using NMS.Assistant.Domain.Dto.Model.Feedback.Filled;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Entity;
using NMS.Assistant.Persistence.Repository.Interface;

namespace NMS.Assistant.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class FeedbackAnswerController : ControllerBase
    {
        private readonly IFeedbackRepository _feedbackRepo;

        public FeedbackAnswerController(IFeedbackRepository feedbackRepo)
        {
            _feedbackRepo = feedbackRepo;
        }

        /// <summary>
        /// Get Answers to a specific Feedback.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: DonationsView
        /// </remarks>
        /// <param name="feedbackGuid">
        /// Feedback Guid, available from /Feedback/Admin.
        /// </param>  
        [HttpGet]
        [Authorize]
        [Route("{feedbackGuid}")]
        [RequiredPermission(PermissionType.FeedbackFormView)]
        public async Task<ActionResult<FeedbackWithQuestionsAndAnswersPerUserViewModel>> FeedbackAnswers(Guid feedbackGuid)
        {
            ResultWithValue<List<FeedbackQuestion>> feedbackQuestionsResult = await _feedbackRepo.GetQuestionsFromFeedback(feedbackGuid);
            if (feedbackQuestionsResult.HasFailed) return BadRequest("Error fetching Questions");

            ResultWithValue<List<FeedbackAnswer>> feedbackAnswersResult = await _feedbackRepo.GetAnswersForFeedback(feedbackGuid);
            if (feedbackAnswersResult.HasFailed) return BadRequest("Error fetching Answers");

            List<FeedbackAnswersFromUserViewModel> usersAnswersList = new List<FeedbackAnswersFromUserViewModel>();

            List<IGrouping<Guid, FeedbackAnswer>> groupedByUserGuid = feedbackAnswersResult.Value.GroupBy(fa => fa.AnonymousUserGuid).ToList();
            foreach (IGrouping<Guid, FeedbackAnswer> usersAnswersGroup in groupedByUserGuid)
            {
                List<FeedbackQuestionAndAnswerViewModel> responses = new List<FeedbackQuestionAndAnswerViewModel>();

                foreach (FeedbackAnswer answer in usersAnswersGroup)
                {
                    FeedbackQuestion fbQuestion = feedbackQuestionsResult.Value.FirstOrDefault(fq => fq.Guid.Equals(answer.FeedbackQuestionGuid));
                    if (fbQuestion == null) continue;

                    responses.Add(new FeedbackQuestionAndAnswerViewModel
                    {
                        Question = fbQuestion.Question,
                        QuestionType = fbQuestion.Type,
                        Answer = answer.Answer
                    });
                }

                usersAnswersList.Add(new FeedbackAnswersFromUserViewModel
                {
                    UserGuid = usersAnswersGroup.Key,
                    AnswerDate = usersAnswersGroup.First().DateAnswered,
                    Responses = responses,
                    AppType = usersAnswersGroup.First().AppType
                });
            }

            ResultWithValue<Feedback> feedbackFormResult = await _feedbackRepo.GetFeedbackFormFromId(feedbackGuid);
            if (feedbackFormResult.HasFailed) return BadRequest("Error fetching FeedbackForm");
            FeedbackWithQuestionsAndAnswersPerUserViewModel result = new FeedbackWithQuestionsAndAnswersPerUserViewModel
            {
                Name = feedbackFormResult.Value.Name,
                CreatedOn = feedbackFormResult.Value.Created,
                QuestionsAndAnswersPerUser = usersAnswersList
            };
            return Ok(result);
        }
    }
}