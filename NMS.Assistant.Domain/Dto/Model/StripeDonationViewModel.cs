namespace NMS.Assistant.Domain.Dto.Model
{
    public class StripeDonationViewModel
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Token { get; set; }
        public bool IsGooglePay { get; set; }
    }
}
