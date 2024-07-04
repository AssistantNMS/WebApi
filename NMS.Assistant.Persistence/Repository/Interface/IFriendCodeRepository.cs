using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Entity;

namespace NMS.Assistant.Persistence.Repository.Interface
{
    public interface IFriendCodeRepository
    {
        Task<ResultWithValue<List<FriendCode>>> GetAllPendingFriendCodes(bool showPc, bool showPs4, bool showXb1, bool showNsw, bool showGog, bool showMac);
        Task<ResultWithValue<FriendCode>> GetAllPendingFriendCodeByEmailHashAndGuid(string emailHash, Guid guid);
        Task<ResultWithValue<List<FriendCode>>> GetAllFriendCodes(bool showPc, bool showPs4, bool showXb1, bool showNsw, bool showGog, bool showMac);
        Task<Result> AddPendingFriendCode(FriendCode addPendingFriendCode);
        Task<Result> AddFriendCode(FriendCode addFriendCode);
        Task<Result> EditPendingFriendCode(FriendCode editPendingFriendCode);
        Task<Result> EditFriendCode(FriendCode editFriendCode);
        Task<Result> DeletePendingFriendCode(Guid guid);
        Task<Result> DeleteFriendCode(Guid guid);
    }
}