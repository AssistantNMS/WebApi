using System;
using NMS.Assistant.Domain.Dto.Enum;
using NMS.Assistant.Domain.Dto.Model;
using NMS.Assistant.Persistence.Entity;

namespace NMS.Assistant.Data.Mapper.ViewModel
{
    public static class StripeMapper
    {
        public static Donation ToDatabaseModel(this StripeDonationViewModel donation)
        {
            Donation persistence = new Donation
            {
                Guid = Guid.NewGuid(),
                DonationDate = DateTime.Now,
                Name = "Unknown",
                Amount = $"{donation.Amount} {donation.Currency}",
                Type = donation.IsGooglePay ? DonationType.GooglePay : DonationType.ApplePay
            };

            return persistence;
        }
    }
}
