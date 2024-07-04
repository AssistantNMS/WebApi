using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NMS.Assistant.Api.Filter;
using NMS.Assistant.Data.Helper;
using NMS.Assistant.Data.Mapper.ViewModel;
using NMS.Assistant.Domain.Dto.Enum;
using NMS.Assistant.Domain.Dto.Model.Feedback;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Integration.Repository.Interface;
using NMS.Assistant.Persistence.Entity;
using NMS.Assistant.Persistence.Repository.Interface;

namespace NMS.Assistant.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class FeedbackController : ControllerBase
    {
        private readonly ISlackRepository _slackRepo;
        private readonly IFeedbackRepository _feedbackRepo;

        public FeedbackController(IFeedbackRepository feedbackRepo, ISlackRepository slackRepo)
        {
            _feedbackRepo = feedbackRepo;
            _slackRepo = slackRepo;
        }

        /// <summary>
        /// Get Latest Feedback form.
        /// </summary>
        [HttpGet]
        [CacheFilter(CacheType.Feedback)]
        public async Task<ActionResult<FeedbackViewModel>> FeedbackQuestions()
        {
            ResultWithValue<Feedback> latestFeedbackResult = await _feedbackRepo.GetLatestFeedbackForm();
            if (latestFeedbackResult.HasFailed) return NoContent();

            return Ok(latestFeedbackResult.Value.ToViewModel());
        }

        /// <summary>
        /// Submit Feedback.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> ReceiveFeedback(FeedbackAnsweredViewModel feedbackResponse)
        {
            List<FeedbackAnswer> answers = feedbackResponse.ToDatabaseModel();
            Result feedbackResult = await _feedbackRepo.ReceiveFeedback(answers);
            if (feedbackResult.HasFailed) return BadRequest(feedbackResult.ExceptionMessage);

            ResultWithValue<Feedback> feedbackFormAnswered = await _feedbackRepo.GetFeedbackFormFromId(feedbackResponse.FeedbackGuid);
            if (feedbackFormAnswered.HasFailed) return Ok();

            string msg = SlackMessageHelper.NewFeedbackReceived(feedbackFormAnswered.Value, feedbackResponse);
            await _slackRepo.SendMessageToFeedbackChannels(msg);

            return Ok();
        }

        /// <summary>
        /// Get All Feedback forms.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: FeedbackFormView
        /// </remarks>
        [HttpGet]
        [Authorize]
        [Route("Admin")]
        [RequiredPermission(PermissionType.FeedbackFormView)]
        public async Task<ActionResult<List<FeedbackWithNumResponsesViewModel>>> GetAllFeedback()
        {
            ResultWithValue<List<Feedback>> latestFeedbackResult = await _feedbackRepo.GetAllFeedbackForm();
            if (latestFeedbackResult.HasFailed) return NoContent();

            return Ok(latestFeedbackResult.Value.ToFeedbackWithNumResponsesViewModel());
        }

        /// <summary>
        /// Create a Feedback form.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: FeedbackFormManage
        /// </remarks>
        [HttpPost]
        [Authorize]
        [Route("Admin")]
        [RequiredPermission(PermissionType.FeedbackFormManage)]
        public async Task<IActionResult> CreateFeedback(AddFeedbackViewModel createFeedback)
        {
            Result result = await _feedbackRepo.CreateFeedback(createFeedback.ToDatabase());
            if (result.HasFailed) return BadRequest(result.ExceptionMessage);

            return Ok();
        }
    }
}