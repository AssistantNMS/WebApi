using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Entity;
using NMS.Assistant.Persistence.Repository.Interface;

namespace NMS.Assistant.Persistence.Repository
{
    public class GuideMetaRepository : IGuideMetaRepository
    {
        private readonly NmsAssistantContext _db;

        public GuideMetaRepository(NmsAssistantContext db)
        {
            _db = db;
        }

        public async Task<ResultWithValue<List<GuideMeta>>> GetGuideMetas()
        {
            List<GuideMeta> guideMetas = await _db.GuideMetaDatas.ToListAsync();
            if (guideMetas == null || !guideMetas.Any()) return new ResultWithValue<List<GuideMeta>>(false, new List<GuideMeta>(), "Could not load GuideMetas");

            return new ResultWithValue<List<GuideMeta>>(true, guideMetas, string.Empty);
        }

        public async Task<ResultWithValue<GuideMeta>> GetGuideMeta(Guid guid)
        {
            GuideMeta latestGuideMeta = await _db.GuideMetaDatas.FirstOrDefaultAsync(gm => gm.Guid == guid);
            if (latestGuideMeta == null) return new ResultWithValue<GuideMeta>(false, new GuideMeta(), $"Could not load GuideMeta for {guid}");

            return new ResultWithValue<GuideMeta>(true, latestGuideMeta, string.Empty);
        }

        public async Task<ResultWithValue<GuideMeta>> GetGuideMetaHandleNotFound(Guid guid, bool give1View = true)
        {
            GuideMeta latestGuideMeta = await _db.GuideMetaDatas.FirstOrDefaultAsync(gm => gm.Guid == guid);
            if (latestGuideMeta == null) //return new ResultWithValue<GuideMeta>(false, new GuideMeta(), $"Could not load GuideMeta for ${guid}");
            {
                GuideMeta newGuideMeta = GuideMeta.NewGuideMeta(guid);
                Result createResult = await CreateGuideMeta(newGuideMeta);
                if (createResult.HasFailed) return new ResultWithValue<GuideMeta>(false, newGuideMeta, $"Could not load or create GuideMeta for ${guid}");

                return new ResultWithValue<GuideMeta>(true, newGuideMeta, string.Empty);
            }

            if (give1View)
            {
                Result addViewResult = await AddView(latestGuideMeta);
                if (addViewResult.HasFailed) return new ResultWithValue<GuideMeta>(false, latestGuideMeta, addViewResult.ExceptionMessage);
            }

            return new ResultWithValue<GuideMeta>(true, latestGuideMeta, string.Empty);
        }

        public async Task<Result> CreateGuideMeta(GuideMeta guideMeta)
        {
            try
            {
                await _db.GuideMetaDatas.AddAsync(guideMeta);
                await _db.SaveChangesAsync();
                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

        public async Task<Result> UpdateGuideMeta(GuideMeta guideMeta)
        {
            GuideMeta guideToEdit = await _db.GuideMetaDatas.FirstAsync(g => g.Guid.Equals(guideMeta.Guid));
            if (guideToEdit == null) return new Result(false, "GuideMeta does not exist in database");

            guideToEdit.Name = guideMeta.Name;
            guideToEdit.Likes = guideMeta.Likes;
            guideToEdit.Views = guideMeta.Views;
            guideToEdit.FileRelativePath = guideMeta.FileRelativePath;

            try
            {
                _db.GuideMetaDatas.Update(guideToEdit);
                await _db.SaveChangesAsync();
                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

        public async Task<Result> DeleteGuideMeta(Guid guid)
        {
            GuideMeta guideToEdit = await _db.GuideMetaDatas.FirstAsync(g => g.Guid.Equals(guid));
            if (guideToEdit == null) return new Result(false, "GuideMeta does not exist in database");
            
            try
            {
                _db.GuideMetaDatas.Remove(guideToEdit);
                await _db.SaveChangesAsync();
                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

        public async Task<Result> LikeGuide(Guid guid)
        {
            GuideMeta guideMeta = await _db.GuideMetaDatas.FirstAsync(u => u.Guid.Equals(guid));
            if (guideMeta == null) return new Result(false, "Guide does not exist in database");

            guideMeta.Likes += 1;

            try
            {
                _db.GuideMetaDatas.Update(guideMeta);
                await _db.SaveChangesAsync();
                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

        private async Task<Result> AddView(GuideMeta guideMeta)
        {
            guideMeta.Views += 1;

            try
            {
                _db.GuideMetaDatas.Update(guideMeta);
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
