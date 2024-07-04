using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using NMS.Assistant.Api.Model;
using NMS.Assistant.Domain.Configuration.Interface;
using NMS.Assistant.Domain.Dto.Enum;
using NMS.Assistant.Domain.Result;

namespace NMS.Assistant.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IDirectory _dirConfig;
        private readonly IAuthentication _authConfig;

        public UploadController(IDirectory dirConfig, IAuthentication authConfig)
        {
            _dirConfig = dirConfig;
            _authConfig = authConfig;
        }

        /// <summary>
        /// Get list of uploaded images from the server
        /// </summary>
        /// <param name="uploadFolderType">
        /// The type of upload, determines where the file will be saved
        /// </param>
        [HttpGet("{uploadFolderType}")]
        [Authorize]
        public IActionResult GetUploadedImages(UploadFolderType uploadFolderType)
        {
            if (uploadFolderType == UploadFolderType.Unknown) return BadRequest("Invalid folder selection");
            string baseDirectoryPath = _dirConfig.CommunitySpotlight.DiskPath; // Change to default directory path

            if (uploadFolderType == UploadFolderType.CommunitySpotlight)
            {
                baseDirectoryPath = _dirConfig.CommunitySpotlight.DiskPath;
            }

            List<string> files = Directory.GetFiles(baseDirectoryPath).ToList();
            return Ok(files);
        }

        /// <summary>
        /// Upload an image to the server
        /// </summary>
        /// <param name="uploadFolderType">
        /// The type of upload, determines where the file will be saved
        /// </param>
        [HttpPost("{uploadFolderType}")]
        [Authorize]
        public async Task<IActionResult> Create(UploadFolderType uploadFolderType)
        {
            if (uploadFolderType == UploadFolderType.Unknown) return BadRequest("Invalid folder selection");

            string webPath = _dirConfig.CommunitySpotlight.WebPath;
            string baseDirectoryPath = _dirConfig.CommunitySpotlight.DiskPath; // Change to default directory path

            if (uploadFolderType == UploadFolderType.CommunitySpotlight)
            {
                webPath = _dirConfig.CommunitySpotlight.WebPath;
                baseDirectoryPath = _dirConfig.CommunitySpotlight.DiskPath;
            }

            try
            {
                IFormFileCollection files = Request.Form.Files;
                foreach (IFormFile file in files)
                {
                    if (file.Length <= 0) continue;

                    string origFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.ToString().Trim('"');
                    int fullStopIndex = origFileName.LastIndexOf(".", StringComparison.Ordinal);
                    if (fullStopIndex < 1) return BadRequest("Image name invalid");

                    string origFileNameStart = origFileName.Remove(fullStopIndex, origFileName.Length-fullStopIndex);
                    string origFileExtension = origFileName.Remove(0, origFileName.LastIndexOf(".", StringComparison.Ordinal));

                    int counter = 0;
                    string newFileName = origFileNameStart + origFileExtension;
                    string fullPath = Path.Combine(baseDirectoryPath, newFileName);
                    while (System.IO.File.Exists(fullPath))
                    {
                        counter++;
                        newFileName = origFileNameStart + counter + origFileExtension;
                        fullPath = _dirConfig.CommunitySpotlight.DiskPath + newFileName;

                        if (counter > 25) return BadRequest("More than 25 images with the same name");
                    }

                    webPath = _dirConfig.CommunitySpotlight.WebPath + newFileName;

                    await using FileStream stream = new FileStream(fullPath, FileMode.Create);
                    await file.CopyToAsync(stream);
                }

                return Ok(new { webPath });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        
        /// <summary>
        /// Upload an image to the server
        /// </summary>
        [HttpPost("FromYoutubeUrl")]
        [Authorize]
        public async Task<IActionResult> CreateFromUrl([FromBody] UploadFromYoutubeUrlViewModel vm)
        {
            YouTubeService youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = _authConfig.GoogleAuth.ApiKey,
                ApplicationName = "AssistantApps.NMS.WebApi"
            });

            VideosResource.ListRequest searchVideoRequest = youtubeService.Videos.List("snippet");
            searchVideoRequest.Id = vm.YoutubeId;
            VideoListResponse searchVideoResponse = await searchVideoRequest.ExecuteAsync();
            Video searchedVideo = searchVideoResponse.Items.FirstOrDefault();
            if (searchedVideo == null) return BadRequest("could not load video");
            
            ChannelsResource.ListRequest searchChannelRequest = youtubeService.Channels.List("snippet");
            searchChannelRequest.Id = searchedVideo.Snippet.ChannelId;
            ChannelListResponse searchChannelResponse = await searchChannelRequest.ExecuteAsync();
            Channel searchedChannel = searchChannelResponse.Items.FirstOrDefault();
            if (searchedChannel == null) return BadRequest("could not load channel");

            YoutubeVideoDataViewModel result = new YoutubeVideoDataViewModel
            {
                ChannelName = searchedVideo.Snippet.ChannelTitle,
                ChannelImageUrl = GetBestImageUrlFromThumbnailDetails(searchedChannel.Snippet.Thumbnails),
                PreviewUrl = GetBestImageUrlFromThumbnailDetails(searchedVideo.Snippet.Thumbnails),
                VideoName = searchedVideo.Snippet.Title,
            };

            string channelImgFilename = $"Channel-{searchedVideo.Snippet.ChannelId}";
            ResultWithValue<string> channelImgDownloadResult = DownloadImageToCommunitySpotlightFolder(result.ChannelImageUrl, channelImgFilename);
            if (channelImgDownloadResult.IsSuccess) result.ChannelImageUrl = channelImgDownloadResult.Value;

            string videoImgFilename = $"Video-{searchedVideo.Id}";
            ResultWithValue<string> videoImgDownloadResult = DownloadImageToCommunitySpotlightFolder(result.PreviewUrl, videoImgFilename);
            if (videoImgDownloadResult.IsSuccess) result.PreviewUrl = videoImgDownloadResult.Value;

            return Ok(result);
        }

        private ResultWithValue<string> DownloadImageToCommunitySpotlightFolder(string imageUrl, string filename)
        {
            const string fileExtension = ".jpg";
            string baseDirectoryPath = _dirConfig.CommunitySpotlight.DiskPath;

            try
            {
                using WebClient client = new WebClient();
                int fullStopIndex = imageUrl.LastIndexOf(".", StringComparison.Ordinal);
                if (fullStopIndex < 1) return new ResultWithValue<string>(false, string.Empty, "Image name invalid");

                string newFileName = filename + fileExtension;
                string fullPath = Path.Combine(baseDirectoryPath, newFileName);
                string webPath = _dirConfig.CommunitySpotlight.WebPath + newFileName;

                if (System.IO.File.Exists(fullPath)) return new ResultWithValue<string>(true, webPath, string.Empty);
                
                client.DownloadFile(new Uri(imageUrl), fullPath);
                return new ResultWithValue<string>(true, webPath, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResultWithValue<string>(false, string.Empty, $"Internal server error: {ex}");
            }
        }

        private string GetBestImageUrlFromThumbnailDetails(ThumbnailDetails details)
        {
            if (details.Maxres?.Url != null) return details.Maxres.Url;
            if (details.High?.Url != null) return details.High.Url;
            if (details.Medium?.Url != null) return details.Medium.Url;
            if (details.Standard?.Url != null) return details.Standard.Url;
            if (details.Default__?.Url != null) return details.Default__.Url;

            return string.Empty;
        }
    }
}