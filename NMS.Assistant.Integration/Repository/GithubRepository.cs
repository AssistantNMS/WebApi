using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Integration.Repository.Interface;

namespace NMS.Assistant.Integration.Repository
{
    public class GithubRepository: BaseExternalApiRepository, IGithubRepository
    {
        private readonly string _baseGithubUrl = "https://raw.githubusercontent.com/AssistantNMS/Languages/master/lang/";

        public GithubRepository(HttpClient httpClient) : base(httpClient) { }

        public async Task<string> GetFileContents(string filename)
        {
            if (IsMaliciousName(filename)) return "{}";

            ResultWithValue<string> githubFileContents = await Get($"{_baseGithubUrl}{filename}");

            if (githubFileContents.HasFailed) return "{}";

            return githubFileContents.Value;
        }

        private bool IsMaliciousName(string filename)
        {
            Regex regexLangFileName = new Regex(@"(language.)([a-z]|\-){2,7}(.json)");
            return !regexLangFileName.Match(filename).Success;
        }
    }
}
