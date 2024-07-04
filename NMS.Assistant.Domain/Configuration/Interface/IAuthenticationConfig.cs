namespace NMS.Assistant.Domain.Configuration.Interface
{
    public interface IAuthentication
    {
        GoogleAuth GoogleAuth { get; set; }
        UptimeRobotAuth UptimeRobotAuth { get; set; }
        NomNomAuth NomNomAuth { get; set; }
        GunterAuth GunterAuth { get; set; }
        NoMansSkySocialAuth NoMansSkySocialAuth { get; set; }
    }

    public interface IGoogleAuth
    {
        string ApiKey { get; set; }
    }

    public interface IUptimeRobotAuth
    {
        string NMSCDApiKey { get; set; }
    }

    public interface INomNomAuth
    {
        string Inventory { get; set; }
    }

    public interface IMastodonAuth
    {
        string AccessToken { get; set; }
    }

    public interface IGunterAuth
    {
        string Update { get; set; }
    }

    public interface INoMansSkySocialAuth
    {
        string ApiKey { get; set; }
    }
}