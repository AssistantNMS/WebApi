using System;

namespace NMS.Assistant.Domain.Dto.Model.Donation
{
    public class AdminDonationViewModel: DonationViewModel
    {
        public Guid Guid { get; set; }

        public string Amount { get; set; }
        public bool IsHidden { get; set; }
    }
}
