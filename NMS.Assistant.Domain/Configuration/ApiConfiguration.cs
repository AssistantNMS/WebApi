using NMS.Assistant.Domain.Configuration.Interface;

namespace NMS.Assistant.Domain.Configuration
{
    public class ApiConfiguration: IApiConfiguration
    {
        public string[] AllowedHosts { get; set; }
        public ApplicationInsights ApplicationInsights { get; set; }
        public Database Database { get; set; }
        public Stripe Stripe { get; set; }
        public Caching Caching { get; set; }
        public Jwt Jwt{ get; set; }
        public Webhook Webhook { get; set; }
        public Logging Logging { get; set; }
        public Twitter Twitter { get; set; }
        public Firebase Firebase { get; set; }
        public SharedDirectory SharedDirectory { get; set; }
        public Redis Redis { get; set; }
        public SendGrid SendGrid { get; set; }
        public Directory Directory { get; set; }
        public Authentication Authentication { get; set; }
    }
}
