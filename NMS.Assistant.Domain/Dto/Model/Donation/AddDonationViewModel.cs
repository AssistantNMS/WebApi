using System;
using NMS.Assistant.Domain.Dto.Enum;

namespace NMS.Assistant.Domain.Dto.Model.Donation
{
    public class AddDonationViewModel
    {
        public string Name { get; set; }

        public string Amount { get; set; }

        public DonationType Type { get; set; }

        public bool IsHidden { get; set; }

        public DateTime DonationDate { get; set; }
    }
}
