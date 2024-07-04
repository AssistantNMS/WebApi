using Microsoft.AspNetCore.Mvc;

namespace NMS.Assistant.Api.Model
{
    [Microsoft.AspNetCore.Mvc.Infrastructure.DefaultStatusCode(403)]
    public class ForbiddenObjectResult : ObjectResult
    {
        public ForbiddenObjectResult(object value)
            : base(value)
        {
            StatusCode = 403;
        }
    }
}
