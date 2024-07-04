using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NMS.Assistant.Api.Filter;
using NMS.Assistant.Domain.Dto.Enum;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Entity;
using NMS.Assistant.Persistence.Repository.Interface;

namespace NMS.Assistant.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class DashboardController : ControllerBase
    {
        private readonly IDonationRepository _donationRepo;
        private readonly IFeedbackRepository _feedbackRepo;
        private readonly IContributorRepository _contributorRepo;
        private readonly ILanguageSubmissionRepository _langSubmissionRepo;
        private readonly IWhatIsNewRepository _whatIsNewRepo;
        private readonly IFriendCodeRepository _friendCodeRepo;

        public DashboardController(IDonationRepository donationRepo, IFeedbackRepository feedbackRepo, IContributorRepository contributorRepo, IWhatIsNewRepository whatIsNewRepo, ILanguageSubmissionRepository langSubmissionRepo, IFriendCodeRepository friendCodeRepo)
        {
            _donationRepo = donationRepo;
            _feedbackRepo = feedbackRepo;
            _contributorRepo = contributorRepo;
            _whatIsNewRepo = whatIsNewRepo;
            _langSubmissionRepo = langSubmissionRepo;
            _friendCodeRepo = friendCodeRepo;
        }

        /// <summary>
        /// Get Number of Donations.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: DonationsView
        /// </remarks>
        [Authorize]
        [HttpGet("Donation")]
        [RequiredPermission(PermissionType.DonationsView)]
        [CacheFilter(CacheType.Dashboard, includeUrl: true)]
        public async Task<ActionResult<int>> GetAllDonationsCount()
        {
            ResultWithValue<int> donationsResult = await _donationRepo.NumberOfDonations();
            if (donationsResult.HasFailed) return NoContent();

            return Ok(donationsResult.Value);
        }

        /// <summary>
        /// Get Number of Contributors.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: ContributorsView
        /// </remarks>
        [Authorize]
        [HttpGet("Contributor")]
        [RequiredPermission(PermissionType.ContributorsView)]
        [CacheFilter(CacheType.Dashboard, includeUrl: true)]
        public async Task<ActionResult<int>> GetAllContributorsCount()
        {
            ResultWithValue<int> contributorsResult = await _contributorRepo.NumberOfContributors();
            if (contributorsResult.HasFailed) return NoContent();

            return Ok(contributorsResult.Value);
        }

        /// <summary>
        /// Get Number of Feedback Forms.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: FeedbackFormView
        /// </remarks>
        [Authorize]
        [HttpGet("FeedbackForms")]
        [RequiredPermission(PermissionType.FeedbackFormView)]
        [CacheFilter(CacheType.Dashboard, includeUrl: true)]
        public async Task<ActionResult<int>> GetFeedbackForms()
        {
            ResultWithValue<List<Feedback>> allFeedbackFormsResult = await _feedbackRepo.GetAllFeedbackForm();
            if (allFeedbackFormsResult.HasFailed) return NoContent();

            return Ok(allFeedbackFormsResult.Value.Count);
        }

        /// <summary>
        /// Get Number of Answers to the latest Feedback Form.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: FeedbackFormView
        /// </remarks>
        [Authorize]
        [HttpGet("FeedbackAnswers")]
        [RequiredPermission(PermissionType.FeedbackFormView)]
        public async Task<ActionResult<int>> GetFeedbackAnswers()
        {
            ResultWithValue<Feedback> latestFeedbackFormsResult = await _feedbackRepo.GetLatestFeedbackForm();
            if (latestFeedbackFormsResult.HasFailed) return 0;

            return Ok(latestFeedbackFormsResult.Value.Answers.Count);
        }

        /// <summary>
        /// Get Number of Language Submissions.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: LanguageSubmissionView
        /// </remarks>
        [Authorize]
        [HttpGet("Language")]
        [RequiredPermission(PermissionType.LanguageSubmissionView)]
        [CacheFilter(CacheType.Dashboard, includeUrl: true)]
        public async Task<ActionResult<int>> GetNumLanguageSubmissions()
        {
            ResultWithValue<List<LanguageSubmission>> langSubResult = await _langSubmissionRepo.GetAllSubmittedLanguages();
            if (langSubResult.HasFailed) return NoContent();

            return Ok(langSubResult.Value.Count);
        }

        /// <summary>
        /// Get Number of What is New items.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: FeedbackFormView
        /// </remarks>
        [Authorize]
        [HttpGet("WhatIsNew")]
        [RequiredPermission(PermissionType.FeedbackFormView)]
        [CacheFilter(CacheType.Dashboard, includeUrl: true)]
        public async Task<ActionResult<int>> GetNumWhatIsNew()
        {
            ResultWithValue<List<WhatIsNew>> whatIsNewResult = await _whatIsNewRepo.GetLatestWhatIsNew(WhatIsNewType.Any, int.MaxValue);
            if (whatIsNewResult.HasFailed) return NoContent();

            return Ok(whatIsNewResult.Value.Count(win => win.ActiveDate < DateTime.Now));
        }

        /// <summary>
        /// Get Number of Friend Codes.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: FeedbackFormView
        /// </remarks>
        [Authorize]
        [HttpGet("FriendCode")]
        [RequiredPermission(PermissionType.FriendCodesView)]
        [CacheFilter(CacheType.Dashboard, includeUrl: true)]
        public async Task<ActionResult<int>> GetNumOfLiveFriendCodes()
        {
            ResultWithValue<List<FriendCode>> friendCodeResult = await _friendCodeRepo.GetAllFriendCodes(true, true, true, true, true, true);
            if (friendCodeResult.HasFailed) return NoContent();

            return Ok(friendCodeResult.Value.Count);
        }

        /// <summary>
        /// Get Number of Pending Friend Codes.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: FeedbackFormView
        /// </remarks>
        [Authorize]
        [HttpGet("PendingFriendCode")]
        [RequiredPermission(PermissionType.FriendCodesView)]
        [CacheFilter(CacheType.Dashboard, includeUrl: true)]
        public async Task<ActionResult<int>> GetNumOfPendingFriendCodes()
        {
            ResultWithValue<List<FriendCode>> friendCodeResult = await _friendCodeRepo.GetAllPendingFriendCodes(true, true, true, true, true, true);
            if (friendCodeResult.HasFailed) return NoContent();

            return Ok(friendCodeResult.Value.Count);
        }
    }
}