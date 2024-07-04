using System.Threading.Tasks;
using NMS.Assistant.Domain.Contract.SendGrid;

namespace NMS.Assistant.Integration.Repository.Interface
{
    public interface ISendGridRepository
    {
        Task<bool> SendEmailUsingTemplate(SendGridTemplateEmail templateEmailDetails);
    }
}