namespace NMS.Assistant.Domain.Configuration.Interface
{
    public interface IApiConfiguration
    {
        string[] AllowedHosts { get; set; }
        ApplicationInsights ApplicationInsights { get; set; }
        Database Database { get; set; }
        Stripe Stripe { get; set; }
        Caching Caching { get; set; }
        Jwt Jwt { get; set; }
        Webhook Webhook { get; set; }
        Logging Logging { get; set; }
        Twitter Twitter { get; set; }
        Firebase Firebase { get; set; }
        SharedDirectory SharedDirectory { get; set; }
        Redis Redis { get; set; }
        SendGrid SendGrid { get; set; }
        Directory Directory { get; set; }
        Authentication Authentication { get; set; }
    }
}
