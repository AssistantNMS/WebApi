using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using NMS.Assistant.Domain.Dto.Enum;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Integration.Contract;
using NMS.Assistant.Integration.Repository.Interface;
using NMS.Assistant.Tracker.Tracker.Abstract;

namespace NMS.Assistant.Tracker.Tracker
{
    public class NmsfmTracker : ATracker
    {
        private readonly INmsfmRepository _nmsfmRepo;
        private readonly IRedisRepository _redisRepo;
        private readonly Logger _logger;

        public NmsfmTracker(IServiceProvider serviceProvider, Logger logger)
        {
            IntervalInMinutes = 30;
            _nmsfmRepo = serviceProvider.GetService<INmsfmRepository>();
            _redisRepo = serviceProvider.GetService<IRedisRepository>();
            _logger = logger;
        }

        protected override Task Init() => GetDataFromExcel();
        protected override Task<Result> Check() => GetDataFromExcel();


        private async Task<Result> GetDataFromExcel()
        {
            ResultWithValue<List<NmsfmSheet>> result = await _nmsfmRepo.ReadTrackDataFromExcel();
            if (result.HasFailed) return result;

            await PersistToRedis(result.Value);
            return new Result(true, string.Empty);
        }

        private void LogInfo(string msg) => _logger.Info($"{GetType().Name} - {msg}");
        private void LogError(string msg) => _logger.Error($"{GetType().Name} - {msg}");

        private async Task PersistToRedis(List<NmsfmSheet> sheets)
        {
            LogInfo($"{RedisCacheType.NMSFM} - Saving to Redis");
            Result redisResult = await _redisRepo.SaveToCache(RedisCacheType.NMSFM, sheets);
            if (redisResult.HasFailed) LogError($"{RedisCacheType.NMSFM} - Could not persist to Redis");
        }
    }
}
