using System;
using NMS.Assistant.Domain.Dto.Enum;

namespace NMS.Assistant.Domain.Dto.Model.Donation
{
    public class DonationViewModel
    {
        public string Name { get; set; }

        public DonationType Type { get; set; }

        public DateTime DonationDate { get; set; }
    }
}
