using System;
using System.Collections.Generic;
using NMS.Assistant.Domain.Constants;
using NMS.Assistant.Domain.Contract.SendGrid.TemplateData;

namespace NMS.Assistant.Data.Localiser
{
    public static class FriendCodeLocaliser
    {
        public static ConfirmFriendCodeTemplateData GetConfirmEmail(Dictionary<string, string> languageDict, string baseApiUrl, string emailHash, Guid friendCodeGuid)
        {
            ConfirmFriendCodeTemplateData result = new ConfirmFriendCodeTemplateData
            {
                Title = languageDict[LocaleKey.ThankYou],
                Subtitle = languageDict[LocaleKey.ThankYouForSubmittingFriendCode],
                Subject = languageDict[LocaleKey.ConfirmFriendCode],
                AdditionalNote = languageDict[LocaleKey.WeDoNotStoreEmails],
                EmailHashExample = languageDict[LocaleKey.EmailHashExample],
                EmailHash = emailHash,
                Confirm = languageDict[LocaleKey.ConfirmFriendCode],
                ConfirmLink = $"{baseApiUrl}/FriendCode/Confirm/{emailHash}/{friendCodeGuid}",
                BackupLinkText = languageDict[LocaleKey.EmailBackupLink],
                AnyQuestionsText = languageDict[LocaleKey.AnyQuestionsReplyToThisEmail],
                EmailSignOff = languageDict[LocaleKey.EmailSignOff],
                EmailSignOffTeam = languageDict[LocaleKey.EmailSignOffTeam],
                NeedMoreHelp = languageDict[LocaleKey.NeedMoreHelp],
                WeAreHereToHelp = languageDict[LocaleKey.WeAreHereToHelp],
            };
            return result;
        }
    }
}
