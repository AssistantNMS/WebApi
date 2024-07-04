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
    public class DonationRepository: IDonationRepository
    {
        private readonly NmsAssistantContext _db;

        public DonationRepository(NmsAssistantContext db)
        {
            _db = db;
        }

        public async Task<ResultWithValue<List<Donation>>> GetAllDonations()
        {
            List <Donation> donations = await _db.Donations.Where(d => !d.IsHidden).OrderBy(f => f.DonationDate).ToListAsync();
            if (donations == null) return new ResultWithValue<List<Donation>>(false, new List<Donation>(), "Could not load Donations");

            return new ResultWithValue<List<Donation>>(true, donations, string.Empty);
        }

        public async Task<ResultWithValue<List<Donation>>> GetAllDonationsDesc()
        {
            List<Donation> donations = await _db.Donations.OrderByDescending(f => f.DonationDate).ToListAsync();
            if (donations == null) return new ResultWithValue<List<Donation>>(false, new List<Donation>(), "Could not load Donations");

            return new ResultWithValue<List<Donation>>(true, donations, string.Empty);
        }

        public async Task<Result> AddDonation(Donation addDonation)
        {
            try
            {
                await _db.Donations.AddAsync(new Donation
                {
                    Guid = Guid.NewGuid(),
                    Type = addDonation.Type,
                    Name = addDonation.Name,
                    Amount = addDonation.Amount,
                    IsHidden = addDonation.IsHidden,
                    DonationDate = addDonation.DonationDate
                });
                await _db.SaveChangesAsync();
                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

        public async Task<Result> EditDonation(Donation editDonation)
        {
            try
            {
                _db.Donations.Update(editDonation);
                await _db.SaveChangesAsync();
                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

        public async Task<Result> DeleteDonation(Guid guid)
        {
            try
            {
                Donation toDelete = await _db.Donations.FirstAsync(d => d.Guid.Equals(guid));
                if (toDelete == null) return new Result(false, "Could not find the specified Guid");

                _db.Donations.Remove(toDelete);
                await _db.SaveChangesAsync();
                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

        public async Task<ResultWithValue<int>> NumberOfDonations()
        {
            try
            {
                int numberOfDonations = await _db.Donations.CountAsync();
                return new ResultWithValue<int>(true, numberOfDonations, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResultWithValue<int>(false, 0, ex.Message);
            }
        }
    }
}
