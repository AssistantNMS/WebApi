using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NMS.Assistant.Domain.Dto.Enum;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Entity;
using NMS.Assistant.Persistence.Helper;
using NMS.Assistant.Persistence.Mapper;
using NMS.Assistant.Persistence.Repository.Interface;

namespace NMS.Assistant.Persistence.Repository
{
    public class FriendCodeRepository : IFriendCodeRepository
    {
        private readonly NmsAssistantContext _db;

        public FriendCodeRepository(NmsAssistantContext db)
        {
            _db = db;
        }

        public async Task<ResultWithValue<List<FriendCode>>> GetAllPendingFriendCodes(bool showPc, bool showPs4, bool showXb1, bool showNsw, bool showGog, bool showMac)
        {
            List<PendingFriendCode> pendingFriendCodes = await _db.PendingFriendCodes
                .AddFilterIfValue(!showPc, l => l.PlatformType != PlatformType.PC)
                .AddFilterIfValue(!showPs4, l => l.PlatformType != PlatformType.PS4)
                .AddFilterIfValue(!showXb1, l => l.PlatformType != PlatformType.XB1)
                .AddFilterIfValue(!showNsw, l => l.PlatformType != PlatformType.NSW)
                .AddFilterIfValue(!showGog, l => l.PlatformType != PlatformType.GOG)
                .AddFilterIfValue(!showMac, l => l.PlatformType != PlatformType.MAC)
                .OrderBy(pfc => pfc.SortRank).ThenBy(pfc => pfc.DateSubmitted)
                .ToListAsync();
            if (pendingFriendCodes == null) return new ResultWithValue<List<FriendCode>>(false, new List<FriendCode>(), "Could not load PendingFriendCode");

            return new ResultWithValue<List<FriendCode>>(true, pendingFriendCodes.ToBaseFriendCode(), string.Empty);
        }

        public async Task<ResultWithValue<FriendCode>> GetAllPendingFriendCodeByEmailHashAndGuid(string emailHash, Guid guid)
        {
            try
            {
                PendingFriendCode pendingFriendCode = await _db.PendingFriendCodes
                    .FirstAsync(pf => pf.EmailHash.Equals(emailHash) && pf.Guid.Equals(guid)
                ); ;
                if (pendingFriendCode == null) return new ResultWithValue<FriendCode>(false, new FriendCode(), "Could not load PendingFriendCode");
                
                return new ResultWithValue<FriendCode>(true, pendingFriendCode.ToBaseFriendCode(), string.Empty);
            }
            catch (Exception ex)
            {
                return new ResultWithValue<FriendCode>(false, new FriendCode(), ex.Message);
            }
        }

        public async Task<ResultWithValue<List<FriendCode>>> GetAllFriendCodes(bool showPc, bool showPs4, bool showXb1, bool showNsw, bool showGog, bool showMac)
        {
            List<FriendCode> friendCodes = await _db.FriendCodes
                .AddFilterIfValue(!showPc, l => l.PlatformType != PlatformType.PC)
                .AddFilterIfValue(!showPs4, l => l.PlatformType != PlatformType.PS4)
                .AddFilterIfValue(!showXb1, l => l.PlatformType != PlatformType.XB1)
                .AddFilterIfValue(!showNsw, l => l.PlatformType != PlatformType.NSW)
                .AddFilterIfValue(!showGog, l => l.PlatformType != PlatformType.GOG)
                .AddFilterIfValue(!showMac, l => l.PlatformType != PlatformType.MAC)
                .OrderBy(fc => fc.SortRank).ThenBy(fc => fc.DateSubmitted)
                .ToListAsync();
            if (friendCodes == null) return new ResultWithValue<List<FriendCode>>(false, new List<FriendCode>(), "Could not load FriendCodes");

            return new ResultWithValue<List<FriendCode>>(true, friendCodes, string.Empty);
        }

        public async Task<Result> AddPendingFriendCode(FriendCode addPendingFriendCode)
        {
            try
            {
                await _db.PendingFriendCodes.AddAsync(addPendingFriendCode.ToPendingFriendCode());
                await _db.SaveChangesAsync();
                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

        public async Task<Result> AddFriendCode(FriendCode addFriendCode)
        {
            try
            {
                await _db.FriendCodes.AddAsync(addFriendCode);
                await _db.SaveChangesAsync();
                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

        public async Task<Result> EditPendingFriendCode(FriendCode editPendingFriendCode)
        {
            try
            {
                PendingFriendCode toEditPendingFriendCode = await _db.PendingFriendCodes.FirstAsync(g => g.Guid.Equals(editPendingFriendCode.Guid));
                if (toEditPendingFriendCode == null) return new Result(false, "Friend Code does not exist in database");

                toEditPendingFriendCode.Name = editPendingFriendCode.Name;
                toEditPendingFriendCode.PlatformType = editPendingFriendCode.PlatformType;
                toEditPendingFriendCode.Code = editPendingFriendCode.Code;
                toEditPendingFriendCode.DateSubmitted = editPendingFriendCode.DateSubmitted;
                toEditPendingFriendCode.SortRank = editPendingFriendCode.SortRank;

                _db.PendingFriendCodes.Update(toEditPendingFriendCode);
                await _db.SaveChangesAsync();
                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

        public async Task<Result> EditFriendCode(FriendCode editFriendCode)
        {
            try
            {
                FriendCode toEditFriendCode = await _db.FriendCodes.FirstAsync(g => g.Guid.Equals(editFriendCode.Guid));
                if (toEditFriendCode == null) return new Result(false, "Friend Code does not exist in database");

                toEditFriendCode.Name = editFriendCode.Name;
                toEditFriendCode.PlatformType = editFriendCode.PlatformType;
                toEditFriendCode.Code = editFriendCode.Code;
                toEditFriendCode.DateSubmitted = editFriendCode.DateSubmitted;
                toEditFriendCode.SortRank = editFriendCode.SortRank;

                _db.FriendCodes.Update(toEditFriendCode);
                await _db.SaveChangesAsync();
                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

        public async Task<Result> DeletePendingFriendCode(Guid guid)
        {
            try
            {
                PendingFriendCode toPendingFriendCode = await _db.PendingFriendCodes.FirstAsync(d => d.Guid.Equals(guid));
                if (toPendingFriendCode == null) return new Result(false, "Could not find the specified Guid");

                _db.PendingFriendCodes.Remove(toPendingFriendCode);
                await _db.SaveChangesAsync();
                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

        public async Task<Result> DeleteFriendCode(Guid guid)
        {
            try
            {
                FriendCode toFriendCode = await _db.FriendCodes.FirstAsync(d => d.Guid.Equals(guid));
                if (toFriendCode == null) return new Result(false, "Could not find the specified Guid");

                _db.FriendCodes.Remove(toFriendCode);
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
