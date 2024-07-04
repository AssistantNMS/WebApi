using System;
using System.Threading.Tasks;
using NMS.Assistant.Domain.Result;

namespace NMS.Assistant.Tracker.Tracker.Abstract
{
    public abstract class ATracker
    {
        /// <summary>
        /// Introduced because of EntityFramework does not allow Async calls to DB
        /// </summary>
        public bool CanRunAsync { get; protected set; } = false;
        public int IntervalInMinutes { get; protected set; } = 5;
        public DateTime LastRun { get; protected set; }

        public async Task RunInit()
        {
            await Init();
            RunComplete();
        }

        protected virtual Task Init() => Task.FromResult(0);

        public virtual bool ShouldRun() => // Allow for 5 sec clockskew
            DateTime.Now.Add(TimeSpan.FromSeconds(5)) > LastRun.Add(TimeSpan.FromMinutes(IntervalInMinutes));

        public async Task<Result> RunCheck()
        {
            Result result = await Check();
            RunComplete();
            return result;
        }

        protected virtual Task<Result> Check() => Task.FromResult(new Result(false, "Not Implemented"));

        public void RunComplete() => LastRun = DateTime.Now;
    }
}
