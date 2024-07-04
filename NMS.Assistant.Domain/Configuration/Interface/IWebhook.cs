using System.Collections.Generic;

namespace NMS.Assistant.Domain.Configuration.Interface
{
    public interface IWebhook
    {
        List<string> AppNews { get; set; }
        List<string> Feedbacks { get; set; }
        List<string> Translations { get; set; }
        List<string> Guides { get; set; }
        List<string> Versions { get; set; }
        List<string> Personal { get; set; }
    }
}
