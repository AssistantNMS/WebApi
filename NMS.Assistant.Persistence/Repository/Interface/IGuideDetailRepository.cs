using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NMS.Assistant.Domain.Dto.Enum;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Contract;
using NMS.Assistant.Persistence.Entity;

namespace NMS.Assistant.Persistence.Repository.Interface
{
    public interface IGuideDetailRepository
    {
        Task<ResultWithValue<List<GuideDetailWithMeta>>> GetGuideDetails();
        Task<ResultWithValue<List<GuideDetailWithMeta>>> GetActiveGuideDetails(LanguageType lang);
        Task<ResultWithValue<GuideDetailWithMeta>> GetGuideDetail(Guid guid, LanguageType lang);
        Task<Result> CreateGuideDetail(Guid guideMetaGuid, GuideDetail guideDetail, LanguageType lang);
        Task<Result> UpdateGuideDetail(GuideDetail guideDetail, LanguageType lang);
        Task<Result> DeleteGuideDetail(Guid guid);
    }
}