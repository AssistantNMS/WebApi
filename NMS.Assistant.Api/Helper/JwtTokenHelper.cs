using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using NMS.Assistant.Domain.Constants;
using NMS.Assistant.Domain.Result;

namespace NMS.Assistant.Api.Helper
{
    public class JwtTokenHelper
    {
        public static ResultWithValue<Guid> GetUserIdFromToken(HttpContext httpContext)
        {
            httpContext.Request.Headers.TryGetValue("Authorization", out StringValues authHeaderValue);
            string token = authHeaderValue.ToString().Replace("Bearer", "").Trim();

            if (string.IsNullOrEmpty(token)) return new ResultWithValue<Guid>(false, default, "Auth header not supplied");

            string userId = httpContext.User.FindFirst(CustomClaimTypes.UserId)?.Value;
            if (!Guid.TryParse(userId, out Guid id)) return new ResultWithValue<Guid>(false, default, "You are incorrectly logged in");

            return new ResultWithValue<Guid>(true, id, string.Empty);
        }
    }
}
