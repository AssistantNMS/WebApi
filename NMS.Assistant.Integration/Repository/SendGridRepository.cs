using SendGrid;
using System.Net;
using System.Threading.Tasks;
using NMS.Assistant.Domain.Configuration.Interface;
using NMS.Assistant.Domain.Contract.SendGrid;
using NMS.Assistant.Integration.Repository.Interface;
using SendGrid.Helpers.Mail;

namespace NMS.Assistant.Integration.Repository
{
    public class SendGridRepository : ISendGridRepository
    {
        private readonly ISendGrid _config;

        public SendGridRepository(ISendGrid config)
        {
            _config = config;
        }

        public async Task<bool> SendEmailUsingTemplate(SendGridTemplateEmail templateEmailDetails)
        {
            var client = new SendGridClient(_config.ApiKey);

            var msg = new SendGridMessage();
            msg.AddTo(new EmailAddress(templateEmailDetails.To));
            msg.SetFrom(new EmailAddress(templateEmailDetails.From));
            msg.SetTemplateId(templateEmailDetails.TemplateId);
            msg.SetTemplateData(templateEmailDetails.TemplateData);

            var response = await client.SendEmailAsync(msg);
            return response.StatusCode == HttpStatusCode.OK
                   || response.StatusCode == HttpStatusCode.Created
                   || response.StatusCode == HttpStatusCode.Accepted;
        }
    }
}
