using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NMS.Assistant.Data.Mapper.ViewModel;
using NMS.Assistant.Data.Service.Interface;
using NMS.Assistant.Domain.Configuration.Interface;
using NMS.Assistant.Domain.Contract;
using NMS.Assistant.Domain.Result;

namespace NMS.Assistant.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IApiConfiguration _appConfig;
        private readonly IUserService _userService;

        public AuthController(IApiConfiguration appConfig, IUserService userService)
        {
            _userService = userService;
            _appConfig = appConfig;
        }
        
        /// <summary>
        /// Get JWT token on successful login.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AuthenticateViaBasic()
        {
            string authHeaderValue;
            string authRequest = Request.Headers["Authorization"];
            if (authRequest != null && authRequest.StartsWith("Basic"))
            {
                authHeaderValue = authRequest.Replace("Basic", "").Trim();
            }
            else
            {
                const string message = "Basic Auth header not present";
                return StatusCode(400, message);
            }
            if (string.IsNullOrEmpty(authHeaderValue))
            {
                const string message = "Basic Auth header not present";
                return StatusCode(400, message);
            }
            authHeaderValue = Encoding.Default.GetString(Convert.FromBase64String(authHeaderValue));
            string emailPassword = authHeaderValue;
            int separatorIndex = emailPassword.IndexOf(':');

            if (separatorIndex <= 0)
            {
                const string message = "Email/Password combination is not in the correct format";
                return StatusCode(400, message);
            }

            string username = emailPassword.Substring(0, separatorIndex);
            string password = emailPassword.Substring(separatorIndex + 1);

            return await Authenticate(username, password);
        }

        private async Task<IActionResult> Authenticate(string username, string password)
        {
            ResultWithValue<UserWithToken> loginResult = await _userService.UserLoginAndGetJwtToken(username, password);
            if (loginResult.HasFailed)
            {
                const string message = "Email/Password combination does not match our records";
                return Unauthorized(message);
            }

            Response.Headers.Add("Token", loginResult.Value.Token);
            Response.Headers.Add("TokenExpiry", _appConfig.Jwt.TimeValidInSeconds.ToString());

            return Ok(loginResult.Value.ToViewModel());
        }
    }
}