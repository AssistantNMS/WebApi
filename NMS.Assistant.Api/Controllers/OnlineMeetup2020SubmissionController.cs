using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NMS.Assistant.Api.Filter;
using NMS.Assistant.Data.Mapper.ViewModel;
using NMS.Assistant.Domain.Dto.Enum;
using NMS.Assistant.Domain.Dto.Model.OnlineMeetup;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Entity;
using NMS.Assistant.Persistence.Repository.Interface;

namespace NMS.Assistant.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class OnlineMeetup2020SubmissionController : ControllerBase
    {
        private readonly IOnlineMeetup2020SubmissionRepository _om2020Repo;

        public OnlineMeetup2020SubmissionController(IOnlineMeetup2020SubmissionRepository om2020Repo)
        {
            _om2020Repo = om2020Repo;
        }

        /// <summary>
        /// Get Submissions for the NMS Online Meetup 2020
        /// </summary>
        [HttpGet]
        [CacheFilter(CacheType.OnlineMeetup2020, includeQuery: true)]
        public async Task<ActionResult<List<OnlineMeetup2020SubmissionViewModel>>> GetAll()
        {
            ResultWithValue<List<OnlineMeetup2020Submission>> meetupResult = await _om2020Repo.GetAllSubmissions();
            if (meetupResult.HasFailed) return NoContent();

            return Ok(meetupResult.Value.ToViewModel());
        }

        /// <summary>
        /// Add NMS Online Meetup 2020 Submission.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: CommunityLinksManage
        /// </remarks>
        [HttpPost]
        [Authorize]
        [RequiredPermission(PermissionType.CommunityLinksManage)]
        public async Task<IActionResult> AddCommunityLink(OnlineMeetup2020SubmissionViewModel addSubmission)
        {
            Result addResult = await _om2020Repo.AddSubmission(addSubmission.ToDatabaseModel(Guid.NewGuid()));
            if (addResult.HasFailed) return BadRequest(addResult.ExceptionMessage);

            return Ok();
        }


        /// <summary>
        /// Edit NMS Online Meetup 2020 Submission.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: CommunityLinksView, CommunityLinksManage
        /// </remarks>
        [HttpPut]
        [Authorize]
        [RequiredPermission(PermissionType.CommunityLinksView, PermissionType.CommunityLinksManage)]
        public async Task<IActionResult> EditCommunityLink(OnlineMeetup2020SubmissionViewModel editSubmission)
        {
            Result addResult = await _om2020Repo.EditOnlineMeetup2020Submission(editSubmission.ToDatabaseModel(editSubmission.Guid));
            if (addResult.HasFailed) return BadRequest(addResult.ExceptionMessage);

            return Ok();
        }

        /// <summary>
        /// Remove NMS Online Meetup 2020 Submission.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: CommunityLinksManage
        /// </remarks>
        /// <param name="guid">
        /// Community Link Guid, available from /OnlineMeetup2020.
        /// </param>   
        [HttpDelete]
        [Authorize]
        [Route("{guid}")]
        [RequiredPermission(PermissionType.CommunityLinksManage)]
        public async Task<IActionResult> DeleteCommunityLink(Guid guid)
        {
            Result addResult = await _om2020Repo.DeleteOnlineMeetup2020Submission(guid);
            if (addResult.HasFailed) return BadRequest(addResult.ExceptionMessage);

            return Ok();
        }
    }
}
