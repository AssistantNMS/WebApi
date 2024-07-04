using NMS.Assistant.Domain.Configuration.Interface;

namespace NMS.Assistant.Domain.Configuration
{
    public class Authentication : IAuthentication
    {
        public GoogleAuth GoogleAuth { get; set; }
        public UptimeRobotAuth UptimeRobotAuth { get; set; }
        public NomNomAuth NomNomAuth { get; set; }
        public MastodonAuth MastodonAuth { get; set; }
        public GunterAuth GunterAuth { get; set; }
        public NoMansSkySocialAuth NoMansSkySocialAuth { get; set; }
    }

    public class GoogleAuth : IGoogleAuth
    {
        public string ApiKey { get; set; }
    }    
    
    public class UptimeRobotAuth : IUptimeRobotAuth
    {
        public string NMSCDApiKey { get; set; }
    }
    
    public class NomNomAuth : INomNomAuth
    {
        public string Inventory { get; set; }
    }    
    
    public class MastodonAuth : IMastodonAuth
    {
        public string AccessToken { get; set; }
    }

    public class GunterAuth : IGunterAuth
    {
        public string Update { get; set; }
    }

    public class NoMansSkySocialAuth : INoMansSkySocialAuth
    {
        public string ApiKey { get; set; }
    }
}