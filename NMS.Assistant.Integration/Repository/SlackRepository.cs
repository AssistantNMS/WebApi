using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NMS.Assistant.Domain.Configuration.Interface;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Integration.Repository.Interface;

namespace NMS.Assistant.Integration.Repository
{
    public class SlackRepository : ISlackRepository
    {
        private readonly IWebhook _settings;

        public SlackRepository(IWebhook settings)
        {
            _settings = settings;
        }

        public async Task<Result> SendMessageToAllChannels(string msgContent) => await SendMessageToMultipleChannels(_settings.AppNews, msgContent);
        public async Task<Result> SendMessageToFeedbackChannels(string msgContent) => await SendMessageToMultipleChannels(_settings.Feedbacks, msgContent);
        public async Task<Result> SendMessageToTranslationChannels(string msgContent) => await SendMessageToMultipleChannels(_settings.Translations, msgContent);
        public async Task<Result> SendMessageToGuideChannels(string msgContent) => await SendMessageToMultipleChannels(_settings.Guides, msgContent);
        public async Task<Result> SendMessageToVersionChannels(string msgContent) => await SendMessageToMultipleChannels(_settings.Versions, msgContent);
        public async Task<Result> SendMessageToPersonalChannels(string msgContent) => await SendMessageToMultipleChannels(_settings.Personal, msgContent);

        private async Task<Result> SendMessageToMultipleChannels(IEnumerable<string> channels, string msgContent)
        {
            List<Task<ResultWithValue<string>>> tasks = channels.Select(g => SendMessage(g, msgContent)).ToList();
            ResultWithValue<string>[] completedTasks = await Task.WhenAll(tasks);

            return new Result(completedTasks.All(c => c.IsSuccess), string.Empty);
        }

        public async Task<ResultWithValue<string>> SendMessage(string url, string msgContent)
        {
            ResultWithValue<string> sendMessageResult = new ResultWithValue<string>(false, "Initialisation", "Initialisation");

            try
            {
                WebRequest tRequest = WebRequest.Create(url);
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";

                byte[] byteArray = Encoding.UTF8.GetBytes(msgContent);
                tRequest.ContentLength = byteArray.Length;
                using (Stream dataStream = await tRequest.GetRequestStreamAsync())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = await tRequest.GetResponseAsync())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            if (dataStreamResponse == null)
                            {
                                return new ResultWithValue<string>(false, "Something went very wrong", "Something went very wrong");
                            }
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                sendMessageResult.IsSuccess = true;
                                sendMessageResult.Value = await tReader.ReadToEndAsync();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                sendMessageResult.IsSuccess = false;
                sendMessageResult.Value = $"Slack exception: {ex.Message}";
            }

            return sendMessageResult;
        }
    }
}