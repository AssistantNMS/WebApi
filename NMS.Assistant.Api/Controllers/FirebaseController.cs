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
    public class FirebaseController : ControllerBase
    {
        private readonly IFirebaseRepository _firebaseRepo;

        public FirebaseController(IFirebaseRepository firebaseRepo)
        {
            _firebaseRepo = firebaseRepo;
        }

        /// <summary>
        /// Send a Firebase Push Notification.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: FirebasePush
        /// </remarks>
        [HttpPost]
        [Authorize]
        [RequiredPermission(PermissionType.FirebasePush)]
        public async Task<ActionResult<string>> PushNotification(FirebasePushViewModel push)
        {
            ResultWithValue<string> firebaseCloudMessageResult = await _firebaseRepo.SendMessage(push.Topic, push.Title, push.Message);
            if (firebaseCloudMessageResult.HasFailed) return BadRequest(firebaseCloudMessageResult.ExceptionMessage);

            return Ok(firebaseCloudMessageResult.Value);
        }
    }
}