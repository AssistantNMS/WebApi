using System.Net.Http;
using System.Threading.Tasks;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Integration.Repository.Interface;

namespace NMS.Assistant.Integration.Repository
{
    public class FaqBotRepository : BaseExternalApiRepository, IFaqBotRepository
    {
        private const string BaseApiUrl = "https://nms-faq-bot.herokuapp.com/api";

        public FaqBotRepository(HttpClient httpClient) : base(httpClient) { }

        public async Task<Result> AlertFaqBotOfVersionChange()
        {
            string url = $"{BaseApiUrl}/newVersion ";

            ResultWithValue<string> apiResult = await Get(url);
            return apiResult;
        }
    }
}
