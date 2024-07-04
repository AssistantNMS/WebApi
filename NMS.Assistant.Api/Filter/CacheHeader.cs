using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NMS.Assistant.Api.Helper;
using NMS.Assistant.Data.Helper;

namespace NMS.Assistant.Api.Filter
{
    public class CacheHeaderAttribute : TypeFilterAttribute
    {
        public CacheHeaderAttribute(int minutes = 0, int hours = 0, int days = 0, int months = 0) : base(typeof(CacheHeaderFilter))
        {
            int total = SecondsHelper.FromMinutes(minutes) +
                        SecondsHelper.FromHours(hours) +
                        SecondsHelper.FromDays(days) +
                        SecondsHelper.FromMonths(months);

            Arguments = new object[] { total };
        }
    }

    public class CacheHeaderFilter : IActionFilter
    {
        private readonly int? _secondsToLive;

        public CacheHeaderFilter(int total = 0)
        {
            if (total > 0) _secondsToLive = total;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            context.HttpContext.Response.ApplyCacheHeader(_secondsToLive);
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Nothing
        }
    }
}
