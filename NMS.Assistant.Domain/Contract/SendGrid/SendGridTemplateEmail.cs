namespace NMS.Assistant.Domain.Contract.SendGrid
{
    public class SendGridTemplateEmail
    {
        public string To { get; set; }
        public string From { get; set; }
        public string TemplateId { get; set; }
        public object TemplateData { get; set; }
    }
}
