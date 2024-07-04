using Microsoft.AspNetCore.Mvc;

namespace NMS.Assistant.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MiscController : ControllerBase
    {
        //[HttpGet]
        //[Route("/")]
        //public IActionResult HandleNavigateToApiRoot()
        //{
        //    return RedirectPermanent("https://nmsassistant.com");
        //}

        /// <summary>
        /// Handle HEAD requests to root of API.
        /// </summary>
        [HttpHead]
        [Route("/")]
        [Route("/{*url}", Order = 998)]
        public IActionResult Head()
        {
            return Ok("API running...");
        }

        /// <summary>
        /// Handle requests for favicon
        /// </summary>
        [HttpGet]
        [Route("/favicon.ico")]
        public IActionResult HandleFaviconRequest()
        {
            return RedirectPermanent("https://nmsassistant.com/favicon.ico");
        }

        /// <summary>
        /// Handle requests for robots.txt
        /// </summary>
        [HttpGet]
        [Route("/robots.txt")]
        public IActionResult HandleRobots()
        {
            return RedirectPermanent("https://nmsassistant.com/robots.txt");
        }

        /// <summary>
        /// Handle requests for Version, from a bad deploy
        /// </summary>
        [HttpGet]
        [Route("/https:/nomanssky.kurtlourens.com/assets/json/versionV2.json")]
        public IActionResult HandleVersionScrewUp()
        {
            return RedirectPermanent("https://nmsassistant.com/assets/json/versionV2.json");
        }

        /// <summary>
        /// Handle not found requests. Version requests redirected to /version. Everything else redirected to Swagger Docs
        /// </summary>
        [HttpGet]
        [Route("/{*url}", Order = 999)]
        public IActionResult CatchAll(string url)
        {
            if (url.Contains("version.json") || url.Contains("versionV2.json"))
            {
                return RedirectPermanent("https://api.nmsassistant.com/version");
            }

            return RedirectPermanent("https://api.nmsassistant.com");
        }
    }
}