using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NMS.Assistant.Api.Filter;
using NMS.Assistant.Domain.Dto.Enum;
using NMS.Assistant.Integration.Repository.Interface;

namespace NMS.Assistant.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GithubController : ControllerBase
    {
        private readonly IGithubRepository _githubRepo;

        public GithubController(IGithubRepository githubRepo)
        {
            _githubRepo = githubRepo;
        }

        /// <summary>
        /// Get a Language file from Github
        /// </summary>
        /// <param name="filename">
        /// e.g. language.en.json
        /// </param>
        [HttpGet("{filename}")]
        [CacheFilter(CacheType.Github, includeUrl: true)]
        public async Task<ActionResult<string>> GetGithubFile(string filename) => Ok(await _githubRepo.GetFileContents(filename));
    }
}