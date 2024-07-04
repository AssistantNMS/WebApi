using System.Collections.Generic;
using NMS.Assistant.Domain.Configuration.Interface;

namespace NMS.Assistant.Domain.Configuration
{
    public class Webhook : IWebhook
    {
        public List<string> AppNews { get; set; }
        public List<string> Feedbacks { get; set; }
        public List<string> Translations { get; set; }
        public List<string> Guides { get; set; }
        public List<string> Versions { get; set; }
        public List<string> Personal { get; set; }
    }
}
