using System;
using System.Collections.Generic;
using System.Linq;
using NMS.Assistant.Domain.Dto.Model.Donation;
using NMS.Assistant.Persistence.Entity;

namespace NMS.Assistant.Data.Mapper.ViewModel
{
    public static class DonationMapper
    {
        public static DonationViewModel ToViewModel(this Donation donation)
        {
            DonationViewModel vm = new DonationViewModel
            {
                Name = donation.Name,
                Type = donation.Type,
                DonationDate = donation.DonationDate
            };

            return vm;
        }

        public static List<DonationViewModel> ToViewModel(this List<Donation> orig) => orig.Select(o => o.ToViewModel()).ToList();

        public static AdminDonationViewModel ToAdminViewModel(this Donation donation)
        {
            AdminDonationViewModel vm = new AdminDonationViewModel
            {
                Guid = donation.Guid,
                Name = donation.Name,
                Type = donation.Type,
                Amount = donation.Amount,
                DonationDate = donation.DonationDate,
                IsHidden = donation.IsHidden,
            };

            return vm;
        }
        public static List<AdminDonationViewModel> ToAdminViewModel(this List<Donation> orig) => orig.Select(o => o.ToAdminViewModel()).ToList();

        public static Donation ToDatabaseModel(this AddDonationViewModel vm) => vm.ToDatabaseModel(Guid.NewGuid());
        public static Donation ToDatabaseModel(this AdminDonationViewModel vm)
        {
            Donation persistence = new Donation
            {
                Guid = vm.Guid,
                Type = vm.Type,
                Name = vm.Name,
                Amount = vm.Amount,
                IsHidden = vm.IsHidden,
                DonationDate = vm.DonationDate
            };

            return persistence;
        }

        public static Donation ToDatabaseModel(this AddDonationViewModel vm, Guid guid)
        {
            Donation persistence = new Donation
            {
                Guid = guid,
                Type = vm.Type,
                Name = vm.Name,
                Amount = vm.Amount,
                IsHidden = vm.IsHidden,
                DonationDate = vm.DonationDate
            };

            return persistence;
        }
    }
}
