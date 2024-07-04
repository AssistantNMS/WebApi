using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NMS.Assistant.Domain.Constants;
using NMS.Assistant.Domain.Contract;
using NMS.Assistant.Domain.Dto.Model.Feedback;
using NMS.Assistant.Domain.Dto.Model.Guide;
using NMS.Assistant.Domain.Dto.Model.Language;
using NMS.Assistant.Domain.Generated;
using NMS.Assistant.Integration.Contract;
using NMS.Assistant.Persistence.Entity;
using Version = NMS.Assistant.Persistence.Entity.Version;

namespace NMS.Assistant.Data.Helper
{
    public static class SlackMessageHelper
    {
        private static int UtcDate() =>
            (int) (DateTime.Now.ToUniversalTime() - (new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)))
            .TotalSeconds;

        public static string NewCommunityMissionMessage(string title, string message)
        {
            SlackMessage slackMsg = new SlackMessage
            {
                Attachments = new List<Attachment>
                {
                    new Attachment
                    {
                        Fallback = title,
                        Title = title,
                        Text = message,
                        Colour = MessageColour.CommunityMission,
                        Time = UtcDate()
                    }
                }
            };

            return JsonConvert.SerializeObject(slackMsg);
        }

        public static string NewStripeDonation(bool isAndroid, decimal amount)
        {
            string title = $"New Donation from {(isAndroid ? "Android" : "Apple")}";
            SlackMessage slackMsg = new SlackMessage
            {
                Attachments = new List<Attachment>
                {
                    new Attachment
                    {
                        Fallback = title,
                        Title = title,
                        Text = $"{amount:0.00} USD",
                        Colour = MessageColour.StripeDonation,
                        Time = UtcDate()
                    }
                }
            };

            return JsonConvert.SerializeObject(slackMsg);
        }

        public static string NewFeedbackReceived(Feedback form, FeedbackAnsweredViewModel answerViewModel)
        {
            const string title = "New Feedback Submitted";

            List<SlackField> fields = new List<SlackField>();
            foreach (FeedbackQuestion question in form.Questions.ToList())
            {
                FeedbackQuestionAnsweredViewModel answer = answerViewModel.Answers.FirstOrDefault(ans => ans.FeedbackQuestionGuid.Equals(question.Guid));
                string answerString = answer?.Answer ?? "Unknown";
                fields.Add(new SlackField
                {
                    Title = question.Question,
                    Value = question.ContainsPotentiallySensitiveInfo ? "|| HIDDEN ||" : answerString,
                    Short = false
                });
            }

            SlackMessage slackMsg = new SlackMessage
            {
                Attachments = new List<Attachment>
                {
                    new Attachment
                    {
                        Fallback = title,
                        Title = title,
                        Text = $"Feedback received from {answerViewModel.AppType} on form: {form.Name}",
                        Fields = fields,
                        Colour = MessageColour.Feedback,
                        Time = UtcDate()
                    }
                }
            };

            return JsonConvert.SerializeObject(slackMsg);
        }

        public static string NewTranslationReceived(LanguageFileViewModel langFileViewModel)
        {
            const string title = "New Translation Submitted";

            SlackMessage slackMsg = new SlackMessage
            {
                Attachments = new List<Attachment>
                {
                    new Attachment
                    {
                        Fallback = title,
                        Title = title,
                        Text = $"Translation received for {langFileViewModel.Filename}",
                        Colour = MessageColour.Translate,
                        Time = UtcDate()
                    }
                }
            };

            return JsonConvert.SerializeObject(slackMsg);
        }

        public static string NewReleaseLogMessage(string title, string message, ReleaseLogItem release)
        {
            SlackMessage slackMsg = new SlackMessage
            {
                Attachments = new List<Attachment>
                {
                    new Attachment
                    {
                        Fallback = title,
                        Title = title,
                        Text = message,
                        TitleLink = release.Link,
                        Colour = MessageColour.ReleaseLog,
                        Time = UtcDate()
                    }
                }
            };

            return JsonConvert.SerializeObject(slackMsg);
        }

