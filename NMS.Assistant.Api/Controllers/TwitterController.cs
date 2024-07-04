using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NMS.Assistant.Api.Filter;
using NMS.Assistant.Domain.Dto.Enum;
using NMS.Assistant.Domain.Dto.Model;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Integration.Repository.Interface;

namespace NMS.Assistant.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class TwitterController : ControllerBase
    {
        private readonly ITwitterRepository _twitterRepo;

        public TwitterController(ITwitterRepository twitterRepo)
        {
            _twitterRepo = twitterRepo;
        }

        /// <summary>
        /// Send a Twitter message.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: TwitterTweet
        /// </remarks>
        [HttpPost]
        [Authorize]
        [RequiredPermission(PermissionType.TwitterTweet)]
        public async Task<IActionResult> Tweet(TwitterMessageViewModel vm)
        {
            Result tweetResult = await _twitterRepo.TweetMessage(vm.Message);
            if (tweetResult.HasFailed) return BadRequest(tweetResult.ExceptionMessage);

            return Ok();
        }
    }
}