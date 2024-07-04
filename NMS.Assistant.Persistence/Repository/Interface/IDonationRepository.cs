using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Entity;

namespace NMS.Assistant.Persistence.Repository.Interface
{
    public interface IDonationRepository
    {
        Task<ResultWithValue<List<Donation>>> GetAllDonations();
        Task<Result> AddDonation(Donation addDonation);
        Task<Result> EditDonation(Donation editDonation);
        Task<Result> DeleteDonation(Guid guid);
        Task<ResultWithValue<int>> NumberOfDonations();
        Task<ResultWithValue<List<Donation>>> GetAllDonationsDesc();
    }
}