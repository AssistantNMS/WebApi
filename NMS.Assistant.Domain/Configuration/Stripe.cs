using NMS.Assistant.Domain.Configuration.Interface;

namespace NMS.Assistant.Domain.Configuration
{
    public class Stripe: IStripe
    {
        public string PublishableKey { get; set; }
        public string SecretKey { get; set; }
    }
}
