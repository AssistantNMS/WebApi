using Newtonsoft.Json;

namespace NMS.Assistant.Domain.Contract.SendGrid.TemplateData
{
    public class ConfirmFriendCodeTemplateData
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("subtitle")]
        public string Subtitle { get; set; }

        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("additionalNote")]
        public string AdditionalNote { get; set; }

        [JsonProperty("emailHashExample")]
        public string EmailHashExample { get; set; }

        [JsonProperty("emailHash")]
        public string EmailHash { get; set; }

        [JsonProperty("confirm")]
        public string Confirm { get; set; }

        [JsonProperty("confirmLink")]
        public string ConfirmLink { get; set; }

        [JsonProperty("backupLinkText")]
        public string BackupLinkText { get; set; }

        [JsonProperty("anyQuestionsText")]
        public string AnyQuestionsText { get; set; }

        [JsonProperty("emailSignOff")]
        public string EmailSignOff { get; set; }

        [JsonProperty("emailSignOffTeam")]
        public string EmailSignOffTeam { get; set; }

        [JsonProperty("needMoreHelp")]
        public string NeedMoreHelp { get; set; }

        [JsonProperty("weAreHereToHelp")]
        public string WeAreHereToHelp { get; set; }
    }
}
