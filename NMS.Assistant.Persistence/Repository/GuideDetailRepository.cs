using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using NMS.Assistant.Domain.Dto.Enum;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Contract;
using NMS.Assistant.Persistence.Entity;
using NMS.Assistant.Persistence.Repository.Interface;

namespace NMS.Assistant.Persistence.Repository
{
    public class GuideDetailRepository : IGuideDetailRepository
    {
        private readonly NmsAssistantContext _db;

        public GuideDetailRepository(NmsAssistantContext db)
        {
            _db = db;
        }

        public async Task<ResultWithValue<List<GuideDetailWithMeta>>> GetGuideDetails()
        {
            List<GuideMetaGuideDetail> guideMetaGuideDetails = await _db.GuideMetaDatas
                .SelectMany(gm => gm.GuideMetaGuideDetails).ToListAsync();
            List<GuideDetailWithMeta> guideDetails = guideMetaGuideDetails
                .Select(GuideDetailWithMeta.Combine)
                .ToList();

            if (!guideDetails.Any()) return new ResultWithValue<List<GuideDetailWithMeta>>(false, new List<GuideDetailWithMeta>(), "Could not load GuideDetails");

            return new ResultWithValue<List<GuideDetailWithMeta>>(true, guideDetails, string.Empty);
        }

        public async Task<ResultWithValue<List<GuideDetailWithMeta>>> GetActiveGuideDetails(LanguageType lang)
        {
            List<GuideMetaGuideDetail> guideMetaGuideDetails = await _db.GuideMetaDatas
                //.Where(gm => gm.Status == GuideStatusType.Live)
                .SelectMany(gm => gm.GuideMetaGuideDetails).ToListAsync();
            List<GuideDetailWithMeta> guideDetails = guideMetaGuideDetails
                .GroupBy(mgm => mgm.GuideMetaGuid)
                .Select(group => group
                    .OrderByDescending(grItem => grItem.LanguageType == lang)
                    .ThenByDescending(grItem => grItem.LanguageType == LanguageType.English)
                    .FirstOrDefault()
                )
                .Where(gl => gl != null)
                .Select(GuideDetailWithMeta.Combine)
                .ToList();

            if (!guideDetails.Any()) return new ResultWithValue<List<GuideDetailWithMeta>>(false, new List<GuideDetailWithMeta>(), "Could not load GuideDetails");

            return new ResultWithValue<List<GuideDetailWithMeta>>(true, guideDetails, string.Empty);
        }

        public async Task<ResultWithValue<GuideDetailWithMeta>> GetGuideDetail(Guid guid, LanguageType lang)
        {
            List<GuideMetaGuideDetail> guideMetaGuideDetails = await _db.GuideMetaDatas
                .SelectMany(gm => gm.GuideMetaGuideDetails).ToListAsync();
            GuideMetaGuideDetail guideMetaGuideDetail = guideMetaGuideDetails
                .Where(mgm => mgm.LanguageType == lang)
                .FirstOrDefault(gm => gm.GuideMetaGuid == guid);
            if (guideMetaGuideDetail == null) return new ResultWithValue<GuideDetailWithMeta>(false, new GuideDetailWithMeta(), $"Could not load GuideDetail for {guid}");

            GuideDetailWithMeta guideDetail = GuideDetailWithMeta.Combine(guideMetaGuideDetail);
            if (guideDetail == null) return new ResultWithValue<GuideDetailWithMeta>(false, new GuideDetailWithMeta(), $"Could not load GuideDetail for {guid}");

            return new ResultWithValue<GuideDetailWithMeta>(true, guideDetail, string.Empty);
        }

        public async Task<Result> CreateGuideDetail(Guid guideMetaGuid, GuideDetail guideDetail, LanguageType lang)
        {
            try
            {
                await _db.GuideDetails.AddAsync(guideDetail);
                await _db.GuideMetaGuideDetails.AddAsync(new GuideMetaGuideDetail
                {
                    GuideMetaGuid = guideMetaGuid,
                    GuideDetailGuid = guideDetail.Guid,
                    LanguageType = lang
                });
                await _db.SaveChangesAsync();
                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

        public async Task<Result> UpdateGuideDetail(GuideDetail guideDetail, LanguageType lang)
        {
            GuideMeta existingGuideMeta = await _db.GuideMetaDatas.FirstOrDefaultAsync(g => g.Guid.Equals(guideDetail.Guid));
            if (existingGuideMeta == null) return new Result(false, "GuideMeta does not exist in database");

            try
            {
                GuideDetail existingGuideDetail = await _db.GuideMetaGuideDetails
                    .Where(gmgd => gmgd.GuideDetailGuid.Equals(guideDetail.Guid) && gmgd.LanguageType == lang)
                    .Select(gmgd => gmgd.GuideDetail).FirstOrDefaultAsync();

                //GuideDetail existingGuideDetail = await _db.GuideDetails.FirstAsync(g => g.Guid.Equals(guideDetail.Guid));
                if (existingGuideDetail != null)
                {
                    //guideDetail.Guid = existingGuideDetail.Guid;

                    _db.GuideDetails.Update(guideDetail);
                    await _db.SaveChangesAsync();
                    return new Result(true, string.Empty);
                }
                
                return await CreateGuideDetail(existingGuideMeta.Guid, guideDetail, lang);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

        public async Task<Result> DeleteGuideDetail(Guid guid)
        {
            GuideDetail guideToEdit = await _db.GuideDetails.FirstAsync(g => g.Guid.Equals(guid));
            if (guideToEdit == null) return new Result(false, "GuideDetail does not exist in database");

            //List<GuideMetaGuideDetail> realtionshipsToDelete = await _db.GuideMetaGuideDetails
            //    .Where(gmgd => gmgd.GuideDetailGuid.Equals(guid)).ToListAsync();

            try
            {
                //_db.GuideMetaGuideDetails.RemoveRange(realtionshipsToDelete);
                _db.GuideDetails.Remove(guideToEdit);
                await _db.SaveChangesAsync();
                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }
    }
}