        public static string NewNewsArticleMessage(string title, NewsItem newsItem)
        {
            SlackMessage slackMsg = new SlackMessage
            {
                Attachments = new List<Attachment>
                {
                    new Attachment
                    {
                        Fallback = title,
                        Title = title,
                        Text = newsItem.Description,
                        TitleLink = newsItem.Link,
                        ImageUrl = newsItem.Image,
                        Colour = MessageColour.NewsArticle,
                        Time = UtcDate()
                    }
                }
            };
            /*
            SlackBlockKitMessage slackMsg = new SlackBlockKitMessage
            {
                Blocks = new List<Block>
                {
                    new Block
                    {
                        Type = BlockType.section,
                        Text = new BlockText
                        {
                            Type = BlockTextType.mrkdwn,
                            Text = title
                        }
                    },
                    new Block
                    {
                        Type = BlockType.section,
                        Text = new BlockText
                        {
                            Type = BlockTextType.mrkdwn,
                            Text = $"## {version.Name}"
                        }
                    },
                    new Block
                    {
                        Type = BlockType.image,
                        Text = new BlockText
                        {
                            Type = BlockTextType.plain_text,
                            Text = version.Name
                        }
                    },
                    new Block
                    {
                        Type = BlockType.section,
                        Text = new BlockText
                        {
                            Type = BlockTextType.plain_text,
                            Text = version.Description
                        }
                    },
                    new Block
                    {
                        Type = BlockType.context,
                        Text = new BlockText
                        {
                            Type = BlockTextType.plain_text,
                            Text = version.Date
                        }
                    }
                }
            };
            */

            return JsonConvert.SerializeObject(slackMsg);
        }


        public static string NewVersionMessage(Version version)
        {
            SlackMessage slackMsg = new SlackMessage
            {
                Attachments = new List<Attachment>
                {
                    new Attachment
                    {
                        Fallback = $"{version.Name} is Live on Android and iOS",
                        Title = $"{version.Name} is Live",
                        Text = $"{version.Name} has been detected on Google Play and the Apple App Store",
                        TitleLink = "https://api.nmsassistant.com/version",
                        Colour = MessageColour.Version,
                        Time = UtcDate()
                    }
                }
            };
            return JsonConvert.SerializeObject(slackMsg);
        }


        public static string NewGuideSubmissionMessage(GuideDetailViewModel viewModel)
        {
            const string title = "New Guide Submitted";

            string text = $"Guide Submission received, {viewModel.Title}";
            if (string.IsNullOrEmpty(viewModel.Author)) text += $" by {viewModel.Author}";
            
            SlackMessage slackMsg = new SlackMessage
            {
                Attachments = new List<Attachment>
                {
                    new Attachment
                    {
                        Fallback = title,
                        Title = title,
                        Text = text,
                        Fields = new List<SlackField>
                        {
                            new SlackField
                            {
                                Title = "Tags",
                                Value = string.Join(", ", viewModel.Tags ?? new List<string>()),
                                Short = false
                            }
                        },
                        Colour = MessageColour.Guide,
                        Time = UtcDate()
                    }
                }
            };
            return JsonConvert.SerializeObject(slackMsg);
        }

        public static string NewSteamNewsMessage(string title, string message, SteamNewsItem newsItem)
        {
            SlackMessage slackMsg = new SlackMessage
            {
                Attachments = new List<Attachment>
                {
                    new Attachment
                    {
                        Fallback = title,
                        Title = title,
                        Text = message,
                        TitleLink = newsItem.Link,
                        Colour = MessageColour.ReleaseLog,
                        Time = UtcDate()
                    }
                }
            };

            return JsonConvert.SerializeObject(slackMsg);
        }

        public static string NewSteamDepotMessage(string title, string message, SteamDepotItem depotItem)
        {
            SlackMessage slackMsg = new SlackMessage
            {
                Attachments = new List<Attachment>
                {
                    new Attachment
                    {
                        Fallback = title,
                        Title = title,
                        Text = message,
                        Colour = MessageColour.ReleaseLog,
                        Time = UtcDate()
                    }
                }
            };

            return JsonConvert.SerializeObject(slackMsg);
        }
    }
}
