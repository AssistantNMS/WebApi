using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NMS.Assistant.Domain.Result;

namespace NMS.Assistant.Integration.Repository
{
    public class PurgoMalumRepository : BaseExternalApiRepository, IPurgoMalumRepository
    {
        private const string BaseUrl = "https://www.purgomalum.com/service/json?text=";

        public PurgoMalumRepository(HttpClient httpClient) : base(httpClient) { }

        public async Task<ResultWithValue<string>> GetCleanString(string text)
        {
            string url = $"{BaseUrl}{text}";
            ResultWithValue<PurgoMalumResult> cleanText = await Get<PurgoMalumResult>(url);
            if (cleanText.HasFailed) return new ResultWithValue<string>(false, "", cleanText.ExceptionMessage);
            
            return new ResultWithValue<string>(true, cleanText.Value.Result, string.Empty);
        }
    }


    public class PurgoMalumResult
    {
        [JsonProperty("result")]
        public string Result { get; set; }
    }

}
