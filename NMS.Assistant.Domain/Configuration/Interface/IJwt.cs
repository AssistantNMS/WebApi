namespace NMS.Assistant.Domain.Configuration.Interface
{
    public interface IJwt
    {
        string Secret { get; set; }
        int TimeValidInSeconds { get; set; }
        string Issuer { get; set; }
        string Audience { get; set; }
        int ClockSkewInSeconds { get; set; }
    }
}
