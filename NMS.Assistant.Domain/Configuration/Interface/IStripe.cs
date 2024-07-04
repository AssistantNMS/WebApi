namespace NMS.Assistant.Domain.Configuration.Interface
{
    public interface IStripe
    {
        string PublishableKey { get; set; }
        string SecretKey { get; set; }
    }
}
