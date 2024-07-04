using NMS.Assistant.Domain.Dto.Model.Community;
using NMS.Assistant.Domain.Result;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace NMS.Assistant.Integration.Repository
{
    public class NmscdCommunitySearchRepository : BaseExternalApiRepository
    {
        private const string BaseUrl = "https://community.nmscd.com/";

        public NmscdCommunitySearchRepository(HttpClient httpClient) : base(httpClient) { }

        public Task<ResultWithValue<List<NMSCDCommunityLinkViewModel>>> GetLinks() =>
            Get<List<NMSCDCommunityLinkViewModel>>($"{BaseUrl}communityList.json");
        public Task<ResultWithValue<List<NMSCDChipColoursViewModel>>> GetChipColours() =>
            Get<List<NMSCDChipColoursViewModel>>($"{BaseUrl}chipColours.json");
    }
}
